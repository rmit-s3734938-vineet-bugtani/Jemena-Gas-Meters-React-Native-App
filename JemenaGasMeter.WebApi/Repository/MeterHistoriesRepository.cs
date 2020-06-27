using JemenaGasMeter.WebApi.Data;
using JemenaGasMeter.WebApi.DbModels;
using JemenaGasMeter.WebApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace JemenaGasMeter.WebApi.Repository
{
    public class MeterHistoriesRepository : IDataRepository<DbModels.MeterHistory, int>
    {
        private readonly AppDbContext _context;
        private readonly MetersRepository _mrepo;
        private readonly UsersRepository _urepo;
        private readonly WarehousesRepository _wrepo;
        private readonly DepotsRepository _drepo;
        private readonly InstallationsRepository _irepo;
        private readonly TransfersRepository _trepo;
        private readonly Helper helper = new Helper();
        private readonly Validator validater = new Validator();

        public MeterHistoriesRepository(AppDbContext context, MetersRepository mrepo, UsersRepository urepo,
            WarehousesRepository wrepo, DepotsRepository drepo, InstallationsRepository irepo, TransfersRepository trepo)
        {
            _context = context;
            _mrepo = mrepo;
            _urepo = urepo;
            _wrepo = wrepo;
            _drepo = drepo;
            _irepo = irepo;
            _trepo = trepo;
        }

        // Get all the meterHistory
        public IEnumerable<DbModels.MeterHistory> GetAll()
        {
            var meterHistories = _context.MeterHistories.ToList();
            if (meterHistories != null)
            {
                return meterHistories;
            }
            else
            {
                return null;
            }
        }

        // Get single meterHistory by MeterHistoryID 
        public DbModels.MeterHistory Get(int id)
        {
            var meterHistory = _context.MeterHistories.Find(id);
            if (meterHistory != null)
            {
                return meterHistory;
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<DbModels.MeterHistory> GetHistoryByMIRN(string mirn)
        {
            var meterHistories = _context.MeterHistories.Where(x => x.MIRN.Equals(mirn)).ToList();
            if (meterHistories != null)
            {
                return meterHistories;
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<DbModels.MeterHistory> GetHistoryByLocationID(string Id)
        {
            var meterHistories = _context.MeterHistories.Where(x => x.Location.Equals(Id)).ToList();
            if (meterHistories != null)
            {
                return meterHistories;
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<Models.MeterHistoryMeters> GetMeterHistoryByUserId(Models.MeterHistory mpsRequest)
        {
            // var dbmStatus = helper.ConvertMeterStatus(mpsRequest.MeterStatus);
            var meterStatus = 0;
            switch (mpsRequest.MeterStatus)
            {
                case Models.MeterStatus.Inhouse:
                    meterStatus = 1;
                    break;
                case Models.MeterStatus.Install:
                    meterStatus = 5;
                    break;
                case Models.MeterStatus.Pickup:
                    meterStatus = 2;
                    break;
                case Models.MeterStatus.Return:
                    meterStatus = 3;
                    break;
                case Models.MeterStatus.Transfer:
                    meterStatus = 4;
                    break;
                default:
                    meterStatus = 0;
                    break;
            }

            var sql = "select h.*,m.MeterType, m.MeterCondition, m.ExpriyDate from MeterHistories h "
                    + "JOIN Meters m ON m.MIRN = h.MIRN "
                    + "where PayRollID = '" + mpsRequest.PayRollID + "' "
                    + "AND MeterHistoryID = (select MAX(mh.MeterHistoryID) from MeterHistories mh where mh.MIRN = h.MIRN) "
                    + "AND h.MeterStatus = "+ meterStatus + ";";


            var meterHistoryResult = _context.MeterHistories.FromSqlRaw(sql).ToList();
           
            List<Models.MeterHistoryMeters> mhList = new List<Models.MeterHistoryMeters>();
            foreach(DbModels.MeterHistory mh in meterHistoryResult)
            {
                Models.MeterHistoryMeters meterHistoyMeter = new MeterHistoryMeters();
                var meter = _mrepo.Get(mh.MIRN);
                meterHistoyMeter.MeterHistory = helper.ConvertMeterHistoryModel(mh);
                meterHistoyMeter.Meter = helper.ConvertMeterModel(meter);
                mhList.Add(meterHistoyMeter);
            }

            if (mhList != null)
            {
                return mhList;
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<Models.MeterHistory> GetMeterHistoryByMeterId(Models.MeterHistory mpsRequest)
        {
            
            var sql = "SELECT * FROM MeterHistories mh "
                + "WHERE MIRN = '" + mpsRequest.MIRN + "' "
                + "ORDER BY TransactionDate DESC;";

            var meterHistoryResult = _context.MeterHistories.FromSqlRaw(sql).ToList();

            List<Models.MeterHistory> mhList = new List<Models.MeterHistory>();
            foreach (DbModels.MeterHistory mh in meterHistoryResult)
            {
      
                var meterHistoy = helper.ConvertMeterHistoryModel(mh);
                mhList.Add(meterHistoy);
            }

            if (mhList != null)
            {
                return mhList;
            }
            else
            {
                return null;
            }
        }


        // Add new meterHistory and return new meterHistory ID
        public int Add(DbModels.MeterHistory meterHistory)
        {
            _context.MeterHistories.Add(meterHistory);
            _context.SaveChanges();

            return meterHistory.MeterHistoryID;
        }

        // Edit meterHistory by MeterHistoryID
        public int Update(int id, DbModels.MeterHistory meterHistory)
        {
            var mt = _context.MeterHistories.FirstOrDefault(x => x.MeterHistoryID == id);
            if (mt != null)
            {
                mt.MIRN = meterHistory.MIRN;
                mt.PayRollID = meterHistory.PayRollID;
                mt.MeterStatus = meterHistory.MeterStatus;
                mt.Location = meterHistory.Location;
                mt.TransfereeID = meterHistory.TransfereeID;
                mt.TransactionDate = meterHistory.TransactionDate;

                _context.SaveChanges();
                return id;
            }
            else
            {
                return -1;
            }
        }

         // Remove All MeterTransactions
        public void Delete()
        {   
            _context.MeterHistories.RemoveRange(_context.MeterHistories);
            _context.SaveChanges();

        }

        // Remove MeterTransaction by MeterHistoryID
        public int Delete(int id)
        {
            _context.MeterHistories.Remove(_context.MeterHistories.Find(id));
            _context.SaveChanges();

            return id;
        }

        // Create pickup history method which return's either error message or success message
        public Tuple<bool, string> CreatePickupHistory(MeterPickupRequest mpHistory)
        {
            var result = Tuple.Create(true, "");

            // validating Meter ID
            var meter = _mrepo.Get(mpHistory.MIRN);
            if (meter.MIRN == null)
            {
                return Tuple.Create(false, "Meter ID " + mpHistory.MIRN + " is not found");
            }

            // validating PayRoll ID
            var user = _urepo.Get(mpHistory.PayRollID);
            if (user.PayRollID == null)
            {
                return Tuple.Create(false, "PayRollID " + mpHistory.PayRollID + " is not found");
            }

            // validating meter status and meter condition
            // Pickup = 2, then actual meter status must be in Inhouse
            if (!(meter.MeterStatus.Equals(DbModels.MeterStatus.Inhouse)))
            {
                return Tuple.Create(false, "Meter ID " + mpHistory.MIRN + " is not available for pickup, current status is " + meter.MeterStatus);
            }

            if (!(meter.MeterCondition.Equals(DbModels.MeterCondition.Active)))
            {
                return Tuple.Create(false, "Meter ID " + mpHistory.MIRN + " is not available for pickup, current condition is " + meter.MeterCondition);
            }

            // validating location
            var warehouse = _wrepo.Get(mpHistory.Location);
            if (warehouse == null)
            {
                var depot = _drepo.Get(mpHistory.Location);
                if (depot == null)
                {
                    return Tuple.Create(false, "Location  " + mpHistory.Location + " is not found");
                }
            }

            // validating not future date
            if (mpHistory.TransactionDate > DateTime.UtcNow)
            {
                return Tuple.Create(false, "Date should not be in the future date");
            }

            //All validation are passed now converting the viewmodel to dbmodel 
            var mhDbModel = new DbModels.MeterHistory();
            mhDbModel.MIRN = mpHistory.MIRN;
            mhDbModel.PayRollID = mpHistory.PayRollID;
            mhDbModel.MeterStatus = DbModels.MeterStatus.Pickup;
            mhDbModel.Location = mpHistory.Location;
            mhDbModel.TransactionDate = mpHistory.TransactionDate;

            // creating meter history record in the DB
            var mhID = Add(mhDbModel);

            if (mhID > 0)
            {
                return Tuple.Create(true, mhID.ToString());
            }

            return Tuple.Create(false, "Unable to Pickup Meter!");
        }

        // Create return history method which return's either error message or success message
        public Tuple<bool, string> CreateReturnHistory(MeterReturnRequest mrHistory)
        {
            var result = Tuple.Create(true, "");

            // validating Meter ID
            var meter = _mrepo.Get(mrHistory.MIRN);
            if (meter.MIRN == null)
            {
                return Tuple.Create(false, "Meter ID " + mrHistory.MIRN + " is not found");
            }

            // validating PayRoll ID
            var user = _urepo.Get(mrHistory.PayRollID);
            if (user.PayRollID == null)
            {
                return Tuple.Create(false, "PayRollID " + mrHistory.PayRollID + " is not found");
            }

            // validating meter status
            // Return = 5, then actual meter status must be in Pickup
            if (!(meter.MeterStatus.Equals(DbModels.MeterStatus.Pickup)))
            {
                return Tuple.Create(false, "Meter ID " + mrHistory.MIRN + " is not available for return, current status is " + meter.MeterStatus);
            }

            // validating location
            var warehouse = _wrepo.Get(mrHistory.Location);
            if (warehouse == null)
            {
                var depot = _drepo.Get(mrHistory.Location);
                if (depot == null)
                {
                    return Tuple.Create(false, "Location  " + mrHistory.Location + " is not found");
                }
            }

            // validating not future date
            if (mrHistory.TransactionDate > DateTime.UtcNow)
            {
                return Tuple.Create(false, "Date should not be in the future date");
            }

            //All validation are passed now converting the viewmodel to dbmodel 
            var mhDbModel = new DbModels.MeterHistory();
            mhDbModel.MIRN = mrHistory.MIRN;
            mhDbModel.PayRollID = mrHistory.PayRollID;
            mhDbModel.MeterStatus = DbModels.MeterStatus.Return;
            mhDbModel.Location = mrHistory.Location;
            mhDbModel.TransactionDate = mrHistory.TransactionDate;

            // creating meter history record in the DB
            var mhID = Add(mhDbModel);

            if (mhID > 0)
            {
                return Tuple.Create(true, mhID.ToString());
            }
            return Tuple.Create(false, "Unable to Return Meter!");
        }

        // Create return history method which return's either error message or success message
        public Tuple<bool, string> CreateInstallHistory(InstallationRequest iHistory)
        {
            var result = Tuple.Create(true, "");

            // validating Meter ID
            var meter = _mrepo.Get(iHistory.MIRN);
            if (meter.MIRN == null)
            {
                return Tuple.Create(false, "Meter ID " + iHistory.MIRN + " is not found");
            }

            // validating PayRoll ID
            var user = _urepo.Get(iHistory.PayRollID);
            if (user.PayRollID == null)
            {
                return Tuple.Create(false, "PayRollID " + iHistory.PayRollID + " is not found");
            }

            // validating meter status and meter condition
            // Return = 5, then actual meter status must be in Pickup
            if (!(meter.MeterStatus.Equals(DbModels.MeterStatus.Pickup)))
            {
                return Tuple.Create(false, "Meter ID " + iHistory.MIRN + " is not available for install, current status is " + meter.MeterStatus);
            }

            if (!(meter.MeterCondition.Equals(DbModels.MeterCondition.Active)))
            {
                return Tuple.Create(false, "Meter ID " + iHistory.MIRN + " is not available for install, current condition is " + meter.MeterCondition);
            }


            // validating not future date
            if (iHistory.TransactionDate > DateTime.UtcNow)
            {
                return Tuple.Create(false, "Date should not be in the future date");
            }

            // All validation are passed now converting the viewmodel to installation dbmodel 
            var iDbModel = new DbModels.Installation();
            iDbModel.MIRN = iHistory.MIRN;
            iDbModel.StreetNo = iHistory.StreetNo;
            iDbModel.StreetName = iHistory.StreetName;
            iDbModel.Suburb = iHistory.Suburb;
            iDbModel.State = iHistory.State;
            iDbModel.PostCode = iHistory.PostCode;
            iDbModel.Status = DbModels.Status.Active;

            var iResult = _irepo.Add(iDbModel);
            if(iResult > 0)
            {
                //All validation are passed now converting the viewmodel to dbmodel 
                var mhDbModel = new DbModels.MeterHistory();
                mhDbModel.MIRN = iHistory.MIRN;
                mhDbModel.PayRollID = iHistory.PayRollID;
                mhDbModel.MeterStatus = DbModels.MeterStatus.Install;
                mhDbModel.Location = Convert.ToString(iResult);
                mhDbModel.TransactionDate = iHistory.TransactionDate;
                mhDbModel.Comment = iHistory.Comment;

                // creating meter history record in the DB
                var mhID = Add(mhDbModel);

                if (mhID > 0)
                {
                    return Tuple.Create(true, mhID.ToString());
                }
            }
            else
            {
                return Tuple.Create(false, "Unable to create new Installation record!");
            }
            return Tuple.Create(false, "Unable to Install Meter!");
        }

        // Create Transfer history method which return's either error message or success message
        public Tuple<bool, string> CreateTransferHistory(string transferID, TransferRequest tHistory)
        {
            var result = Tuple.Create(true, "");

            // validating Meter ID
            var meter = _mrepo.Get(tHistory.MIRN);
            if (meter.MIRN == null)
            {
                return Tuple.Create(false, "Meter ID " + tHistory.MIRN + " is not found");
            }

            // validating PayRoll ID
            var user = _urepo.Get(tHistory.PayRollID);
            if (user.PayRollID == null)
            {
                return Tuple.Create(false, "PayRollID " + tHistory.PayRollID + " is not found");
            }

            // validating meter status and meter condition
            // Transfer = 4, then actual meter status must be in Pickup
            if (!(meter.MeterStatus.Equals(DbModels.MeterStatus.Pickup)))
            {
                return Tuple.Create(false, "Meter ID " + tHistory.MIRN + " is not available for transfer, current status is " + meter.MeterStatus);
            }

            if (!(meter.MeterCondition.Equals(DbModels.MeterCondition.Active)))
            {
                return Tuple.Create(false, "Meter ID " + tHistory.MIRN + " is not available for transfer, current condition is " + meter.MeterCondition);
            }

            // validating not future date
            if (tHistory.TransactionDate > DateTime.UtcNow)
            {
                return Tuple.Create(false, "Date should not be in the future date");
            }

            //All validation are passed now converting the viewmodel to dbmodel 
            var mhDbModel = new DbModels.MeterHistory();
            mhDbModel.MIRN = tHistory.MIRN;
            mhDbModel.PayRollID = tHistory.PayRollID;
            mhDbModel.MeterStatus = DbModels.MeterStatus.Transfer;
            mhDbModel.TransfereeID = transferID;
            mhDbModel.TransactionDate = tHistory.TransactionDate;
            mhDbModel.Comment = tHistory.Comment;

            // creating meter history record in the DB
            var mhID = Add(mhDbModel);

            if (mhID > 0)
            {
                return Tuple.Create(true, mhID.ToString());
            }
           
            return Tuple.Create(false, "Unable to Transfer Meter!");
        }

        //Get Meter details by passing MeterID, PayRollId and MeterStatus
        public Tuple<bool, string, ArrayList> GetMeterByUserAndStatus(Models.MeterHistory mpsRequest)
        {
            // Store meter in array
            ArrayList metersList = new ArrayList();
            var result = Tuple.Create(true, "", metersList);
            DbModels.MeterStatus dbmStatus;

            var validMeterStatus = validater.ValidateMeterStatus(mpsRequest.MeterStatus);

            if(validMeterStatus == true)
            {
                dbmStatus = helper.ConvertMeterStatus(mpsRequest.MeterStatus);
                // validating PayRoll ID
                var user = _urepo.Get(mpsRequest.PayRollID);
                if (user == null)
                {
                    return Tuple.Create(false, "PayRollID " + mpsRequest.PayRollID + " is not found", metersList);
                }
                // Find meter ID using payroll id and meter status from meter history table
                var meters = _context.MeterHistories.Where(x => x.PayRollID.Contains(mpsRequest.PayRollID) && (x.MeterStatus == dbmStatus)).ToList();

                foreach (DbModels.MeterHistory meter in meters)
                {
                    // Find the meter record from meter table using meterid
                    //  var m = _context.Meters.Where(x => x.MIRN.Equals(meter.MIRN));
                    // _context.Installations.FirstOrDefault(x => x.InstallationID == id);

                    DbModels.Meter meterDb = _context.Meters.FirstOrDefault(x => x.MIRN == meter.MIRN && x.MeterStatus == dbmStatus);

                    if (meterDb != null)
                    {
                        metersList.Add(meterDb);
                    }
                }
                return Tuple.Create(true, "Meters List", metersList);
            }
            else
            {
                return Tuple.Create(false, "Invalid meter status: " + mpsRequest.MeterStatus, metersList);
            }
        }

        //Get Meter by passind transaction dates
        public Tuple<bool, string, ArrayList> GetMeterByDate(Models.ViewByDateRequest mdRequest) {

            // Store meter in array
            ArrayList transactionList = new ArrayList();
            var result = Tuple.Create(true, "");
            DbModels.MeterStatus dbmStatus;

            var startDate = mdRequest.StartDate;
            var endDate = mdRequest.EndDate;
            var validMeterStatus = validater.ValidateMeterStatus(mdRequest.MeterStatus);

            if (validMeterStatus == true)
            {
                dbmStatus = helper.ConvertMeterStatus(mdRequest.MeterStatus);

                // validating not future date
                if (startDate > DateTime.UtcNow)
                {
                    return Tuple.Create(false, "Starting Date should not be in the future date", transactionList);
                }
                else if(endDate > DateTime.UtcNow)
                {
                    return Tuple.Create(false, "End Date should not be in the future date", transactionList);
                }
                else if(startDate > endDate)
                {
                    return Tuple.Create(false, "Start Date should be before end date", transactionList);
                }

                // Find meter transaction history between dates from meter history table
                var transaction = _context.MeterHistories.Where(x => (x.TransactionDate >= startDate) && (x.TransactionDate <= endDate) && (x.MeterStatus == dbmStatus)).ToList();

                if(transaction != null)
                {
                    transactionList.Add(transaction);
                }
                return Tuple.Create(true, "Meters List", transactionList);
            }
            return Tuple.Create(false, "Invalid meter status: " + mdRequest.MeterStatus, transactionList);
        }

    }
}
