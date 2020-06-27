using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using JemenaGasMeter.WebApi.Repository;
using AutoMapper;
using JemenaGasMeter.WebApi.Models;
using System.Collections;

namespace JemenaGasMeter.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly UsersRepository _repo;
        private readonly IMapper _mapper;
        private readonly Helper helper = new Helper();

        public UsersController(UsersRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        /// <summary>
        /// This api is for testing in azure deployment.
        /// </summary>
        /// <param></param>    
        /// 
        [HttpGet]
        [Route("test")]
        public IEnumerable<string> Get()
        {
            return _repo.Get();
        }

        // Returns all user
        // GET: api/users
        [HttpGet]
        public IActionResult GetAll()
        {
            var users =   _repo.GetAll()?.ToList();
            if (users == null)
            {
                return NotFound();
            }

            var result = new List<Models.User>();
            var userModel = _mapper.Map<List<DbModels.User>, List<Models.User> >(users, result);

            if (!result.Any())
            {
                return NotFound();
            }

            return Ok(result);

        }

        // Returns particular user 
        // GET api/users/1
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            var user = _repo.Get(id);
            if(user == null)
            {
                return NotFound();
            }
           
            return Ok(user);
        }

        // Returns true if username and password matches the user
        // GET: api/users/1/1234
        /// <remarks>
        /// Sample output:
        ///
        ///     GET / true or false
        ///     {
        ///        "id": 1,
        ///        "PIN": "1234"
        ///     }
        ///
        /// </remarks>
        /// 
        [HttpGet]
        [Route("{id}/{pin}")]
        public bool LoggedUser(string id, string pin)
        {
            // bool result = _repo.LoginUser(id, pin);
            if (_repo.LoginUser(id, pin) == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        [HttpPost("add/")]
        [ValidateModel]
        public IActionResult Add([FromBody]User user)
        {
           
            // convert the user status to DbModel user status
            var dbUserStatus = helper.ConvertToDbModelUserStatus(user.UserStatus);

            // convert the user type to DbModel user type
            var dbUserType = helper.ConvertToDbModelUserType(user.UserType);

            var dbModel = new DbModels.User();
            dbModel.PayRollID = user.PayRollID;
            dbModel.FirstName = user.FirstName;
            dbModel.LastName = user.LastName;
            dbModel.UserStatus = dbUserStatus;
            dbModel.UserType = dbUserType;
            dbModel.PIN = user.PIN;
            dbModel.ModifyDate = user.ModifyDate;

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
        public IActionResult Update(string id, [FromBody]User user)
        {
            // getting Pay roll ID, User
            var userByID = _repo.Get(id);
            if (userByID == null)
            {
                return NotFound();
            }
            else
            {
                // convert the user status to DbModel user status
                var dbUserStatus = helper.ConvertToDbModelUserStatus(user.UserStatus);

                // convert the user type to DbModel user type
                var dbUserType = helper.ConvertToDbModelUserType(user.UserType);

                var dbModel = new DbModels.User();
               
                dbModel.FirstName = user.FirstName;
                dbModel.LastName = user.LastName;
                dbModel.UserStatus = dbUserStatus;
                dbModel.UserType = dbUserType;
                dbModel.PIN = user.PIN;
                dbModel.ModifyDate = user.ModifyDate;

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

        [Route("{id}/activate")]
        [HttpPut]
        [ValidateModel]
        public IActionResult activateUser(string id)
        {
            if (id.Equals(null))
            {
                ModelState.AddModelError("Error", "Invalid User ID");
                return NotFound();
            }
            else
            {
                var result = _repo.Active(id);
                if (result == true)
                {
                    return Ok(result);
                }
                else
                {
                    ModelState.AddModelError("Error", "User not found");
                    return NotFound();
                }
            }
        }

        [Route("{id}/inactivate")]
        [HttpPut]
        [ValidateModel]
        public IActionResult inactivateUser(string id)
        {
            if (id.Equals(null))
            {
                ModelState.AddModelError("Error", "Invalid User ID");
                return NotFound();
            }
            else
            {
                var result = _repo.InActive(id);
                if (result == true)
                {
                    return Ok(result);
                }
                else
                {
                    ModelState.AddModelError("Error", "User not found");
                    return NotFound();
                }
            }
        }

        [HttpDelete("{id}")]
        [ValidateModel]
        public IActionResult Delete(string id)
        {
            if (id.Equals(null))
            {
                ModelState.AddModelError("Error", "Invalid User ID");
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
                    ModelState.AddModelError("Error", "User not found");
                    return NotFound();
                }
            }
        }
    }
}
