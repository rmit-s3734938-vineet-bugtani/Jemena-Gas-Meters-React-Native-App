using System;

namespace JemenaGasMeter.WebApi.Models
{
    public class ChangeMeterTypeRequest
    {
        public string[] MIRN { get; set; }
        public MeterType MeterType { get; set; }
    }
}
