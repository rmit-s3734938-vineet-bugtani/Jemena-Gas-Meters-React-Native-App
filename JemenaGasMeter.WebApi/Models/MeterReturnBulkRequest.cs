using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JemenaGasMeter.WebApi.Models
{
    public class MeterReturnBulkRequest
    {
        public string[] MIRN { get; set; }
        public string PayRollID { get; set; }
        public MeterStatus MeterStatus { get; set; }
        public MeterCondition[] MeterCondition { get; set; }
        public string Location { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
