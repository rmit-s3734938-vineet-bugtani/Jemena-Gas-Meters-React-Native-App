using System;

namespace JemenaGasMeter.WebApi.Models
{
    public class ViewByDateRequest
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public MeterStatus MeterStatus { get; set; }
    }
}
