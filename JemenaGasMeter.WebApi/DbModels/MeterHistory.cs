using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace JemenaGasMeter.WebApi.DbModels
{

    public class MeterHistory
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MeterHistoryID { get; set; }
        public string MIRN { get; set; }
        public string PayRollID { get; set; }
        public MeterStatus MeterStatus { get; set; }
        public string Location { get; set; }
        public string TransfereeID { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Comment { get; set; }
        public Meter Meter { get; set; }
        public User User { get; set; }
    }
}
