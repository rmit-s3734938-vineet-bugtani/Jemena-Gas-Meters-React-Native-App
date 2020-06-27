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
    public class SearchController : Controller
    {
        private readonly MeterHistoriesRepository _repo;
        private readonly MetersRepository _mrepo;
        private readonly UsersRepository _urepo;
        private readonly WarehousesRepository _wrepo;
        private readonly DepotsRepository _drepo;
        private readonly InstallationsRepository _irepo;
        private readonly IMapper _mapper;
        private Helper helper = new Helper();


        public SearchController(MeterHistoriesRepository repo,
            MetersRepository mrepo, UsersRepository urepo,
            WarehousesRepository wrepo, DepotsRepository drepo,
            InstallationsRepository irepo, IMapper mapper)
        {
            _repo = repo;
            _mrepo = mrepo;
            _urepo = urepo;
            _wrepo = wrepo;
            _drepo = drepo;
            _irepo = irepo;
            _mapper = mapper;
        }

        // Select Meters Single or Bulk
        // GET api/meters/select
        /// <summary>
        /// Get meters by passing List of Meter ID or single Meter ID.
        /// </summary>    
        [HttpPost("view-meter-id")]
        [ValidateModel]
        public IActionResult GetBulkMeters(ViewMeterBulkRequest mbRequest)
        {
            // getting MeterID[]

            // Store the meters 
            ArrayList selectedMeterList = new ArrayList();

            var metersList = mbRequest.MIRN;
            for (var i = 0; i < metersList.Length; i++)
            {
                var meter = _mrepo.Get(metersList[i]);

                if (meter != null)
                {
                    selectedMeterList.Add(meter);
                }
                else
                {
                    ModelState.AddModelError("Error", "Meter ID " + metersList[i] + " is not available!");
                    return NotFound();
                }
            }
            return Ok(selectedMeterList);
        }

        // Get Meter Details by passing PayrollID and Meter status
        // POST api/search/view-meter
        /// <summary>
        /// Get meters by passing PayrollID and Meter Status.
        /// </summary> 
        /// <remarks>
        /// Meter Status : Inhouse = 1,Pickup = 2, Return = 3, Transfer = 4, Install = 5
        /// </remarks>
        [HttpPost("view-meter")]
        [ValidateModel]
        public IActionResult GetMeterByPayRollIDAndStatus(ViewByInstallerMeterRequest mpsRequest)
        {
            // Getting Payroll ID and Meter Status

            var viewModel = new Models.MeterHistory();

            viewModel.PayRollID = mpsRequest.PayRollID;
            viewModel.MeterStatus = mpsRequest.MeterStatus;

            // Get meter id, payroll from meterhistory table
            var result = _repo.GetMeterByUserAndStatus(viewModel);
            if (result.Item1 == false)
            {
                ModelState.AddModelError("Error", result.Item2);
                return NotFound();
            }
            var dbModelMeterList = result.Item3;
            ArrayList vMeter = new ArrayList();

            foreach (DbModels.Meter dbMeter in dbModelMeterList)
            {
                var m = helper.ConvertMeterModel(dbMeter);
                vMeter.Add(m);
            }
            return Ok(vMeter);
        }

        // Pickup Meters Single or Bulk
        // POST api/search/transactiondate
        [HttpPost("transactiondate")]
        [ValidateModel]
        public IActionResult SearchMetersByDate(ViewByDateRequest rbdRequest)
        {
            // inputs start date and end date with meter status
            // pass the model to repo
            var result = _repo.GetMeterByDate(rbdRequest);

            if (result.Item1 == false)
            {
                ModelState.AddModelError("Error", result.Item2);
                return NotFound();
            }
            var dbModeltransactionList = result.Item3;
            return Ok(result.Item3);
        }
    }
}
