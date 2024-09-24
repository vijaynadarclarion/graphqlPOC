using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HotChocolate.Subscriptions;
using Najm.GraphQL.ApplicationCore.Accidents.Services;

namespace Najm.GraphQL.ApplicationCore.Types;
public class Mutation
{
    private readonly ITopicEventSender _eventSender;
    private readonly HttpClient _httpClient;

    public Mutation(ITopicEventSender eventSender)
    {
        _eventSender = eventSender;
        _httpClient = new HttpClient();
    }

    public async Task<QuoteRequest> FetchQuoteData()
    {
        // Generate a unique requestId
        var requestId = Guid.NewGuid().ToString();

        // Start fetching data from REST APIs
        var endpoints = new List<string> { "https://dummyjson.com/quotes/random", "https://dummyjson.com/quotes/random" /* add your endpoints here */ };

       // var tasks = endpoints.Select(endpoint => FetchAndPublishData(endpoint, requestId)).ToList();
     //   await Task.WhenAll(tasks);

        return new QuoteRequest { RequestId = requestId };
    }
    /*
    private async Task FetchAndPublishData(string endpoint, string requestId)
    {
        var response = await _httpClient.GetStringAsync(endpoint);
        var quoteResponse = new QuoteResponse
        {
            RequestId = requestId,
            Data = response
        };

        await _eventSender.SendAsync(requestId, quoteResponse);
    }*/
}


