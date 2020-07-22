using System.Collections.Generic;

namespace Funky.Durables.Models
{
    public class CustomerFileInformation
    {
        public List<CustomerFileRecord> ValidRecords { get; set; }
        public List<CustomerFileRecord> InvalidRecords { get; set; }
        public List<CustomerFileRecord> InvalidRowRecords { get; set; }
    }
}