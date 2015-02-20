using CORSProvider.HttpFeatures;
using Microsoft.Framework.DependencyInjection;
using System;

namespace CORSProvider
{
    public static class CORSExtensions
    {
        public static IServiceCollection AddCORS(this IServiceCollection services, CORSConfiguration configuration = null)
        {
            return services.AddInstance<ICORSConfiguration>(configuration ?? new CORSConfiguration());
        }
    }
}