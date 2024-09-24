using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using HotChocolate;
using IdentityModel.Client;
using Najm.GraphQL.ApplicationCore._Interfaces;
using Najm.GraphQL.ApplicationCore.Entity;

namespace Najm.GraphQL.Infrastructure.Accidents.Resolvers;

public class PolicyResolver : IPolicyResolver
{
   // private HttpClient _httpClient;
    private readonly IHttpClientFactory _httpClientFactory;



    public PolicyResolver(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }


    public async Task<List<VehicleDetail>> GetPolicies(decimal vehicleId)
    {    
        using (var httpClient = _httpClientFactory.CreateClient())
        { //assign httpclientfactory instance to httpclient
            httpClient.Timeout = TimeSpan.FromSeconds(100); // _externalClientOption.Timeout;
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));


            var accessToken = await GetAccessTokenAsync(httpClient); // Method to retrieve OAuth access token
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await httpClient.GetAsync("https://nnunified.najm.sa/api/v1/vehicles/" + vehicleId);
            if (response.IsSuccessStatusCode)
            {
                var jsonContent = await response.Content.ReadAsStringAsync();
                var vehicelInfo = JsonSerializer.Deserialize<VehicleInfo>(jsonContent);
                return vehicelInfo.Value;
                // Process the successful response
            }
        }

        return null;
    }

    private async Task<string> GetAccessTokenAsync(HttpClient _httpClient)
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


