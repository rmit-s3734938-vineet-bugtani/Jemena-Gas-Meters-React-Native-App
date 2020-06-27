using System;

namespace JemenaGasMeter.WebApi.Models
{
    public class MeterHistory
    {
        public string MeterHistoryID { get; set; }
        public string MIRN { get; set; }
        public string PayRollID { get; set; }
        public MeterStatus MeterStatus { get; set; }
        public string Location { get; set; }
        public string TransfereeID { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Comment { get; set; }
    }
}
