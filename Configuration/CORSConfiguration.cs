using System;
using System.Collections.Generic;

namespace CORSProvider.HttpFeatures
{
    public interface ICORSConfiguration
    {
        IEnumerable<string> AllowedOrigins { get; set; }
    }

    public class CORSConfiguration : ICORSConfiguration
    {
        public CORSConfiguration()
        {
            AllowedOrigins = new string[] { "*" };
        }

        public IEnumerable<string> AllowedOrigins { get; set; }
    }
}