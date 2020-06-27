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
    public class WarehousesController : Controller
    {
        private readonly WarehousesRepository _repo;
        private readonly MeterHistoriesRepository _mhrepo;
        private readonly IMapper _mapper;
        private readonly Helper helper = new Helper();

        public WarehousesController(WarehousesRepository repo, MeterHistoriesRepository mhrepo, IMapper mapper)
        {
            _repo = repo;
            _mhrepo = mhrepo;
            _mapper = mapper;
        }

        // Returns all warehouse
        // GET: api/warehouses
       
        [HttpGet]
        public IActionResult GetAll()
        {
            var warehouses =   _repo.GetAll()?.ToList();
            if (warehouses == null)
            {
                return NotFound();
            }

            var result = new List<Models.Warehouse>();
            var warehouseModel = _mapper.Map<List<DbModels.Warehouse>, List<Models.Warehouse> >(warehouses, result);

            if (!result.Any())
            {
                return NotFound();
            }

            return Ok(result);

        }

        // Returns particular warehouse 
        // GET api/warehouses/1
       
        
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            var warehouse = _repo.Get(id);
            if(warehouse == null)
            {
                return NotFound();
            }
            return Ok(warehouse);
        }

        [HttpPost("add/")]
        [ValidateModel]
        public IActionResult Add([FromBody]Warehouse warehouse)
        {

            // convert the warehouse status to DbModel warehouse status
            var dbWarehouseStatus = helper.ConvertToDbModelStatus(warehouse.Status);

            var dbModel = new DbModels.Warehouse();
            dbModel.WarehouseID = warehouse.WarehouseID;
            dbModel.StreetName = warehouse.StreetName;
            dbModel.Suburb = warehouse.Suburb;
            dbModel.PostCode = warehouse.PostCode;
            dbModel.Status = dbWarehouseStatus;

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
        public IActionResult Update(string id, [FromBody]Warehouse warehouse)
        {
            // getting Pay roll ID, Warehouse
            var warehouseByID = _repo.Get(id);
            if (warehouseByID == null)
            {
                return NotFound();
            }
            else
            {
                // convert the warehouse status to DbModel warehouse status
                var dbWarehouseStatus = helper.ConvertToDbModelStatus(warehouse.Status);

                var dbModel = new DbModels.Warehouse();

                dbModel.StreetName = warehouse.StreetName;
                dbModel.Suburb = warehouse.Suburb;
                dbModel.PostCode = warehouse.PostCode;
                dbModel.Status = dbWarehouseStatus;

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
              //  ModelState.AddModelError("Error", "Invalid Warehouse ID");
                return NotFound("Invalid Warehouse ID");
            }
            else
            {
                var meterHistoryByID = _mhrepo.GetHistoryByLocationID(id);
                var c = meterHistoryByID.Count();

                if (c == 0)
                {
                    var result = _repo.DeleteWarehouse(id);
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
