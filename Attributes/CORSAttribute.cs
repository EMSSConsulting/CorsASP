using CORSProvider.HttpFeatures;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.Framework.DependencyInjection;
using System;
using System.Linq;

namespace AntennaPortal.Middleware
{
    [System.AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true)]
    public class CORSAttribute : AuthorizationFilterAttribute
    {
        public CORSAttribute()
        {

        }
        
        public bool AllowCredentials { get; set; }

        public string AllowHeaders { get; set; }

        public string[] AllowMethods { get; set; }

        private const string OriginHeader = "Origin";
        private const string RequestHeadersHeader = "Access-Control-Request-Headers";
        private const string RequestMethodHeader = "Access-Control-Request-Method";


        private const string AllowOriginHeader = "Access-Control-Allow-Origin";
        private const string AllowHeadersHeader = "Access-Control-Allow-Headers";
        private const string AllowMethodsHeader = "Access-Control-Allow-Methods";
        private const string AllowCredentialsHeader = "Access-Control-Allow-Credentials";

        public override void OnAuthorization(AuthorizationContext context)
        {
            AddCORSHeaders(context.HttpContext);
            if (context.HttpContext.Request.Method != "OPTIONS")
                base.OnAuthorization(context);
            else context.Result = new HttpStatusCodeResult(200);
        }

        protected virtual void AddCORSHeaders(HttpContext context)
        {
            if (context.Request.Headers.ContainsKey(OriginHeader))
            {
                var origin = context.Request.Headers.Get(OriginHeader);

                var corsConfig = context.ApplicationServices.GetRequiredService<ICORSConfiguration>();
                if (corsConfig != null)
                {
                    if (corsConfig.AllowedOrigins != null && !corsConfig.AllowedOrigins.Contains(origin))
                        origin = corsConfig.AllowedOrigins.FirstOrDefault() ?? "*";
                }

                context.Response.Headers.Set(AllowOriginHeader, origin);

                if (AllowCredentials)
                    context.Response.Headers.Set(AllowCredentialsHeader, "true");

                context.Response.Headers.Set(AllowHeadersHeader, AllowHeaders ?? context.Request.Headers.Get(RequestHeadersHeader) ?? "*");
                context.Response.Headers.Set(AllowMethodsHeader, AllowMethods?.Union(new[] { "OPTIONS" }).Aggregate((x, y) => x + ", " + y) ?? context.Request.Headers.Get(RequestMethodHeader) ?? "*");
            }
        }
    }
}