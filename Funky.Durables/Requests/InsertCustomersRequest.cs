using System.Collections.Generic;
using Funky.Durables.Models;

namespace Funky.Durables.Requests
{
    public class InsertCustomersRequest
    {
        public List<CustomerFileRecord> Records { get; set; }
    }
}