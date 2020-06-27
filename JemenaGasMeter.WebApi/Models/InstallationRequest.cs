using System;

namespace JemenaGasMeter.WebApi.Models
{
    public class InstallationRequest
    {
        public string MIRN { get; set; }
        public string PayRollID { get; set; }
        public MeterStatus MeterStatus {get; set; }
        public string StreetNo { get; set; }
        public string StreetName { get; set; }
        public string Suburb { get; set; }
        public string State { get; set; }
        public int PostCode { get; set; }
        public string Comment { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
