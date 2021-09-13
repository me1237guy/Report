using Dapper.Contrib.Extensions;
using System.ComponentModel;

namespace ReportApp.Model
{
    [Table("OrderTable")]
    public class Order
    {
        [DisplayName("流水號")]
        public string Number { get; set; }
    }
}
