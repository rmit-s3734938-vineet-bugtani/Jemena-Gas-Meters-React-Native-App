using JemenaGasMeter.WebApi.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JemenaGasMeter.WebApi.Models
{
    public class ViewMeterBulkRequest
    {
        public string[] MIRN { get; set; }
    }
}
