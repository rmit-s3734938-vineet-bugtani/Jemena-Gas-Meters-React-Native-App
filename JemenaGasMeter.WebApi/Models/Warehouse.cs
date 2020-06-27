using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JemenaGasMeter.WebApi.Models
{
    public class Warehouse
    {
        public string WarehouseID { get; set; }
        public string StreetName { get; set; }
        public string Suburb { get; set; }
        public int PostCode { get; set; }
        public Status Status { get; set; }
    }
}
