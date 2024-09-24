using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Adf.Core.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

/*
namespace Adf.Core.Serilog
{
    public static class SerilogHelper
    {
        public static bool IsSerilogConfigured { get; private set; } = false;

        private static IHttpContextAccessor HttpContextAccessor;
        public static void ConfigureContext(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        private static HttpContext GetHttpContextInternal()
        {
            if (HttpContextAccessor == null)
            {
                return null;
            }
            return HttpContextAccessor.HttpContext;
        }

        public static T GetApplicationService<T>()
        {
            try
            {
                return GetHttpContext().RequestServices.GetRequiredService<T>();
            }
            catch (Exception ex)
            {
                SerilogHelper.LogDebug($"Class:SerilogHelper, Method:GetApplicationService, Message: {ex.GetFormattedErrorMessage()}");
                return default(T);
            }
        }

        public static HttpContext GetHttpContext() { return GetHttpContextInternal(); }

        public static void ConfigureNajmSeriLog(IConfiguration seriLogConfiguration)
        {
            // Set true to disable log
            bool isLogEnable = seriLogConfiguration.GetValue<bool>("Serilog:IsLogEnable");
            Log.Logger = new LoggerConfiguration()
                           .Filter.ByExcluding(_ => !isLogEnable)
                           .Enrich.WithHttpRequestId()
                           .ReadFrom.Configuration(seriLogConfiguration)
                           .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Warning)
                           .CreateLogger();
            IsSerilogConfigured = true;
            // Need to make path dynamic and append data everytime 
            // Also need to set message template with custom result
            bool selfLogEnable = seriLogConfiguration.GetValue<bool>("Serilog:SelfLogEnable");
            if (selfLogEnable)
            {
                string selfLogFilePath = seriLogConfiguration.GetValue<string>("Serilog:SelfLogFilePath");
                if (!Directory.Exists(selfLogFilePath))
                {
                    Directory.CreateDirectory(selfLogFilePath);
                }
                string applicationName = seriLogConfiguration.GetSection("Serilog:Properties").GetValue<string>("ApplicationName");
                var filename = selfLogFilePath + string.Format("{0}_{1}.txt", applicationName, DateTime.Now.ToString("yyyy-MM-dd"));
                var file = File.AppendText(filename);
                //SelfLog.Enable(TextWriter.Synchronized(file));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enrichmentConfiguration"></param>
        /// <returns></returns>
        public static LoggerConfiguration WithHttpRequestId(this LoggerEnrichmentConfiguration enrichmentConfiguration)
        {
            if (enrichmentConfiguration == null) throw new ArgumentNullException(nameof(enrichmentConfiguration));
            return enrichmentConfiguration.With<HttpRequestIdEnricher>();
        }

        /// <summary>
        /// To Log Debug Messages 
        /// </summary>
        public static void LogDebug(string message, string userName = "")
        {
            GetDefaultLogger(null, userName).Debug(message);
        }

        /// <summary>
        /// To Log Debug Messages with custom properties
        /// </summary>
        public static void LogDebug(LogModel logModel)
        {
            GetDefaultLogger(logModel).Debug(logModel.Message);
        }

        /// <summary>
        /// To Log Information Messages 
        /// </summary>
        public static void LogInformation(string message, string userName = "")
        {
            GetDefaultLogger(null, userName).Information(message);
        }

        /// <summary>
        /// To Log Information Messages with custom properties
        /// </summary>
        public static void LogInformation(LogModel logModel)
        {
            GetDefaultLogger(logModel).Information(logModel.Message);
        }
        public static void AuditLogInformation(string message, string request = "", string response = "")
        {
            GetDefaultLoggerAuditInfo(request, response).Information(message);
        }

        /// <summary>
        /// To Log Exception details with custom properties
        /// </summary>
        public static void LogError(Exception ex, string message = "", string userName = "")
        {
            if (message.Trim().Length == 0)
            {
                message = ex.GetFormattedErrorMessage();
            }
            HttpContext httpContext = GetHttpContext();
            if (httpContext != null)
            {
                string Request = GetRequestBody(httpContext.Request).ConfigureAwait(false).GetAwaiter().GetResult();
                GetDefaultLogger(null, userName).ForContext("LogLevel", LogEventLevel.Error.ToString()).Error(ex, message);
            }
            else
            {

                GetDefaultLogger(null, userName).ForContext("LogLevel", LogEventLevel.Error.ToString()).Error(ex, message);
            }
        }

        public static void LogError(string message, string userName = "")
        {
            if (message.Trim().Length > 0)
            {
                GetDefaultLogger(null, userName).ForContext("Message", message);
            }
        }

       
        public static string GetFormattedErrorMessage(this Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace(ex, true);
            sb.AppendLine($"Method-> {trace?.GetFrame(0)?.GetMethod()?.ReflectedType?.FullName}");
            sb.AppendLine($"LineNumber-> {Convert.ToString(trace?.GetFrame(0)?.GetFileLineNumber())}");
            sb.AppendLine($"Column-> {Convert.ToString(trace?.GetFrame(0)?.GetFileColumnNumber())}");

            sb.AppendLine($"Message-> {ex.Message}");
            if (ex.InnerException != null)
            {
                sb.AppendLine($"InnerMessage-> {ex.InnerException.Message}");
            }
            sb.AppendLine($"StackTrace-> {ex.StackTrace}");
            return sb.ToString();
        }

        public static string GetAbsoluteUrl(HttpRequest httpRequest)
        {
            UriBuilder uriBuilder = new UriBuilder();
            uriBuilder.Scheme = httpRequest.Scheme;
            uriBuilder.Host = httpRequest.Host.Host;
            uriBuilder.Path = httpRequest.Path.ToString();
            uriBuilder.Query = httpRequest.QueryString.ToString();
            return uriBuilder.Uri.AbsoluteUri;
        }

        //private static IBrowser GetBrowserDetails()
        //{
        //    try
        //    {
        //        HttpContext httpContext = GetHttpContext();
        //        var userAgentStringSpan = httpContext.Request.Headers["User-Agent"][0].AsSpan();
        //        return Detector.GetBrowser(userAgentStringSpan);
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}

        private static ILogger GetDefaultLogger(LogModel logModel = null, string userName = "")
        {
            string machineName = null;
            string httpVerb = string.Empty;
            string url = null;
            string loggerType = null;
            DateTime createdOn = DateTime.Now;
            string createdBy = "1";

            HttpContext httpContext = GetHttpContext();
            string Request = "";
            if (httpContext != null)
            {
                machineName = httpContext.Request?.Host.Value;
                httpVerb = httpContext.Request?.Method;
                url = GetAbsoluteUrl(httpContext.Request);
                loggerType = null;

                // IBrowser browser = GetBrowserDetails();
                Request = GetRequestBody(httpContext.Request).ConfigureAwait(false).GetAwaiter().GetResult();
                if (logModel == null)
                {
                    if (string.IsNullOrEmpty(userName))
                    {
                       // IWebWorker webWorker = GetApplicationService<IWebWorker>();
                        //if (webWorker != null)
                        //{
                        //    userName = webWorker.UserName;
                        //}
                    }
                    //.ForContext("BrowserName", browser?.Name).ForContext("BrowserVersion", browser?.Version)
                    // .ForContext("OperatingSystem", browser?.OS)
                                      

                    return Log.Logger.ForContext("MachineName", machineName).ForContext("CreatedOn", createdOn).ForContext("CreatedBy", createdBy).ForContext("HttpVerb", httpVerb).ForContext("Url", url).ForContext("LoggerType", loggerType).ForContext("UserName", userName).ForContext("Request", Request);
                }
                else
                {
                    if (string.IsNullOrEmpty(logModel.Request))
                    {
                        logModel.Request = Request;
                    }
                    return Log.Logger.ForContext("CreatedOn", createdOn).ForContext("CreatedBy", createdBy).ForContext("UserName", logModel.UserName)
                       .ForContext("Request", logModel.Request);
                      // .ForContext("BrowserName", browser?.Name)
                      //.ForContext("BrowserVersion", browser?.Version).ForContext("OperatingSystem", browser?.OS);
                }
            }
            else
            {
                if (logModel == null)
                {
                    if (string.IsNullOrEmpty(userName))
                    {
                        //IWebWorker webWorker = GetApplicationService<IWebWorker>();
                        //if (webWorker != null)
                        //{
                        //    userName = webWorker.UserName;
                        //}
                    }

                    return Log.Logger.ForContext("MachineName", machineName).ForContext("CreatedOn", createdOn).ForContext("CreatedBy", createdBy).ForContext("HttpVerb", httpVerb).ForContext("Url", url).ForContext("LoggerType", loggerType).ForContext("UserName", userName).ForContext("Request", Request);
                }
                else
                {
                    if (string.IsNullOrEmpty(logModel.Request))
                    {
                        logModel.Request = Request;
                    }

                    return Log.Logger.ForContext("MachineName", machineName).ForContext("CreatedOn", createdOn).ForContext("CreatedBy", createdBy).ForContext("HttpVerb", httpVerb).ForContext("Url", url).ForContext("LoggerType", loggerType).ForContext("UserName", logModel.UserName).ForContext("Request", logModel.Request);
                }
            }
        }

       
        private static async Task<string> GetRequestBody(HttpRequest request)
        {
            HttpRequestRewindExtensions.EnableBuffering(request);
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            await request.Body.ReadAsync(buffer, 0, buffer.Length);
            var bodyAsText = Encoding.UTF8.GetString(buffer);
            request.Body.Seek(0, SeekOrigin.Begin);
            string Headers = request.Headers.GetRequestHeaders();
            return $"Request:{bodyAsText.Replace("\0", "").Trim()} | Headers:{Headers.Trim()}";
        }
        private static ILogger GetDefaultLoggerAuditInfo(string request = "", string response = "")
        {
            string httpVerb = string.Empty;
            string url = string.Empty;

            DateTime createdOn = DateTime.Now;
            string createdBy = "1";
            bool IsOutboudCall = true;

            HttpContext httpContext = GetHttpContext();

            if (httpContext != null)
            {
                httpVerb = httpContext.Request?.Method;
                url = GetAbsoluteUrl(httpContext.Request);
                return Log.Logger.ForContext("CreatedOn", createdOn).ForContext("CreatedBy", createdBy).ForContext("HttpVerb", httpVerb).ForContext("Url", url).ForContext("IsOutboudCall", IsOutboudCall).ForContext("RequestBody", request).ForContext("ResponseBody", response);
            }
            else
            {

                return Log.Logger;
            }

        }
    }
    /// <summary>
    /// Enrich log events with a HttpRequestId GUID.
    /// </summary>
    public class HttpRequestIdEnricher : ILogEventEnricher
    {
        /// <summary>
        /// The property name added to enriched log events.
        /// </summary>
        public const string HttpRequestIdPropertyName = "RequestId";

        static readonly string RequestIdItemName = typeof(HttpRequestIdEnricher).Name + "+RequestId";

        /// <summary>
        /// Enrich the log event with an id assigned to the currently-executing HTTP request, if any.
        /// </summary>
        /// <param name="logEvent">The log event to enrich.</param>
        /// <param name="propertyFactory">Factory for creating new properties to add to the event.</param>
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            if (logEvent == null) throw new ArgumentNullException("logEvent");
            Guid requestId;
            if (!TryGetCurrentHttpRequestId(out requestId))
                return;
            var requestIdProperty = new LogEventProperty(HttpRequestIdPropertyName, new ScalarValue(requestId));
            logEvent.AddPropertyIfAbsent(requestIdProperty);
        }

        /// <summary>
        /// Retrieve the id assigned to the currently-executing HTTP request, if any.
        /// </summary>
        /// <param name="requestId">The request id.</param>
        /// <returns><c>true</c> if there is a request in progress; <c>false</c> otherwise.</returns>
        public static bool TryGetCurrentHttpRequestId(out Guid requestId)
        {
            HttpContext httpContext = SerilogHelper.GetHttpContext();
            if (httpContext != null && httpContext.Items == null)
            {
                requestId = default(Guid);
                return false;
            }
            else if (httpContext == null)
            {
                requestId = default(Guid);
                return false;
            }

            var requestIdItem = httpContext.Items[RequestIdItemName];
            if (requestIdItem == null)
                httpContext.Items[RequestIdItemName] = requestId = Guid.NewGuid();
            else
                requestId = (Guid)requestIdItem;
            return true;
        }
    }

    public class LogModel
    {
        public static LogModel FromContext()
        {
            LogModel logModel = new LogModel();
            HttpContext httpContext = SerilogHelper.GetHttpContext();
            string ipAddress = httpContext.Connection.RemoteIpAddress.ToString();
            string action = httpContext.Request.Path;
            UriBuilder uriBuilder = new UriBuilder();
            uriBuilder.Scheme = httpContext.Request.Scheme;
            uriBuilder.Host = httpContext.Request.Host.Host;
            uriBuilder.Path = httpContext.Request.Path.ToString();
            uriBuilder.Query = httpContext.Request.QueryString.ToString();
            string requestURL = uriBuilder.Uri.AbsoluteUri;
            logModel.Action = action;
            logModel.IPAddress = ipAddress;
            logModel.RequestURL = requestURL;

            try
            {
                logModel.Message = string.Format("Request {0} {1} {2}", httpContext.Request.Method, httpContext.Request.Path, httpContext.Response.StatusCode.ToString());
            }
            catch
            {
                logModel.Message = "";
            }
            return logModel;
        }

        public string Action { get; set; }
        public string RequestURL { get; set; }
        public string IPAddress { get; set; }

        public string UserName { get; set; }
        public string Message { get; set; }
        public string Request { get; set; }
        public string Response { get; set; }

        public TimeSpan ExecutionTime { get; set; }
        public string SessionID { get; set; }
    }
}*/
