using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using JemenaGasMeter.WebApi.Repository;
using AutoMapper;
using JemenaGasMeter.WebApi.Models;

namespace JemenaGasMeter.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepotsController : Controller
    {
        private readonly DepotsRepository _repo;
        private readonly MeterHistoriesRepository _mhrepo;
        private readonly IMapper _mapper;
        private readonly Helper helper = new Helper();

        public DepotsController(DepotsRepository repo, MeterHistoriesRepository mhrepo, IMapper mapper)
        {
            _repo = repo;
            _mhrepo = mhrepo;
            _mapper = mapper;
        }

        // Returns all depot
        // GET: api/depots
        
        [HttpGet]
        [ValidateModel]
        public IActionResult GetAll()
        {
            var depots = _repo.GetAll()?.ToList();
            if (depots == null)
            {
                ModelState.AddModelError("Depot", "Not Available!");
                return NotFound();
            }

            var result = new List<Models.Depot>();
            var depotModel = _mapper.Map<List<DbModels.Depot>, List<Models.Depot>>(depots, result);

            if (!result.Any())
            {
                ModelState.AddModelError("Depot", "Not Available!");
                return NotFound();
            }
            return Ok(result);
        }
        
        // Returns particular depot 
        // GET api/depots/1
        
        [HttpGet("{id}")]
        [ValidateModel]
        public IActionResult Get(string id)
        {
            var depot = _repo.Get(id);
            if(depot == null)
            {
                ModelState.AddModelError("Depot", "Not Available!");
                return NotFound();
            }
           
            return Ok(depot);
        }

        [HttpPost("add/")]
        [ValidateModel]
        public IActionResult Add([FromBody]Depot depot)
        {

            // convert the depot status to DbModel depot status
            var dbDepotStatus = helper.ConvertToDbModelStatus(depot.Status);

            var dbModel = new DbModels.Depot();
            dbModel.DepotID = depot.DepotID;
            dbModel.StreetName = depot.StreetName;
            dbModel.Suburb = depot.Suburb;
            dbModel.PostCode = depot.PostCode;
            dbModel.Status = dbDepotStatus;

            var result = _repo.Add(dbModel);
            if (result.Equals("-1"))
            {
                return NotFound();
            }
            else
            {
                return Ok(result);
            }
        }

        [HttpPost("edit/{id}")]
        [ValidateModel]
        public IActionResult Update(string id, [FromBody]Depot depot)
        {
            // getting Pay roll ID, Depot
            var depotByID = _repo.Get(id);
            if (depotByID == null)
            {
                return NotFound();
            }
            else
            {
                // convert the depot status to DbModel depot status
                var dbDepotStatus = helper.ConvertToDbModelStatus(depot.Status);

                var dbModel = new DbModels.Depot();
             
                dbModel.StreetName = depot.StreetName;
                dbModel.Suburb = depot.Suburb;
                dbModel.PostCode = depot.PostCode;
                dbModel.Status = dbDepotStatus;

                var result = _repo.Update(id, dbModel);
                if (result.Equals("-1"))
                {
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
                // ModelState.AddModelError("Error", "Invalid Depot ID");
                return NotFound("Invalid Depot ID");
            }
            else
            {
                var meterHistoryByID = _mhrepo.GetHistoryByLocationID(id);
                var c = meterHistoryByID.Count();

                if (c == 0)
                {
                    var result = _repo.DeleteDepot(id);
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
                    ModelState.AddModelError("Error", "This depot has one or more meter histories. So cannot delete meter!");
                    return NotFound("This depot has one or more meter histories. So cannot delete meter!");
                }
            }
        }
    }
}
