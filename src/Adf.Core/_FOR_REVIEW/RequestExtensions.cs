﻿//using System;
//using System.Collections.Generic;
//using System.Text;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Http.Extensions;

//namespace Framework.Core.Extensions
//{
//    public static class RequestExtensions
//    {
//        public static string GetPathWithSubDomain(this HttpRequest request)
//        {
//            var fullUrl = request.GetDisplayUrl();
//            fullUrl = fullUrl.Remove(0, fullUrl.LastIndexOf(request.Host.ToString(), StringComparison.Ordinal)).Replace(request.Host.ToString(), "");
//            var subDomain = fullUrl.Remove(fullUrl.IndexOf(request.Path.ToString(), StringComparison.Ordinal));
//            var fullUrlWithSubDomain = string.IsNullOrEmpty(subDomain) ? request.Path.ToString() : (
//                (subDomain.StartsWith("/")
//                    ? subDomain
//                    : $"/{subDomain}") + request.Path);
//            return fullUrlWithSubDomain;
//        }
//    }
//}
