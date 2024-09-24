using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Najm.GraphQL.ApplicationCore.Types;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class SseController : ControllerBase
{
    private static readonly ConcurrentDictionary<string, ConcurrentQueue<string>> _eventStreams = new();
    private readonly ILogger<SseController> _logger;
    private readonly HttpClient _httpClient;

    public SseController(ILogger<SseController> logger)
    {
        _logger = logger;
        _httpClient = new HttpClient();
    }

    [HttpGet("{requestId}")]
    public async Task Get(string requestId, [FromQuery] List<string> fields, CancellationToken cancellationToken)
    {
        Response.Headers.Add("Content-Type", "text/event-stream");

        var queue = _eventStreams.GetOrAdd(requestId, _ => new ConcurrentQueue<string>());

        // Fetch data from REST APIs in parallel
        var endpoints = new List<string> { "https://api.endpoint1.com", "https://api.endpoint2.com" /* add your endpoints here */ };
        var tasks = endpoints.Select(endpoint => FetchAndPublishData(endpoint, requestId, fields)).ToList();

        while (!cancellationToken.IsCancellationRequested)
        {
            if (queue.TryDequeue(out var message))
            {
                await Response.WriteAsync($"data: {message}\n\n", cancellationToken);
                await Response.Body.FlushAsync(cancellationToken);
            }
            else
            {
                await Task.Delay(1000, cancellationToken);
            }
        }

        await Task.WhenAll(tasks);
        _eventStreams.TryRemove(requestId, out _);
    }

    private async Task FetchAndPublishData(string endpoint, string requestId, List<string> fields)
    {
        var response = await _httpClient.GetStringAsync(endpoint);

        var quoteResponse = new QuoteResponse
        {
            RequestId = requestId,
            Data = response
        };

        var message = Newtonsoft.Json.JsonConvert.SerializeObject(quoteResponse);
        PublishEvent(requestId, message);
    }

    public static void PublishEvent(string requestId, string message)
    {
        if (_eventStreams.TryGetValue(requestId, out var queue))
        {
            queue.Enqueue(message);
        }
    }
}
