using System.Collections.Generic;
using Funky.Durables.Models;

namespace Funky.Durables.DataAccess.Commands
{
    public class InsertCustomerDataCommand
    {
        public string Category { get; set; }
        public List<CustomerFileRecord> Records { get; set; }
    }
}
