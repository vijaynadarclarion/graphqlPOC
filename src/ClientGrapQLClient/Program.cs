// See https://aka.ms/new-console-template for more information
// Create the GraphQL client
using System.Net.WebSockets;
using System.Text;
using ClientGrapQLClient;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using HotChocolate;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
namespace GraphQLSubscriptionClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // TokenService tokenService = new TokenService();
            //string token = tokenService.GenerateToken();
            // Configure the GraphQL client
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) => true // Ignore SSL errors
            };

            var httpClient = new HttpClient(handler);

            // Create the GraphQL client with HTTP and WebSocket settings
            var client = new GraphQLHttpClient(new GraphQLHttpClientOptions
            {
                EndPoint = new Uri("https://localhost:7171/graphql"),                // Use the HTTP endpoint
                UseWebSocketForQueriesAndMutations = true,                           // Enable WebSocket support
                WebSocketEndPoint = new Uri("ws://localhost:7171/graphql")           // WebSocket endpoint
            }, new NewtonsoftJsonSerializer(), httpClient);  // Pass in the handler with SSL bypass


            // Define the subscription query
            var subscriptionQuery = new GraphQLRequest
            {
                Query = @"
               subscription {
                compQuoteRequest(input: {
                    requestReferenceNo: ""REQ112233789"",
                    quoteRequestSourceID: 302,
                    groupID: 5,
                    coverageTypeID: 5
                }) {
                    requestReferenceNo,
                    insuranceCompanyCode,
                    errors {
                      message,
                      code
                    },
                  entity {
                    maxLiability,
                    policyTitleID
                   }
                }
            }"
            };

            // Subscribe to the compQuoteRequest
            var observable = client.CreateSubscriptionStream<CompQuoteResponse>(subscriptionQuery);

            // Handle subscription response
            var subscription = observable.Subscribe(result =>
            {
                if (result.Errors != null && result.Errors.Length > 0)
                {
                    // Handle errors
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"Error: {error.Message}");
                    }
                }
                else
                {
                    // Handle the subscription data
                    var quote = result.Data.CompQuoteRequest;
                    Console.WriteLine($"Request Reference No: {quote.RequestReferenceNo}");
                    Console.WriteLine($"Insurance Company Code: {quote.InsuranceCompanyCode}");
                    Console.WriteLine($"Max Liability: {quote.Entity.MaxLiability}");
                    Console.WriteLine($"Policy Title ID: {quote.Entity.PolicyTitleID}");

                    if (quote.Errors != null)
                    {
                        foreach (var error in quote.Errors)
                        {
                            Console.WriteLine($"Error: {error.Message}, Code: {error.Code}");
                        }
                    }
                }
            });

            // Keep the console app running to receive updates
            Console.WriteLine("Listening for subscription updates...");
            Console.ReadLine();

            // Dispose subscription when done
            subscription.Dispose(); 
     
        }
    }

    // Define the response models
    public class CompQuoteResponse
    {
        public CompQuoteRequest CompQuoteRequest { get; set; }
    }

    public class CompQuoteRequest
    {
        public string RequestReferenceNo { get; set; }
        public string InsuranceCompanyCode { get; set; }
        public CompQuoteEntity Entity { get; set; }
        public CompQuoteError[] Errors { get; set; }
    }

    public class CompQuoteEntity
    {
        public decimal MaxLiability { get; set; }
        public int PolicyTitleID { get; set; }
    }

    public class CompQuoteError
    {
        public string Message { get; set; }
        public int Code { get; set; }
    }
}
