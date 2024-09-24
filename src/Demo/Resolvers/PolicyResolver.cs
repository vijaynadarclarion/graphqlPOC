using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Demo.Data;
using Demo.Entity;
using IdentityModel.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Identity.Client;
using NUPDemo.Entities;

namespace Demo.Resolvers;

public class PolicyResolver
{
    private readonly IDbContextFactory<AppReadOnlyNajmnetDbContext> _dbContextFactory;
    private readonly IHttpClientFactory _httpClientFactory;

    public PolicyResolver(
            IDbContextFactory<AppReadOnlyNajmnetDbContext> dbContextFactory, IHttpClientFactory httpClientFactory)
    {
        _dbContextFactory = dbContextFactory ??
            throw new ArgumentNullException(nameof(dbContextFactory));
        _httpClientFactory = httpClientFactory;
    }

    public async Task<List<VehicleDetail>> GetPolicies([Parent] AccidentParty accidentPartyInfo, [ScopedService] AppReadOnlyNajmnetDbContext context)
    {
        if(accidentPartyInfo == null)
            throw new ArgumentNullException(nameof(accidentPartyInfo));

        // Call the information from rest api

        var client = _httpClientFactory.CreateClient();

        try
        {
            if (accidentPartyInfo.VehicleId == null)
                return null;

            var accessToken = await GetAccessTokenAsync(client); // Method to retrieve OAuth access token

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await client.GetAsync("https://nnunified.najm.sa/api/v1/vehicles/" + accidentPartyInfo.VehicleId);
            if (response.IsSuccessStatusCode)
            {
                var jsonContent = await response.Content.ReadAsStringAsync();
                var vehicelInfo = JsonSerializer.Deserialize<VehicleInfo>(jsonContent); 
                return vehicelInfo.Value;
                // Process the successful response
            }
            else
            {
                return null;
            }

            /* var idVehicle = context.IdVehicles.FirstOrDefault(v => v.VehicleId == accidentPartyInfo.VehicleId);

             if (idVehicle != null && idVehicle.PolicyId != null)
             {
                 return context.IdPolicies
                     .Where(p => p.PolicyId == idVehicle.PolicyId)
                     .OrderByDescending(x => x.EffectiveGreDate);
             }*/
        }
        catch(Exception ex)
        {
            string exMsg = ex.Message;
        }
        finally
        {
            client.Dispose();

        }

        return null;
        
    }

    

    public async Task<string> GetAccessTokenAsync(HttpClient _httpClient)
    {   
        _httpClient.BaseAddress = new Uri("https://identitytest.najm.sa");
        var discoveryDocument = await _httpClient.GetDiscoveryDocumentAsync();
        _httpClient.DefaultRequestHeaders.Accept.Clear();
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
        var tokenResponse = await _httpClient.RequestClientCredentialsTokenAsync(
             new ClientCredentialsTokenRequest
             {
                 Address = discoveryDocument.TokenEndpoint,
                 ClientId = "ShamsClientTest",
                 ClientSecret = "JYPwH1qoQkme59dk2MdHSg",
                 Scope = "NNMotor.Read NNMotor.Write",
                 GrantType = "client_credentials"
             });

        return tokenResponse.AccessToken;

    }


}


public class VehicleListDetail
{
    public string PlateNumber { get; set; }
    public string AppliedInNajm { get; set; }
    public string AppliedInElm { get; set; }
    public string ElmRejectionreason { get; set; }
    public string Trans_Type { get; set; }
    public string Sys_Date { get; set; }
    public string First_Plat_Letter { get; set; }
    public string Second_Plate_Letter { get; set; }
    public string Third_Plate_Letter { get; set; }
}

public class VehicleDetail
{
    public int Vehicle_ID { get; set; }
    public int Policy_ID { get; set; }
    public string Sub_Policy_Number { get; set; }
    public string Defiend_By { get; set; }
    public string Plat_Number { get; set; }
    public string First_Plate_Letter { get; set; }
    public string Second_Plate_Letter { get; set; }
    public string Third_Plate_Letter { get; set; }
    public string Custom_ID { get; set; }
    public long Seq_Number { get; set; }
    public string Coverage_Type { get; set; }
    public string Plate_Type { get; set; }
    public string Issue_Gre_Date { get; set; }
    public string Effective_Gre_Date { get; set; }
    public string Expire_Gre_Date { get; set; }
    public string Manufacture { get; set; }
    public string Manufacturing_Year { get; set; }
    public string Model { get; set; }
    public string Color { get; set; }
    public string Chassis_Number { get; set; }
    public int? Customer_ID { get; set; }
    public string Personal_Accident_Coverage { get; set; }
    public string Geographic_Coverage_Gcc { get; set; }
    public List<VehicleListDetail> VehicleListDetail { get; set; }
}

public class VehicleInfo
{
    public List<object> Errors { get; set; }
    public bool IsValid { get; set; }
    public List<VehicleDetail> Value { get; set; }
}
