using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace JemenaGasMeter.WebApi.Models
{

    public class Transfer
    {
        public int TransferID { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
    }
}
