using System;

namespace JemenaGasMeter.WebApi.Models
{
    public class TransferRequest
    {
        public string MIRN { get; set; }
        public string PayRollID { get; set; }
        public MeterStatus MeterStatus {get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public string Comment { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
