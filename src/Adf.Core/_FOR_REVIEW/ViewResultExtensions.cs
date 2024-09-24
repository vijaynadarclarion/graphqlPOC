﻿//// --------------------------------------------------------------------------------------------------------------------
//// <copyright file="ViewResultExtensions.cs" company="Usama Nada">
////   No Copyright .. Copy, Share, and Evolve.
//// </copyright>
//// --------------------------------------------------------------------------------------------------------------------

//namespace Adf.Core.Extensions
//{
//    #region usings

//    using System.IO;
//    using System.Linq;
//    using System.Text;

//    using Microsoft.AspNetCore.Http;
//    using Microsoft.AspNetCore.Mvc;
//    using Microsoft.AspNetCore.Mvc.Controllers;
//    using Microsoft.AspNetCore.Mvc.Rendering;
//    using Microsoft.AspNetCore.Routing;
//    using Microsoft.Extensions.DependencyInjection;
//    using Microsoft.Extensions.Options;

//    #endregion

//    /// <summary>
//    /// The view result extensions.
//    /// </summary>
//    public static class ViewResultExtensions
//    {
//        /// <summary>
//        /// The to html.
//        /// </summary>
//        /// <param name="result">
//        /// The result.
//        /// </param>
//        /// <param name="httpContext">
//        /// The http context.
//        /// </param>
//        /// <returns>
//        /// The <see cref="string"/>.
//        /// </returns>
//        public static string ToHtml(this ViewResult result, HttpContext httpContext)
//        {
//            var feature = httpContext.Features.Get<IRoutingFeature>();
//            var routeData = feature.RouteData;
//            var viewName = result.ViewName ?? routeData.Values["action"] as string;
//            var actionContext = new ActionContext(httpContext, routeData, new ControllerActionDescriptor());
//            var options = httpContext.RequestServices.GetRequiredService<IOptions<MvcViewOptions>>();
//            var htmlHelperOptions = options.Value.HtmlHelperOptions;
//            var viewEngineResult = result.ViewEngine?.FindView(actionContext, viewName, true) ?? options.Value
//                                       .ViewEngines.Select(x => x.FindView(actionContext, viewName, true))
//                                       .FirstOrDefault(x => x != null);
//            var view = viewEngineResult.View;
//            var builder = new StringBuilder();

//            using (var output = new StringWriter(builder))
//            {
//                var viewContext = new ViewContext(
//                    actionContext,
//                    view,
//                    result.ViewData,
//                    result.TempData,
//                    output,
//                    htmlHelperOptions);

//                view.RenderAsync(viewContext).GetAwaiter().GetResult();
//            }

//            return builder.ToString();
//        }
//    }
//}
