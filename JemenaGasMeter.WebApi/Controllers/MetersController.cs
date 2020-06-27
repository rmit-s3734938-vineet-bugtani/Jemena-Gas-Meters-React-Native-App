using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using JemenaGasMeter.WebApi.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using JemenaGasMeter.WebApi.Models;
using System.Collections;
using System;

namespace JemenaGasMeter.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MetersController : Controller
    {
        private readonly MetersRepository _repo;
        private readonly IMapper _mapper;
        private readonly Validator validater = new Validator();
        private readonly Helper helper = new Helper();
        private readonly MeterHistoriesRepository _mhrepo;

        public MetersController(MetersRepository repo, IMapper mapper, MeterHistoriesRepository mhrepo)
        {
            _repo = repo;
            _mhrepo = mhrepo;
            _mapper = mapper;
        }


        // Returns all meter
        // GET: api/meters

        [HttpGet]
        [ValidateModel]
        public IActionResult GetAll()
        {
            var meters = _repo.GetAll()?.ToList();
            if (meters == null)
            {
                ModelState.AddModelError("Meter", "Not Available!");
                return NotFound();
            }

            var result = new List<Models.Meter>();
            var meterModel = _mapper.Map<List<DbModels.Meter>, List<Models.Meter>>(meters, result);

            if (!result.Any())
            {
                ModelState.AddModelError("Meter", "Not Available!");
                return NotFound();
            }
            return Ok(result);
        }

        // Returns particular meters 
        // GET api/meters/1

        [HttpGet("{id}")]
        [ValidateModel]
        public IActionResult Get(string id)
        {
            var meter = _repo.Get(id);
            if (meter == null)
            {
                ModelState.AddModelError("Error", "Meter ID "+ meter.MIRN +" is not available!");
                return NotFound();
            }
            return Ok(meter);
        }

        [HttpPost("reset")]
        [ValidateModel]
        public IActionResult ResetAll()
        {
            _repo.ResetAll();
            return Ok();
        }


        [HttpPost("add/")]
        [ValidateModel]
        public IActionResult Add([FromBody]Meter meter)
        {
            var dbMeterType = helper.ConvertMeterType(meter.MeterType);
            var dbMeterStatus = helper.ConvertMeterStatus(meter.MeterStatus);
            var dbMeterCondition = helper.ConvertMeterCondition(meter.MeterCondition);


            var dbModel = new DbModels.Meter();
            dbModel.MIRN = meter.MIRN;
            dbModel.MeterType = dbMeterType;
            dbModel.MeterStatus = dbMeterStatus;
            dbModel.MeterCondition = dbMeterCondition;
            dbModel.ExpriyDate = meter.ExpriyDate;

            var result = _repo.Add(dbModel);
            if (result.Equals("-1"))
            {
                ModelState.AddModelError("Error", "Meter ID already exists");
                return NotFound();
            }
            else
            {
                return Ok(result);
            }
        }

        [HttpPost("edit/{id}")]
        [ValidateModel]
        public IActionResult Update(string id, [FromBody]Meter meter)
        {
            // getting Pay roll ID, User
            var meterByID = _repo.Get(id);
            if (meterByID == null)
            {
                return NotFound();
            }
            else
            {
                var dbMeterType = helper.ConvertMeterType(meter.MeterType);
                var dbMeterStatus = helper.ConvertMeterStatus(meter.MeterStatus);
                var dbMeterCondition = helper.ConvertMeterCondition(meter.MeterCondition);


                var dbModel = new DbModels.Meter();
                dbModel.MIRN = id;
                dbModel.MeterType = dbMeterType;
                dbModel.MeterStatus = dbMeterStatus;
                dbModel.MeterCondition = dbMeterCondition;
                dbModel.ExpriyDate = meter.ExpriyDate;

                var result = _repo.Update(id, dbModel);
                if (result.Equals("-1"))
                {
                    ModelState.AddModelError("Error", "Invalid Meter ID");
                    return NotFound();
                }
                else
                {
                    return Ok(result);
                }
            }
        }

        [HttpDelete("delete/{id}")]
        [ValidateModel]
        public IActionResult Delete(string id)
        {
            if (id.Equals(null))
            {
                ModelState.AddModelError("Error", "Invalid Meter ID");
                return NotFound();
            }
            else
            {
                var meterHistoryByID = _mhrepo.GetHistoryByMIRN(id);
                var c = meterHistoryByID.Count();

                if (c == 0)
                {
                    var result = _repo.DeleteMeter(id);
                    if (result.Item1 == true)
                    {
                        return Ok(result);
                    }
                    else
                    {
                       // ModelState.AddModelError("Error", "Meter not found");
                        return NotFound(result);
                    }
                }
                else
                {
                    ModelState.AddModelError("Error", "This meter has one or more meter histories. So cannot delete meter!");
                    return NotFound("This meter has one or more meter histories. So cannot delete meter!");
                }
            }
        }

        [HttpGet("status/{id}")]
        [ValidateModel]
        public IActionResult IsAvailable(string id)
        {
            var meter = _repo.Get(id);
            if (meter != null)
            {
                var result = new Models.Meter();
                var meterModel = _mapper.Map<DbModels.Meter, Models.Meter>(meter, result);

                if (result.MeterStatus.Equals(Models.MeterStatus.Inhouse))
                {
                    var meterCondition = result.MeterCondition.ToString();
                    if (result.MeterCondition.Equals(Models.MeterCondition.Faulty))
                    {
                        meterCondition = result.MeterCondition.ToString();
                        ModelState.AddModelError("Error", "Meter is not available for pickup. Meter status is " + meterCondition);
                        return NotFound();
                    }
                    else if (result.MeterCondition.Equals(Models.MeterCondition.Expired))
                    {
                        meterCondition = result.MeterCondition.ToString();
                        ModelState.AddModelError("Error", "Meter is not available for pickup. Meter status is " + meterCondition);
                        return NotFound();
                    }
                    ModelState.AddModelError("Error", "Meter is Ready to Pickup");
                    return Ok();
                }
                else
                {
                    var meterState = result.MeterStatus.ToString();
                    ModelState.AddModelError("Error", "Meter is not in warehouse to pickup meter");
                    return NotFound();
                }
            }
            ModelState.AddModelError("Error", "Invalid Meter Number!");
            return NotFound();
        }

        // Change Meters status Single or Bulk
        // GET api/meters/change-status
        /// <summary>
        /// Change Meter status by passing List of Meter ID or single Meter ID with meter status.
        /// </summary>    
        /// <remarks>
        /// Meter Status : Inhouse = 1, Pickup = 2, Return = 3, Transfer = 4, Install = 5
        /// 
        /// Meter Condition : Active = 1, Faulty = 2, Expired = 3
        /// </remarks>
        [HttpPost("change-status")]
        [ValidateModel]
        public IActionResult ChangeMeterStatus(ChangeMeterStatusRequest cmsRequest)
        {
            // getting MeterID[], meter status

            // Store the meters 
            ArrayList statusChangedMeterList = new ArrayList();
            var meterStatusUpdateResult = true;

            var metersList = cmsRequest.MIRN;
            for (var i = 0; i < metersList.Length; i++)
            {
                var meter = _repo.Get(metersList[i]);

                if (meter != null)
                {
                    var meterID = meter.MIRN;
                    var validMeterStatus = validater.ValidateMeterStatus(cmsRequest.MeterStatus);
                    var validMeterCondition = validater.ValidateMeterCondition(cmsRequest.MeterCondition);

                    if(validMeterStatus == true)
                    {
                        if(validMeterCondition == true)
                        {
                            meterStatusUpdateResult = _repo.UpdateMeterStatus(meterID, cmsRequest.MeterStatus, cmsRequest.MeterCondition);
                        }
                        else
                        {
                            ModelState.AddModelError("Error", "Invalid meter condition: " + cmsRequest.MeterCondition);
                            return NotFound();
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("Error", "Invalid meter status: " + cmsRequest.MeterStatus);
                        return NotFound();
                    }


                    if (meterStatusUpdateResult == true)
                    {
                        statusChangedMeterList.Add(metersList[i]);
                    }
                    else
                    {
                        ModelState.AddModelError("Error", "Unable to update meter status");
                        return NotFound();
                    }
                }
                else
                {
                    ModelState.AddModelError("Error", "Meter ID " + metersList[i] + " is not available!");
                    return NotFound();
                }
            }
            String message = " ";
            if (statusChangedMeterList.Count > 0)
            {
                message = "Total number of Meter: " + statusChangedMeterList.Count + "\nStatus Changed Meter List: \n";
                foreach (String statusChangedMeter in statusChangedMeterList)
                {
                    message += statusChangedMeter + "\n";
                }
            }
            return Ok(message);
        }

        // Change Meters Type Single or Bulk
        // GET api/meters/change-type
        /// <summary>
        /// Change Meter type by passing List of Meter ID or single Meter ID with meter type.
        /// </summary> 
        /// <remarks>
        /// Meter Type are Domestic = 1, Commercial = 2
        /// </remarks>
        [HttpPost("change-type")]
        [ValidateModel]
        public IActionResult ChangeMeterType(ChangeMeterTypeRequest cmtRequest)
        {
            // getting MeterID[], meter type

            // Store the meters 
            ArrayList typeChangedMeterList = new ArrayList();
            var meterTypeUpdateResult = true;

            var metersList = cmtRequest.MIRN;
            for (var i = 0; i < metersList.Length; i++)
            {
                var meter = _repo.Get(metersList[i]);

                if (meter != null)
                {
                    var meterID = meter.MIRN;
                    var validMeterType = validater.ValidateMeterType(cmtRequest.MeterType);

                    if(validMeterType == true)
                    {
                       meterTypeUpdateResult = _repo.UpdateMeterType(meterID, cmtRequest.MeterType);
                    }
                    else
                    {
                        ModelState.AddModelError("Error", "Invalid meter type: " + cmtRequest.MeterType);
                        return NotFound();
                    }

                    if (meterTypeUpdateResult == true)
                    {
                        typeChangedMeterList.Add(metersList[i]);
                    }
                    else
                    {
                        ModelState.AddModelError("Error", "Invalid meter type: " + cmtRequest.MeterType);
                        return NotFound();
                    }
                }
                else
                {
                    ModelState.AddModelError("Error", "Meter ID " + metersList[i] + " is not available!");
                    return NotFound();
                }
            }
            String message = " ";
            if (typeChangedMeterList.Count > 0)
            {
                message = "Total number of Meter: " + typeChangedMeterList.Count + "\nStatus Changed Meter List: \n";
                foreach (String typeChangedMeter in typeChangedMeterList)
                {
                    message += typeChangedMeter + "\n";
                }
            }
            return Ok(message);
        }
    }
}
