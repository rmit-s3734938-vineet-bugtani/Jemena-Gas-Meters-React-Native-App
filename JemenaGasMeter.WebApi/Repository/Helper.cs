using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JemenaGasMeter.WebApi.Repository
{
    public class Helper
    {
        // Convert the meter condition to DbModel meter condition
        public DbModels.MeterType ConvertMeterType(Models.MeterType mType)
        {
            var meterType = DbModels.MeterType.Domestic;

            switch (mType)
            {
                case Models.MeterType.Domestic:
                    meterType = DbModels.MeterType.Domestic;
                    break;
                case Models.MeterType.Commercial:
                    meterType = DbModels.MeterType.Commercial;
                    break;
                default:
                    meterType = DbModels.MeterType.Domestic;
                    break;
            }
            return meterType;
        }

        // Convert the Db model meter type to view Model meter type
        public Models.MeterType ConverViewModeltMeterType(DbModels.MeterType mType)
        {
            var vMeterType = Models.MeterType.Domestic;

            switch (mType)
            {
                case DbModels.MeterType.Domestic:
                    vMeterType = Models.MeterType.Domestic;
                    break;
                case DbModels.MeterType.Commercial:
                    vMeterType = Models.MeterType.Commercial;
                    break;
                default:
                    vMeterType = Models.MeterType.Domestic;
                    break;
            }
            return vMeterType;
        }

        // convert the view model meter status to DbModel meter status
        public DbModels.MeterStatus ConvertMeterStatus(Models.MeterStatus mStatus)
        {
            var dbMeterStatus = DbModels.MeterStatus.Inhouse;

            switch (mStatus)
            {
                case Models.MeterStatus.Inhouse:
                    dbMeterStatus = DbModels.MeterStatus.Inhouse;
                    break;
                case Models.MeterStatus.Install:
                    dbMeterStatus = DbModels.MeterStatus.Install;
                    break;
                case Models.MeterStatus.Pickup:
                    dbMeterStatus = DbModels.MeterStatus.Pickup;
                    break;
                case Models.MeterStatus.Return:
                    dbMeterStatus = DbModels.MeterStatus.Return;
                    break;
                case Models.MeterStatus.Transfer:
                    dbMeterStatus = DbModels.MeterStatus.Transfer;
                    break;
                default:
                    dbMeterStatus = DbModels.MeterStatus.Inhouse;
                    break;
            }
            return dbMeterStatus;
        }

        // convert the DB model meter status to View Model meter status
        public Models.MeterStatus ConvertViewModelMeterStatus(DbModels.MeterStatus mStatus)
        {
            var vMeterStatus = Models.MeterStatus.Inhouse;

            switch (mStatus)
            {
                case DbModels.MeterStatus.Inhouse:
                    vMeterStatus = Models.MeterStatus.Inhouse;
                    break;
                case DbModels.MeterStatus.Install:
                    vMeterStatus = Models.MeterStatus.Install; ;
                    break;
                case DbModels.MeterStatus.Pickup:
                    vMeterStatus = Models.MeterStatus.Pickup;
                    break;
                case DbModels.MeterStatus.Return:
                    vMeterStatus = Models.MeterStatus.Return;
                    break;
                case DbModels.MeterStatus.Transfer:
                    vMeterStatus = Models.MeterStatus.Transfer;
                    break;
                default:
                    vMeterStatus = Models.MeterStatus.Inhouse;
                    break;
            }
            return vMeterStatus;
        }


        // Convert the meter condition to DbModel meter condition
        public DbModels.MeterCondition ConvertMeterCondition(Models.MeterCondition mCondition)
        {
            var dbMeterCondition = DbModels.MeterCondition.Active;

            switch (mCondition)
            {
                case Models.MeterCondition.Active:
                    dbMeterCondition = DbModels.MeterCondition.Active;
                    break;
                case Models.MeterCondition.Faulty:
                    dbMeterCondition = DbModels.MeterCondition.Faulty;
                    break;
                case Models.MeterCondition.Expired:
                    dbMeterCondition = DbModels.MeterCondition.Expired;
                    break;
                default:
                    dbMeterCondition = DbModels.MeterCondition.Faulty;
                    break;
            }
            return dbMeterCondition;
        }

        // Convert the Db model meter condition to view Model meter condition
        public Models.MeterCondition ConvertViewModelMeterCondition(DbModels.MeterCondition mCondition)
        {
            var vMeterCondition = Models.MeterCondition.Active;

            switch (mCondition)
            {
                case DbModels.MeterCondition.Active:
                    vMeterCondition = Models.MeterCondition.Active;
                    break;
                case DbModels.MeterCondition.Faulty:
                    vMeterCondition = Models.MeterCondition.Faulty;
                    break;
                case DbModels.MeterCondition.Expired:
                    vMeterCondition = Models.MeterCondition.Expired;
                    break;
                default:
                    vMeterCondition = Models.MeterCondition.Faulty;
                    break;
            }
            return vMeterCondition;
        }

        // converting dbModel meter into view model meter
        public Models.Meter ConvertMeterModel(DbModels.Meter dbMeter)
        {
            var vmMeter = new Models.Meter()
            {
                MIRN = dbMeter.MIRN,
                MeterType = ConverViewModeltMeterType(dbMeter.MeterType),
                MeterStatus = ConvertViewModelMeterStatus(dbMeter.MeterStatus),
                MeterCondition = ConvertViewModelMeterCondition(dbMeter.MeterCondition)
            };
            return vmMeter;
        }

        // converting dbModel meterHistory into view model meter History
        public Models.MeterHistory ConvertMeterHistoryModel(DbModels.MeterHistory dbMeterHistory)
        {
            var vmhMeter = new Models.MeterHistory()
            {
                MeterHistoryID = Convert.ToString(dbMeterHistory.MeterHistoryID),
                MIRN = dbMeterHistory.MIRN,
                PayRollID = dbMeterHistory.PayRollID,
                MeterStatus = ConvertViewModelMeterStatus(dbMeterHistory.MeterStatus),
                Location = dbMeterHistory.Location,
                TransfereeID = dbMeterHistory.TransfereeID,
                TransactionDate = dbMeterHistory.TransactionDate,
                Comment = dbMeterHistory.Comment
            };
            return vmhMeter;
        }

        // Convert the user Status to DbModel User Status
        public DbModels.UserStatus ConvertToDbModelUserStatus(Models.UserStatus uStatus)
        {
            var userStatus = DbModels.UserStatus.Inactive;

            switch (uStatus)
            {
                case Models.UserStatus.Active:
                    userStatus = DbModels.UserStatus.Active;
                    break;
                case Models.UserStatus.Inactive:
                    userStatus = DbModels.UserStatus.Inactive;
                    break;
                default:
                    userStatus = DbModels.UserStatus.Inactive;
                    break;
            }
            return userStatus;
        }

        // Convert the Db model User Status to view Model User Status
        public Models.UserStatus ConverToViewModelUserStatus(DbModels.UserStatus uStatus)
        {
            var vUserStatus = Models.UserStatus.Inactive;

            switch (uStatus)
            {
                case DbModels.UserStatus.Active:
                    vUserStatus = Models.UserStatus.Active;
                    break;
                case DbModels.UserStatus.Inactive:
                    vUserStatus = Models.UserStatus.Inactive;
                    break;
                default:
                    vUserStatus = Models.UserStatus.Inactive;
                    break;
            }
            return vUserStatus;
        }

        // Convert the User type to DbModel User type
        public DbModels.UserType ConvertToDbModelUserType(Models.UserType uType)
        {
            var userType = DbModels.UserType.Technician;

            switch (uType)
            {
                case Models.UserType.Technician:
                    userType = DbModels.UserType.Technician;
                    break;
                case Models.UserType.Contractor:
                    userType = DbModels.UserType.Contractor;
                    break;
                default:
                    userType = DbModels.UserType.Technician;
                    break;
            }
            return userType;
        }

        // Convert the Db model user type to view Model user type
        public Models.UserType ConverToViewModeltUserType(DbModels.UserType uType)
        {
            var vUserType = Models.UserType.Technician;

            switch (uType)
            {
                case DbModels.UserType.Technician:
                    vUserType = Models.UserType.Technician;
                    break;
                case DbModels.UserType.Contractor:
                    vUserType = Models.UserType.Contractor;
                    break;
                default:
                    vUserType = Models.UserType.Technician;
                    break;
            }
            return vUserType;
        }

        // Convert the View model Status to DB Model Status
        public DbModels.Status ConvertToDbModelStatus(Models.Status status)
        {
            var dbStatus = DbModels.Status.Inactive;

            switch (status)
            {
                case Models.Status.Active:
                    dbStatus = DbModels.Status.Active;
                    break;
                case Models.Status.Inactive:
                    dbStatus = DbModels.Status.Inactive;
                    break;
                default:
                    dbStatus = DbModels.Status.Inactive;
                    break;
            }
            return dbStatus;
        }

        // Convert the Db model Status to view Model Status
        public Models.Status ConverToViewModelStatus(DbModels.Status status)
        {
            var vStatus = Models.Status.Inactive;

            switch (status)
            {
                case DbModels.Status.Active:
                    vStatus = Models.Status.Active;
                    break;
                case DbModels.Status.Inactive:
                    vStatus = Models.Status.Inactive;
                    break;
                default:
                    vStatus = Models.Status.Inactive;
                    break;
            }
            return vStatus;
        }
    }
}
