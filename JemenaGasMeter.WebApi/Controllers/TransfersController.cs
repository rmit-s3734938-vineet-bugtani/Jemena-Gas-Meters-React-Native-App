using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using JemenaGasMeter.WebApi.Repository;
using AutoMapper;

namespace JemenaGasMeter.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransfersController : Controller
    {
        private readonly TransfersRepository _repo;
        private readonly IMapper _mapper;

        public TransfersController(TransfersRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        // Returns all transfer
        // GET: api/transfers
        [HttpGet]
        [ValidateModel]
        public IActionResult GetAll()
        {
            var transfers = _repo.GetAll()?.ToList();
            if (transfers == null)
            {
                ModelState.AddModelError("Transfer", "Not Available!");
                return NotFound();
            }

            var result = new List<Models.Transfer>();
            var transferModel = _mapper.Map<List<DbModels.Transfer>, List<Models.Transfer>>(transfers, result);

            if (!result.Any())
            {
                ModelState.AddModelError("Transfer", "Not Available!");
                return NotFound();
            }
            return Ok(result);
        }

        // Delete all transfers
        [HttpPost("delete")]
        [ValidateModel]
        public IActionResult DeleteAll()
        {
            _repo.DeleteAll();
            return Ok();
        }


        // Returns particular transfer 
        // GET api/transfers/1

        [HttpGet("{id}")]
        [ValidateModel]
        public IActionResult Get(int id)
        {
            var transfer = _repo.Get(id);
            if(transfer == null)
            {
                ModelState.AddModelError("Transfer", "Not Available!");
                return NotFound();
            }
           
            return Ok(transfer);
        }
    }
}
