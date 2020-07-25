using CsvHelper.Configuration.Attributes;

namespace Funky.Durables.Models
{
    public class CustomerFileRecord
    {
        [Name("Name")]
        public string Name { get; set; }
        
        [Name("Address")]
        public string Address { get; set; }
    }
}