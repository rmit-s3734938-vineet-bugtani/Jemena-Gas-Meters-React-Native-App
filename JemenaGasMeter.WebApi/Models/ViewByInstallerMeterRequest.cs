using System;

namespace JemenaGasMeter.WebApi.Models
{
    public class ViewByInstallerMeterRequest
    {
        public string PayRollID { get; set; }
        public MeterStatus MeterStatus { get; set; }
    }
}
