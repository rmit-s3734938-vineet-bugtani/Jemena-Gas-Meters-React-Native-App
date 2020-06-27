using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JemenaGasMeter.WebApi.DbModels
{
    public class Installation
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InstallationID { get; set; }
        public string MIRN { get; set; }
        public string StreetNo { get; set; }
        public string StreetName { get; set; }
        public string Suburb { get; set; }
        public string State { get; set; }
        public int PostCode { get; set; }
        public Status Status { get; set; }
    }
}
