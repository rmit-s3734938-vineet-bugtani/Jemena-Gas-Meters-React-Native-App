using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JemenaGasMeter.WebApi.Models
{
    public class Meter
    {
        public string MIRN { get; set; }
        public MeterType MeterType { get; set; }
        public MeterStatus MeterStatus { get; set; }
        public MeterCondition MeterCondition { get; set; }
        public DateTime ExpriyDate { get; set; }
    }
}
