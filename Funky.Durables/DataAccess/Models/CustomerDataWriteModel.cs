using Microsoft.Azure.Cosmos.Table;

namespace Funky.Durables.DataAccess.Models
{
    public class CustomerDataWriteModel : TableEntity
    {
        public string Name { get; set; }
        public string Address { get; set; }
    }
}
