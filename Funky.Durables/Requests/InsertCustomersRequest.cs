using System.Collections.Generic;
using Funky.Durables.Models;
using Funky.Durables.Patterns.FunctionChaining;

namespace Funky.Durables.Requests
{
    public class InsertCustomersRequest
    {
        public List<CustomerFileRecord> Records { get; set; }
    }
}