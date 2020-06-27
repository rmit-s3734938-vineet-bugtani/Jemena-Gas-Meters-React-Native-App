using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JemenaGasMeter.WebApi.DbModels
{
    public enum MeterStatus
    {
        Inhouse = 1,
        Pickup = 2,
        Return = 3,
        Transfer = 4,
        Install = 5
    }
}
