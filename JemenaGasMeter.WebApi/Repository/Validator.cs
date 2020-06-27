using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JemenaGasMeter.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace JemenaGasMeter.WebApi.Repository
{
    public class Validator
    {
        // Validating Meter Type
        public bool ValidateMeterType(Models.MeterType mType)
        {
            if (mType.Equals(Models.MeterType.Domestic))
            {
                return true;
            }
            else if (mType.Equals(Models.MeterType.Commercial))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // Validating Meter Status
        public bool ValidateMeterStatus(Models.MeterStatus mStatus)
        {
            if (mStatus.Equals(Models.MeterStatus.Inhouse))
            {
                return true;
            }
            else if (mStatus.Equals(Models.MeterStatus.Install))
            {
                return true;
            }
            else if (mStatus.Equals(Models.MeterStatus.Pickup))
            {
                return true;
            }
            else if (mStatus.Equals(Models.MeterStatus.Return))
            {
                return true;
            }
            else if (mStatus.Equals(Models.MeterStatus.Transfer))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // Validating Meter Condition
        public bool ValidateMeterCondition(Models.MeterCondition mCondition)
        {
            if (mCondition.Equals(Models.MeterCondition.Active))
            {
                return true;
            }
            else if (mCondition.Equals(Models.MeterCondition.Faulty))
            {
                return true;
            }
            else if (mCondition.Equals(Models.MeterCondition.Expired))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
