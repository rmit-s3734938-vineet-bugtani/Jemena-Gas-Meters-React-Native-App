using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace JemenaGasMeter.WebApi.DbModels
{

    public class Transfer
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransferID { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
    }
}
