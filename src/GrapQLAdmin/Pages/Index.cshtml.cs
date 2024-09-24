using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace GrapQLAdmin.Pages;


public class AuthorizationConfig
{
    public List<AuthorizationRule> Authorization { get; set; }
}

public class AuthorizationRule
{
    public string Name { get; set; }

    public bool Value { get; set; }
}

public class Schema
{
    [JsonPropertyName("data")]
    public Data Data { get; set; }
}

public class Data
{
    [JsonPropertyName("__schema")]
    public SchemaDetails SchemaDetails { get; set; }
}

public class SchemaDetails
{
    [JsonPropertyName("queryType")]
    public QueryType QueryType { get; set; }

    [JsonPropertyName("mutationType")]
    public MutationType MutationType { get; set; }

    [JsonPropertyName("subscriptionType")]
    public SubscriptionType SubscriptionType { get; set; }

    [JsonPropertyName("types")]
    public List<TypeElement> Types { get; set; }

    [JsonPropertyName("directives")]
    public List<Directive> Directives { get; set; }
}

public class QueryType
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
}

public class MutationType
{
    // Add properties if mutation type has any unique identifiers or structures
}

public class SubscriptionType
{
    // Add properties if subscription type has any unique identifiers or structures
}

public class TypeElement
{
    [JsonPropertyName("kind")]
    public string Kind { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("fields")]
    public List<Field> Fields { get; set; }

    [JsonPropertyName("inputFields")]
    public List<Field> InputFields { get; set; }

    
    // Include other properties such as inputFields, interfaces, etc., depending on your JSON structure
}

public class Field
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("args")]
    public List<Argument> Args { get; set; }

    [JsonPropertyName("type")]
    public FieldType Type { get; set; }

    [JsonPropertyName("isDeprecated")]
    public bool IsDeprecated { get; set; }

    [JsonPropertyName("deprecationReason")]
    public string DeprecationReason { get; set; }

    public bool IsSelected { get; set; }

}

public class Argument
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("type")]
    public object Type { get; set; }

    [JsonPropertyName("defaultValue")]
    public string DefaultValue { get; set; }
}

public class FieldType
{
    [JsonPropertyName("kind")]
    public string Kind { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("ofType")]
    public TypeOfType OfType { get; set; }
}

public class TypeOfType
{
    [JsonPropertyName("kind")]
    public string Kind { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }
    // Add more properties as needed
}

public class Directive
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("locations")]
    public List<string> Locations { get; set; }

    [JsonPropertyName("args")]
    public List<Argument> Args { get; set; }
}


public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    public string Message { get; set; }
    private readonly GraphQLHttpClient _client;
    string endpointUrl = "";
    string filePath = ""; // Specify the path to your JSON file

    string introspectionQuery = @"
    query IntrospectionQuery {
        __schema {
            queryType { name }
            mutationType { name }
            subscriptionType { name }
            types {
                kind
                name
                description
                fields(includeDeprecated: true) {
                    name
                    description
                    args {
                        name
                        description
                        type {
                            name
                            kind
                            ofType {
                                name
                                kind
                            }
                        }
                        defaultValue
                    }
                    type {
                        kind
                        name
                        ofType {
                            name
                            kind
                            ofType {
                                name
                                kind
                                ofType {
                                    name
                                    kind
                                    ofType {
                                        name
                                        kind
                                    }
                                }
                            }
                        }
                    }
                    isDeprecated
                    deprecationReason
                }
                inputFields {
                    name
                    description
                    type {
                        name
                        kind
                        ofType {
                            name
                            kind
                        }
                    }
                    defaultValue
                }
                interfaces {
                    name
                    kind
                    ofType {
                        name
                        kind
                    }
                }
                enumValues(includeDeprecated: true) {
                    name
                    description
                    isDeprecated
                    deprecationReason
                }
                possibleTypes {
                    name
                    kind
                    ofType {
                        name
                        kind
                    }
                }
            }
            directives {
                name
                description
                locations
                args {
                    name
                    description
                    type {
                        name
                        kind
                        ofType {
                            name
                            kind
                        }
                    }
                    defaultValue
                }
            }
        }
    }";

    [BindProperty]
    public List<string> SelectedFields { get; set; } // To hold the selected checkboxes
    [BindProperty]
    public string Role { get; set; }

    public bool ConfigFileSet { get; set; }
    [BindProperty]
    public int SelectedUser { get; set; }

    // private readonly IConfiguration _hostingEnvironment;

    public IndexModel(ILogger<IndexModel> logger, IConfiguration configuration)
    {
        _logger = logger;
        filePath = configuration.GetValue<string>("SettingPath");
        endpointUrl = configuration.GetValue<string>("GraphQLUrl");
        SelectedUser = 1;
        var httpClientHandler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
        };

        var httpClient = new HttpClient(httpClientHandler);

        _client = new GraphQLHttpClient(endpointUrl, 
            new NewtonsoftJsonSerializer(), httpClient);

    }

    public void OnGet()
    {

    }


    
    public SchemaDetails Schema { get; set; }
    public bool SchemaAvailable { get; set; }
    
    public void OnPostClearSetting()
    {
        Schema = null;
        System.IO.File.Delete(filePath);
    }
    
    public void OnPostLoadSchema()
    {
        string sch = FetchSchemaAsync(true);
        Schema = System.Text.Json.JsonSerializer.Deserialize<SchemaDetails>(sch);

        if(!System.IO.File.Exists(filePath))
        {
            ConfigFileSet = false;
            return;
        }
        string jsonString = System.IO.File.ReadAllText(filePath);

        AuthorizationConfig setting = System.Text.Json.JsonSerializer.Deserialize<AuthorizationConfig>(jsonString);
        ConfigFileSet = true;
        if (setting == null || setting.Authorization.Count == 0)
        {
            foreach (var type in Schema.Types)
            {
                
                if ((type.Kind == "OBJECT" || type.Kind == "INPUT_OBJECT") && (type.Name == "Query" || type.Name == "AccidentInfo" || type.Name == "AccidentParty" || type.Name == "AccidentsInput" || type.Name == "QuoteResponse"))
                {
                    if (type.Kind == "INPUT_OBJECT")
                    {
                        foreach (var field in type.InputFields  )
                        {
                            field.IsSelected = true;
                        }
                    }
                    else
                    {
                        foreach (var field in type.Fields)
                        {
                            field.IsSelected = true;
                        }
                    }
                }
            }

            return;
        }
       // dynamic expando = new ExpandoObject();
       // expando = Schema;

        foreach (var type in Schema.Types)
        {
            
            if ((type.Kind == "OBJECT"  || type.Kind == "INPUT_OBJECT") && (type.Name == "Query" || type.Name == "AccidentInfo" || type.Name == "AccidentParty" || type.Name == "AccidentsInput" || type.Name == "QuoteResponse"))
            {
                if (type.Kind == "INPUT_OBJECT")
                {
                    foreach (var field in type.InputFields)
                    {
                        string typename = string.Format("{0}.{1}", type.Name, field.Name).ToLower();
                        // field.selected = false;
                        if (setting.Authorization.Any(x => x.Name == typename))
                        {
                            field.IsSelected = true;
                            //  SchemaAvailable = true;
                            // field.selected = true;

                        }
                    }
                }
                else
                {
                    foreach (var field in type.Fields)
                    {
                        string typename = string.Format("{0}.{1}", type.Name, field.Name).ToLower();
                        // field.selected = false;
                        if (setting.Authorization.Any(x => x.Name == typename))
                        {
                            field.IsSelected = true;
                            //  SchemaAvailable = true;
                            // field.selected = true;

                        }
                    }
                }

                
            }
        }

        //Schema = expando;
    }

    public void OnPostSaveSchema()
    {
        AuthorizationConfig authorizationConfig = new AuthorizationConfig();
        authorizationConfig.Authorization = new List<AuthorizationRule>();

        foreach (var field in SelectedFields)
        {
            AuthorizationRule rule = new AuthorizationRule();
            rule.Name = field.ToLower();
            rule.Value = true;
            authorizationConfig.Authorization.Add(rule);
        }

        string json = JsonConvert.SerializeObject(authorizationConfig, Formatting.Indented);
        System.IO.File.Delete(filePath);
        // Write JSON to file
        System.IO.File.WriteAllText(filePath, json);       

    }

    public void OnPostGenerateToken()
    {
        Message = GenerateToken("demo");
    }

    public void OnPostSaveAuthorizationSettings()
    {
        AuthorizationConfig authorizationConfig = new AuthorizationConfig();
        authorizationConfig.Authorization = new List<AuthorizationRule>();

        foreach (var field in SelectedFields)
        {
            AuthorizationRule rule = new AuthorizationRule();
            rule.Name = field.ToLower();
            rule.Value = true;
            authorizationConfig.Authorization.Add(rule);
        }

        string json = JsonConvert.SerializeObject(authorizationConfig, Formatting.Indented);
       /// string filePath = "D:\\VijayN\\Najm\\Najm-NajmNet-API-GrapQL-POC\\src\\Najm.GraphQL.WebAPI\\SchemaAuth\\schemasetting.json"; // Specify the path to your JSON file
        System.IO.File.Delete(filePath);
        // Write JSON to file
        System.IO.File.WriteAllText(filePath, json);
    }

    public string FetchSchemaAsync(bool isAdmin)
    {
        var query = new GraphQLHttpRequest
        {
            Query = introspectionQuery
        };
        var tempRole = Role;
        if(isAdmin)
        {
            Role = "admin";
        }
        string token = GenerateToken("demo");
        _client.HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
        Role = tempRole;
        var response = _client.SendQueryAsync<dynamic>(query).Result;
        return response.Data.__schema.ToString();
    }
    public string GenerateToken(string username)
    {
       /* using (var rng = new RNGCryptoServiceProvider())
        {
            var key2 = new byte[256];
            rng.GetBytes(key2);
            string str =  Convert.ToBase64String(key2);
        }*/

        /*if(SelectedUser > 0)
        {
            var client = new HttpClient();

            var disco = client.GetDiscoveryDocumentAsync("https://localhost:5001/").Result;

            Dictionary<int, string> clientMapp = new Dictionary<int, string>();
            clientMapp.Add(1, "Admin");
            clientMapp.Add(3, "IC1");
            clientMapp.Add(4, "IC2");

            var request = new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = clientMapp[SelectedUser],
                ClientSecret = "secret",

                Scope = "api1"
            };
            // request token
            var tokenResponse = (client.RequestClientCredentialsTokenAsync(request)).Result;

            return tokenResponse?.AccessToken;

        }
        return null;*/

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes("nawlvTMgHZxe5Z9kWtrIyYxFVExw+7qH6iL7Kz4F7Iw+pxCGLFD4C2iDY7aW6zopnKMsxDRzbIkV7z+0mbVYzuT2mxYFDYLpV3H4huNfIVzAMTtVHCqSWzCTepv4+Tfl/AHorfw1AJ8jFBw58I3mOyR1/hlmFVnYDcrgMyCCMl7iemUdbESP1ahtF4inyvFQ66nWLSTg39J6lOm7vHyOCyh1wWftWDWxsdfqeIwwLRPCyiA0z+Pf9gEo7AVJggyZoIPnfAYzn6v9VEZ9raA0rgtqNOibFtinzpvVw7xLo99JtsrS9d4CIvXhJMK9adUhZzfE9ZStjhWJWn6CE1iWyg==");  // Replace with your actual secret key

        List<Claim> claims = new List<Claim>();
        claims.Add(new Claim(ClaimTypes.Name, username));
        claims.Add(new Claim(ClaimTypes.Role, Role != null && Role == "admin" ? "Admin" : "User"));
        foreach (var field in SelectedFields)
        {
            claims.Add(new Claim(field.ToLower(), "true"));
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(1),  // Token expiry time
            Issuer = "demo",
            Audience = "demoapi",
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

       
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

}
