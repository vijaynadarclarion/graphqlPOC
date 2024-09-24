using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.Results;
using HotChocolate;
using HotChocolate.Execution;
using HotChocolate.Subscriptions;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using Najm.GraphQL.ApplicationCore.CompQuotes;
using Najm.GraphQL.ApplicationCore.CompQuotes.Validators;
using Najm.GraphQL.ApplicationCore.Types;

namespace Najm.GraphQL.ApplicationCore.Types;



public class QuoteRequest
{
    public string RequestId { get; set; }
    // Add any other fields necessary for the request
}

public class QuoteEntity
{
    public string RequestReferenceNo { get; set; }
    public string QuoteReferenceNo { get; set; }
    public bool Status { get; set; }
    public int PolicyTitleID { get; set; }
    public decimal MaxLiability { get; set; }
    public DateTime PolicyEffectiveDate { get; set; }
    public DateTime PolicyExpiryDate { get; set; }
}

public class QuoteResponse
{
    public string RequestId { get; set; }  // This should exist
    public List<string> Errors { get; set; } = new List<string>();


    public QuoteEntity Data { get; set; }  // Add the Data field if needed


}


    // Helper method to generate a random date
  
public class Subscription
{
    private readonly ITopicEventSender _eventSender;
    private readonly HttpClient _httpClient;

    public Subscription(ITopicEventSender eventSender)
    {
        _eventSender = eventSender;
        _httpClient = new HttpClient();
    }

    [SubscribeAndResolve]
    public async IAsyncEnumerable<CompQuotesSubResList> compQuoteRequest(CompQuotesRequestDto input , 
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        if (input.GroupID == null)
        {
            // Create a response object to carry the error message
            var compQuotesResList = new CompQuotesSubResList();
            compQuotesResList.Errors = new List<CompQuotesSubResError>();
            CompQuotesSubResError error = new CompQuotesSubResError()
            {
                Code = "",
                Message = "GroupId or no input parameter assigned"
            };
            compQuotesResList.Errors.Add(error);

            // Return the error response
            yield return compQuotesResList;
            yield break; // Stop further execution
        }

        // Validate the input using the validator
        var validator = new CompQuotesRequestValidator();
        var validationResult = validator.Validate(input);

        if (!validationResult.IsValid)
        {
            // Map the errors to CompQuotesSubResError with both Code and Message
            var errors = validationResult.Errors.Select(e => new CompQuotesSubResError
            {
                Code = string.IsNullOrEmpty(e.ErrorCode) ? "VALIDATION_ERROR" : e.ErrorCode, // Default code if ErrorCode is empty
                Message = e.ErrorMessage
            }).ToList();

            // If validation fails, return the error messages
            var errorResponse = new CompQuotesSubResList
            {
                Errors = errors
            };

            yield return errorResponse;
            yield break; // Stop further execution if validation fails
        }

        // List of endpoints (add more as needed)

        var endpoints = new Dictionary<int, string>();
        endpoints.Add(1, "https://dummyjson.com/quotes/random");
        endpoints.Add(2, "https://dummyjson.com/quotes/random");
        endpoints.Add(3, "https://dummyjson.com/quotes/random");
        endpoints.Add(4, "https://dummyjson.com/quotes/random");
        endpoints.Add(5, "https://dummyjson.com/quotes/random");
        endpoints.Add(6, "https://dummyjson.com/quotes/random");
        endpoints.Add(7, "https://dummyjson.com/quotes/random");
        endpoints.Add(8, "https://dummyjson.com/quotes/random");
        endpoints.Add(9, "https://dummyjson.com/quotes/random");
        endpoints.Add(10, "https://dummyjson.com/quotes/random");


        // Create a list of tasks with random delays
        var tasks = endpoints.Select(async endpoint =>
        {
            // Introduce a random delay (between 1 and 10 seconds)
            await Task.Delay(new Random().Next(1000, 10000));
            return await FetchAndPublishData(endpoint, input);
        }).ToList();

        // Fetch and process results
        while (tasks.Any())
        {
            var finishedTask = await Task.WhenAny(tasks);
            tasks.Remove(finishedTask);

            var result = await finishedTask;
            yield return result;
        }

    }


    private async Task<CompQuotesSubResList> FetchAndPublishData(KeyValuePair<int, string> endpoint, CompQuotesRequestDto input)
    {
        var random = new Random();
        var quoteResponse = new CompQuotesSubResList
        {
            RequestReferenceNo = input.RequestReferenceNo,
            InsuranceCompanyCode = endpoint.Key,
            Entity = new CompQuotesSubEntity
            {
                QuoteReferenceNo = random.Next(1000, 9999), // Random quote number
                PolicyHolderEligibility = random.Next(0, 2),
                PolicyTitleID = random.Next(1, 10),
                MaxLiability = $"{random.Next(100000, 1000000)} USD",
                PolicyEffectiveDate = DateTime.UtcNow.AddDays(random.Next(-365, 0)).ToString("yyyy-MM-dd"),
                PolicyExpiryDate = DateTime.UtcNow.AddDays(random.Next(1, 365)).ToString("yyyy-MM-dd"),
                VehicleSumInsured = random.Next(10000, 50000),
                HasTrailer = random.Next(0, 2) == 1,
                TrailerSumInsure = random.NextDouble() * 10000,
                TotalLossPercentage = random.NextDouble() * 100,
                Deductibles = new Deductibles(),
                PolicyPremiumFeatures = new PolicyPremiumFeatures(),
                DriverDetail = new DriverDetail(),
                InspectionTypeID = random.Next(1, 5),
                AdditionalCoverage = new AdditionalCoverage(),
                Field1 = GenerateRandomString(10), // Random alphanumeric string
                Field2 = GenerateRandomString(10), // Random alphanumeric string
                Field3 = GenerateRandomString(10), // Random alphanumeric string
                Field4 = GenerateRandomString(10), // Random alphanumeric string
                Field5 = GenerateRandomString(10),
                Field6 = GenerateRandomString(10),
                Field7 = GenerateRandomString(10),
                Field8 = GenerateRandomString(10),
                Field9 = GenerateRandomString(10),
                Field10 = GenerateRandomString(10),
                Field11 = GenerateRandomString(10),
                Field12 = GenerateRandomString(10),
                Field13 = GenerateRandomString(10),
                Field14 = GenerateRandomString(10),
                Field15 = GenerateRandomString(10)
            }
           
            };

        return quoteResponse;
    }


    public string GenerateRandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }


}
