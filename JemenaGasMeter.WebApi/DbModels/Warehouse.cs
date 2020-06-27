using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace JemenaGasMeter.WebApi.DbModels
{  
    public class Warehouse
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string WarehouseID { get; set; }
        public string StreetName { get; set; }
        public string Suburb { get; set; }
        public int PostCode { get; set; }
        public Status Status { get; set; }
    }
}
