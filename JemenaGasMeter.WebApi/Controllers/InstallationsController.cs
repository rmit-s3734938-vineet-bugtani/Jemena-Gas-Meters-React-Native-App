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
    public class InstallationsController : Controller
    {
        private readonly InstallationsRepository _repo;
        private readonly IMapper _mapper;
        private readonly Helper helper = new Helper();

        public InstallationsController(InstallationsRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }


        // Returns all installation
        // GET: api/installations
        
        [HttpGet]
        [ValidateModel]
        public IActionResult GetAll()
        {
            var installations =   _repo.GetAll()?.ToList();
            if (installations == null)
            {
                ModelState.AddModelError("Installation", "Not Available!");
                return NotFound();
            }

            var result = new List<Models.Installation>();
            var installationModel = _mapper.Map<List<DbModels.Installation>, List<Models.Installation>>(installations, result);

            if (!result.Any())
            {
                ModelState.AddModelError("Installation", "Not Available!");
                return NotFound();
            }
            return Ok(result);
        }

        // Returns particular installation 
        // GET api/installations/1
         
        [HttpGet("{id}")]
        [ValidateModel]
        public IActionResult Get(int id)
        {
            var installation = _repo.Get(id);
            if(installation == null)
            {
                ModelState.AddModelError("Installation", "Not Available!");
                return NotFound();
            }
           
            return Ok(installation);
        }

        [HttpPost("add/")]
        [ValidateModel]
        public IActionResult Add([FromBody]Installation installation)
        {

            // convert the installation status to DbModel installation status
            var dbInstallationStatus = helper.ConvertToDbModelStatus(installation.Status);

            var dbModel = new DbModels.Installation();
            dbModel.InstallationID = installation.InstallationID;
            dbModel.MIRN = installation.MIRN;
            dbModel.StreetNo = installation.StreetNo;
            dbModel.StreetName = installation.StreetName;
            dbModel.Suburb = installation.Suburb;
            dbModel.State = installation.State;
            dbModel.PostCode = installation.PostCode;
            dbModel.Status = dbInstallationStatus;

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
        public IActionResult Update(int id, [FromBody]Installation installation)
        {
            // getting Pay roll ID, Installation
            var installationByID = _repo.Get(id);
            if (installationByID == null)
            {
                return NotFound();
            }
            else
            {
                // convert the installation status to DbModel installation status
                var dbInstallationStatus = helper.ConvertToDbModelStatus(installation.Status);

                var dbModel = new DbModels.Installation();

                dbModel.MIRN = installation.MIRN;
                dbModel.StreetNo = installation.StreetNo;
                dbModel.StreetName = installation.StreetName;
                dbModel.Suburb = installation.Suburb;
                dbModel.State = installation.State;
                dbModel.PostCode = installation.PostCode;
                dbModel.Status = dbInstallationStatus;

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

        // Delete all installations
        [HttpPost("delete")]
        [ValidateModel]
        public IActionResult DeleteAll()
        {
            _repo.DeleteAll();
            return Ok();
        }

        [HttpDelete("{id}")]
        [ValidateModel]
        public IActionResult Delete(int id)
        {
            if (id.Equals(null))
            {
                ModelState.AddModelError("Error", "Invalid Installation ID");
                return NotFound();
            }
            else
            {
                var result = _repo.Delete(id);
                if (result == id)
                {
                    return Ok(result);
                }
                else
                {
                    ModelState.AddModelError("Error", "Installation not found");
                    return NotFound();
                }
            }
        }
    }
}
