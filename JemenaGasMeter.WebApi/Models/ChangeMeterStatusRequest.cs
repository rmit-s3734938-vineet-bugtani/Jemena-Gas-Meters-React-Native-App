using System;

namespace JemenaGasMeter.WebApi.Models
{
    public class ChangeMeterStatusRequest
    {
        public string[] MIRN { get; set; }
        public MeterStatus MeterStatus { get; set; }
        public MeterCondition MeterCondition { get; set; }
    }
}
