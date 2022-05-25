using System;
using System.Configuration;

namespace Utilities
{
    public static class Helper
    {
        public static string ConnectionString
        {
            get
            {
                return ConfigurationManager.AppSettings["DbConnectionString"];
            }
        }
    }
}
