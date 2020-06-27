using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using JemenaGasMeter.WebApi.Repository;
using AutoMapper;
using JemenaGasMeter.WebApi.Models;
using System;
using System.Collections;

namespace JemenaGasMeter.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeterHistoriesController : Controller
    {
        private readonly MeterHistoriesRepository _repo;
        private readonly MetersRepository _mrepo;
        private readonly UsersRepository _urepo;
        private readonly WarehousesRepository _wrepo;
        private readonly DepotsRepository _drepo;
        private readonly InstallationsRepository _irepo;
        private readonly TransfersRepository _trepo;
        private readonly IMapper _mapper;
        private Helper helper = new Helper();


        public MeterHistoriesController(MeterHistoriesRepository repo,
            MetersRepository mrepo, UsersRepository urepo,
            WarehousesRepository wrepo, DepotsRepository drepo,
            InstallationsRepository irepo, TransfersRepository trepo, IMapper mapper)
        {
            _repo = repo;
            _mrepo = mrepo;
            _urepo = urepo;
            _wrepo = wrepo;
            _drepo = drepo;
            _irepo = irepo;
            _trepo = trepo;
            _mapper = mapper;
        }

        // Returns all meter history
        // GET: api/meterHistory
        [HttpGet]
        [ValidateModel]
        public IActionResult GetAll()
        {
            var meterHistory = _repo.GetAll()?.ToList();
            if (meterHistory == null)
            {
                ModelState.AddModelError("Meter History", "Not Available!");
                return NotFound();
            }

            var result = new List<Models.MeterHistory>();
            var meterModel = _mapper.Map<List<DbModels.MeterHistory>, List<Models.MeterHistory>>(meterHistory, result);

            if (!result.Any())
            {
                ModelState.AddModelError("Meter History", "Not Available!");
                return NotFound();
            }
            return Ok(result);
        }

        // Returns particular meter history 
        // GET api/meterHistory/1
        [HttpGet("{id}")]
        [ValidateModel]
        public IActionResult Get(int id)
        {
            var meterHistory = _repo.Get(id);
            if (meterHistory == null)
            {
                ModelState.AddModelError("Meter History", "Not Available!");
                return NotFound();
            }
            return Ok(meterHistory);
        }

        [HttpGet("mirn/{mirn}")]
        [ValidateModel]
        public IActionResult GetByMIRN(string mirn)
        {
            var meterHistory = _repo.GetHistoryByMIRN(mirn);
            if (meterHistory == null)
            {
                ModelState.AddModelError("Meter History", "Not Available!");
                return NotFound();
            }
            return Ok(meterHistory);
        }

        // Delete all meter histories
        [HttpPost("delete")]
        [ValidateModel]
        public IActionResult DeleteAll()
        {
            _repo.Delete();
            return Ok();
        }

        // Pickup Meters Single or Bulk
        // POST api/meterhistory/pickup
        /// <remarks>
        /// Meter Status : Inhouse = 1,Pickup = 2, Return = 3, Transfer = 4, Install = 5
        /// </remarks>
        [HttpPost("pickup")]
        [ValidateModel]
        public IActionResult PickupMeters(MeterPickupBulkRequest mhRequest)
        {
            // getting MeterID[], PayRollID, MeterStatus, Location and TransactionDate

            // check is this request is for Pickup
            if (!(mhRequest.MeterStatus.Equals(MeterStatus.Pickup)))
            {
                ModelState.AddModelError("Error", "Invalid request type for pickup meters: " + mhRequest.MeterStatus);
                return NotFound();
            }

            // check Payroll ID 
            // validating PayRoll ID
            var user = _urepo.Get(mhRequest.PayRollID);
            if (user == null)
            {
                ModelState.AddModelError("Error", "PayRollID " + mhRequest.PayRollID + " is not found");
                return NotFound();
            }

            // validating location
            var warehouse = _wrepo.Get(mhRequest.Location);
            if (warehouse == null)
            {
                var depot = _drepo.Get(mhRequest.Location);
                if (depot == null)
                {
                    ModelState.AddModelError("Error", "Location  " + mhRequest.Location + " is not found");
                    return NotFound();
                }
            }

            // validating not future date
            if (mhRequest.TransactionDate > DateTime.UtcNow)
            {
                ModelState.AddModelError("Error", "Date should not be in the future date");
                return NotFound();
            }

            // send the success message with list of meters 
            ArrayList pickedMeterList = new ArrayList();

            // meter ID validation
            var metersList = mhRequest.MIRN;
            for (var i=0; i<metersList.Length; i++)
            {
                var meter = _mrepo.Get(metersList[i]);
                if(meter != null)
                {
                    var viewModel = new Models.MeterPickupRequest();
                    viewModel.MIRN = meter.MIRN;
                    viewModel.PayRollID = mhRequest.PayRollID;
                    viewModel.MeterStatus = mhRequest.MeterStatus;
                    viewModel.Location = mhRequest.Location;
                    viewModel.TransactionDate = mhRequest.TransactionDate;

                    // creating meter transaction in meter history table
                    var result = _repo.CreatePickupHistory(viewModel);

                    // checking the result from repo 
                    // if returns true then update meter status in Meter Table
                    if (result.Item1 == true)
                    {
                        var meterStatusUpdateResult = _mrepo.UpdateMeterStatus(meter.MIRN, mhRequest.MeterStatus);

                        if (meterStatusUpdateResult == true)
                        {
                            pickedMeterList.Add(metersList[i]);
                        }
                        else 
                        {
                            ModelState.AddModelError("Error", "Unable to update meter status");
                            return NotFound();
                        }
                    }
                    else
                    {
                        //if returns false the show the model state error
                        ModelState.AddModelError("Error", result.Item2);
                    }
                }
                else
                {
                    ModelState.AddModelError("Error", "Meter ID " + metersList[i] + " is not found");
                }
            }
            
            String message = " ";
            if (pickedMeterList.Count > 0)
            {
                message = "Total number of Meter: " + pickedMeterList.Count + "\nPicked Meter List: \n";
                foreach(String pickedMeter in pickedMeterList)
                {
                    message += pickedMeter + "\n";
                }
            }
            return Ok(message);
        }

        // Return Meters Single or Bulk
        // POST api/meterhistory/return
        /// <remarks>
        /// Meter Status : Inhouse = 1, Pickup = 2, Return = 3, Transfer = 4, Install = 5
        /// </remarks>
        [HttpPost("return")]
        [ValidateModel]
        public IActionResult ReturnMeters(MeterReturnBulkRequest mhRequest)
        {
            // getting MeterID[], PayRollID, MeterStatus, MeterCondition[], Location and TransactionDate

            // check is this request is for return
            if (!(mhRequest.MeterStatus.Equals(MeterStatus.Return)))
            {
                ModelState.AddModelError("Error", "Invalid request type for return meters: "+ mhRequest.MeterStatus);
                return NotFound();
            }

            // Check number of meters returned and number of meter condition received are the same.
            var meterIDs = mhRequest.MIRN;
            var meterConditions = mhRequest.MeterCondition;

            if (!(meterIDs.Length.Equals(meterConditions.Length)))
            {
                ModelState.AddModelError("Error", "Number of meter condition received does not match with number of Meters passed!");
                return NotFound();
            }

            // check Payroll ID 
            // validating PayRoll ID
            var user = _urepo.Get(mhRequest.PayRollID);
            if (user == null)
            {
                ModelState.AddModelError("Error", "PayRollID " + mhRequest.PayRollID + " is not found");
                return NotFound();
            }

            // validating location
            var warehouse = _wrepo.Get(mhRequest.Location);
            if (warehouse == null)
            {
                var depot = _drepo.Get(mhRequest.Location);
                if (depot == null)
                {
                    ModelState.AddModelError("Error", "Location  " + mhRequest.Location + " is not found");
                    return NotFound();
                }
            }

            // validating not future date
            if (mhRequest.TransactionDate > DateTime.UtcNow)
            {
                ModelState.AddModelError("Error", "Date should not be in the future date");
                return NotFound();
            }
            
            // send the success message with list of meters 
            ArrayList returnedMeterList = new ArrayList();

            // meter ID validation
            var metersList = mhRequest.MIRN;
            for (var i = 0; i < metersList.Length; i++)
            {
                var meter = _mrepo.Get(metersList[i]);

                if (meter != null)
                {
                    var viewModel = new Models.MeterReturnRequest();
                    viewModel.MIRN = meter.MIRN;
                    viewModel.PayRollID = mhRequest.PayRollID;
                    viewModel.MeterStatus = mhRequest.MeterStatus;
                    viewModel.MeterCondition = mhRequest.MeterCondition[i];
                    viewModel.Location = mhRequest.Location;
                    viewModel.TransactionDate = mhRequest.TransactionDate;

                    // creating meter transaction in meter history table
                    var result = _repo.CreateReturnHistory(viewModel);

                    // checking the result from repo 
                    // if returns true then update meter status in Meter Table
                    if (result.Item1 == true)
                    {
                        var meterStatusUpdateResult = _mrepo.UpdateMeterStatus(meter.MIRN, mhRequest.MeterStatus, mhRequest.MeterCondition[i]);
                        if (meterStatusUpdateResult == true)
                        {
                            returnedMeterList.Add(metersList[i]);
                        }
                        else
                        {
                            ModelState.AddModelError("Error", "Unable to update meter status");
                            return NotFound();
                        }
                    }
                    else
                    {
                        //if returns false the show the model state error
                        ModelState.AddModelError("Error", result.Item2);
                    }
                }
                else
                {
                    ModelState.AddModelError("Error", "Meter ID " + metersList[i] + " is not found");
                }
            }

            String message = " ";
            if (returnedMeterList.Count > 0)
            {
                message = "Total number of Meter: " + returnedMeterList.Count + "\nReturned Meter List: \n";
                foreach (String returnedMeter in returnedMeterList)
                {
                    message += returnedMeter + "\n";
                }
            }
            return Ok(message);
        }

        // Install Meter Single 
        // POST api/meterhistory/install
        /// <remarks>
        /// Meter Status : Inhouse = 1, Pickup = 2, Return = 3, Transfer = 4, Install = 5
        /// </remarks>
        [HttpPost("install")]
        [ValidateModel]
        public IActionResult InstallMeters(InstallationRequest iRequest)
        {
            // getting MeterID, MeterStatus,street no, street name, suburb, state, postcode

            // check is this request is for install
            if (!(iRequest.MeterStatus.Equals(MeterStatus.Install)))
            {
                ModelState.AddModelError("Error", "Invalid request type for install meters: " + iRequest.MeterStatus);
                return NotFound();
            }

            // meter ID validation
            var meter = _mrepo.Get(iRequest.MIRN);

            if (meter != null)
            {
                var user = _urepo.Get(iRequest.PayRollID);
                if(user != null)
                {
                    var viewModel = new Models.InstallationRequest();
                    viewModel.MIRN = meter.MIRN;
                    viewModel.PayRollID = iRequest.PayRollID;
                    viewModel.MeterStatus = iRequest.MeterStatus;
                    viewModel.StreetNo = iRequest.StreetNo;
                    viewModel.StreetName = iRequest.StreetName;
                    viewModel.Suburb = iRequest.Suburb;
                    viewModel.State = iRequest.State;
                    viewModel.PostCode = iRequest.PostCode;
                    viewModel.Comment = iRequest.Comment;
                    viewModel.TransactionDate = iRequest.TransactionDate;

                    // creating meter transaction in meter history table
                    var result = _repo.CreateInstallHistory(viewModel);

                    // checking the result from repo 
                    // if returns true then update meter status in Meter Table
                    if (result.Item1 == true)
                    {
                        var meterStatusUpdateResult = _mrepo.UpdateMeterStatus(meter.MIRN, iRequest.MeterStatus);
                        if (meterStatusUpdateResult == true)
                        {
                            return Ok("Meter ID "+ iRequest.MIRN + " is successfully installed");
                        }
                        else
                        {
                            ModelState.AddModelError("Error", "Unable to update meter status");
                            return NotFound();
                        }
                    }
                    else
                    {
                        //if returns false the show the model state error
                        ModelState.AddModelError("Error", result.Item2);
                    }
                }
                else
                {
                    ModelState.AddModelError("Error", "PayRoll ID"+ iRequest.PayRollID +" is not found");
                }
            }
            else
            {
                ModelState.AddModelError("Error", "Meter ID " + iRequest.MIRN + " is not found");
            }
            return Ok();
        }

        // Transfer Meter Single 
        // POST api/meterhistory/transfer
        /// <remarks>
        /// Meter Status : Inhouse = 1, Pickup = 2, Return = 3, Transfer = 4, Install = 5
        /// </remarks>
        [HttpPost("transfer")]
        [ValidateModel]
        public IActionResult TransferBulkMeters(TransferBulkRequest tRequest)
        {
            // getting MeterID[], MeterStatus, Name, company, Comment, Transaction Date

            // send the success message with list of meters 
            ArrayList transferMeterList = new ArrayList();

            // check is this request is for Pickup
            if (!(tRequest.MeterStatus.Equals(MeterStatus.Transfer)))
            {
                ModelState.AddModelError("Error", "Invalid request type for transfer meters: " + tRequest.MeterStatus);
                return NotFound();
            }
            if (tRequest.Name.Equals(null))
            {
                ModelState.AddModelError("Error", "Transferee Name is required");
                return NotFound();
            }
            if (tRequest.Company.Equals(null))
            {
                ModelState.AddModelError("Error", "Transferee Company is required");
                return NotFound();
            }

            // validate Meter ID before create a model
            var metersList = tRequest.MIRN;
            for (var i = 0; i < metersList.Length; i++)
            {
                var isValidMeter = _mrepo.IsValidMeter(metersList[i]);

                if (isValidMeter.Item1.Equals(false))
                {
                    ModelState.AddModelError("Error", isValidMeter.Item2);
                    return NotFound();
                }
            }

            // transfer db model
            var transferDbModel = new DbModels.Transfer();
            transferDbModel.Name = tRequest.Name;
            transferDbModel.Company = tRequest.Company;
            
            // TODO: Validate transfer model inputs Name, Company.
            var transferResult = _trepo.Add(transferDbModel);
            if(transferResult > 0)
            {
                var transferID = transferResult;
                for (var i = 0; i < metersList.Length; i++)
                {
                    var meter = _mrepo.Get(metersList[i]);
                    if (meter != null)
                    {
                        var viewModel = new Models.TransferRequest();
                        viewModel.MIRN = meter.MIRN;
                        viewModel.PayRollID = tRequest.PayRollID;
                        viewModel.MeterStatus = tRequest.MeterStatus;
                        viewModel.Comment = tRequest.Comment;
                        viewModel.TransactionDate = tRequest.TransactionDate;

                        var transferId = Convert.ToString(transferID);

                        // creating meter transaction in meter history table
                        var result = _repo.CreateTransferHistory(transferId, viewModel);

                        // checking the result from repo 
                        // if returns true then update meter status in Meter Table
                        if (result.Item1 == true)
                        {
                            var meterStatusUpdateResult = _mrepo.UpdateMeterStatus(meter.MIRN, tRequest.MeterStatus);

                            if (meterStatusUpdateResult == true)
                            {
                                transferMeterList.Add(metersList[i]);
                            }
                            else
                            {
                                ModelState.AddModelError("Error", "Unable to update meter status");
                                return NotFound();
                            }
                        }
                        else
                        {
                            //if returns false the show the model state error
                            ModelState.AddModelError("Error", result.Item2);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("Error", "Meter ID " + metersList[i] + " is not found");
                    }
                }
            }

            String message = " ";
            if (transferMeterList.Count > 0)
            {
                message = "Total number of Meter: " + transferMeterList.Count + "\nTransfered Meter List: \n";
                foreach (String transferedMeter in transferMeterList)
                {
                    message += transferedMeter + "\n";
                }
            }
            return Ok(message);
        }

        // Search meter by payroll ID and meter status - Admin UI
        [HttpPost("mh-userID")]
        [ValidateModel]
        public IActionResult GetMHByUserId(string userId, MeterStatus meterStatus)
        {
            var viewModel = new MeterHistory();
            viewModel.PayRollID = userId;
            viewModel.MeterStatus = meterStatus;

            var meterH = _repo.GetMeterHistoryByUserId(viewModel);

            if(meterH != null)
            {
                return Ok(meterH);
            }
            return Ok();
        }

        // Search meter by payroll ID and meter status - Admin UI
        [HttpPost("mh-meterID")]
        [ValidateModel]
        public IActionResult GetMHByMeterId(string meterId)
        {   
            var viewModel = new MeterHistory();
            viewModel.MIRN = meterId;

            var meterH = _repo.GetMeterHistoryByMeterId(viewModel);

            if (meterH != null)
            {
                return Ok(meterH);
            }
            return Ok();
        }
    }
}
