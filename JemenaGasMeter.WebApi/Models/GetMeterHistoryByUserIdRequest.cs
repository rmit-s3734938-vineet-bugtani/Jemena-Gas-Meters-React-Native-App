using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JemenaGasMeter.WebApi.Models
{
    public class GetMeterHistoryByUserIdRequest
    {
       
        public string PayRollID { get; set; }
        public MeterStatus MeterStatus { get; set; }
    }
}
