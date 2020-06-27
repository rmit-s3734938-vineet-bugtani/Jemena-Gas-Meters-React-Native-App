using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JemenaGasMeter.WebApi.Models
{
    public class MeterInstallationRequest
    {
        public string MIRN { get; set; }
        public string PayRollID { get; set; }
        public MeterStatus MeterStatus { get; set; }
        public string StreetName { get; set; }
        public string Suburb { get; set; }
        public int PostCode { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
