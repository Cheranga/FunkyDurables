using System;
using System.Collections.Generic;
using System.Text;
using Hatan.Azure.Functions.DependencyInjection.Extensions;

namespace Funky.Durables.DataAccess
{
    public class DatabaseConfiguration : ICustomApplicationSetting
    {
        public string ConnectionString { get; set; }
        public string TableName { get; set; }
    }
}
