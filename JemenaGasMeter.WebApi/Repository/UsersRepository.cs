using JemenaGasMeter.WebApi.Data;
using JemenaGasMeter.WebApi.DbModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JemenaGasMeter.WebApi.Repository
{
    public class UsersRepository : IDataRepository<User, string>
    {
        private readonly AppDbContext _context;
        private readonly Helper helper = new Helper();

        public UsersRepository(AppDbContext context)
        {
            _context = context;
        }

        // Get dummy 
        public IEnumerable<string> Get()
        {
            return new string[] { "firstName: Raji", "lastName: Rudhra" };
        }

        // Get all the users
        public IEnumerable<User> GetAll()
        {
            var users = _context.Users.ToList();
            if(users != null)
            {
                return users;
            }
            else
            {
                return null;
            }
        }

        // Get single user by ID 
        public User Get(string id)
        {
            var user = _context.Users.Find(id);
            if(user != null)
            {
                return user;
            }
            else
            {
                return null;
            }
        }

        // Add new user and return new user's PayRollID
        public string Add(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();

            return user.PayRollID;
        }

        // Edit user by PayRollID
        public string Update(string id, User user)
        {
            var u = _context.Users.FirstOrDefault(x => x.PayRollID == id);
            if (u != null)
            {
                u.FirstName = user.FirstName;
                u.LastName = user.LastName;
                u.PIN = user.PIN;
                u.UserStatus = user.UserStatus;
                u.UserType = user.UserType;
                u.ModifyDate = user.ModifyDate;

                _context.SaveChanges();
                return "User " + id + " Updated";
            }
            else
            {
                return "-1";
            }
        }

        // This set the Inactive to active by passing PayRollID
        public bool Active(string id)
        {
            var user = Get(id);
            if (user == null)
            {
                return false;
            }
            else
            {
                user.UserStatus = DbModels.UserStatus.Active;
                _context.SaveChanges();
                return true;
            }
        }

        // This set the Active to inactive by passing PayRollID
        public bool InActive(string id)
        {
            var user = Get(id);
            if (user == null)
            {
                return false;
            }
            else
            {
                user.UserStatus = DbModels.UserStatus.Inactive;
                _context.SaveChanges();
                return true;
            }
        }

        // This check the logging user exist or not
        public bool LoginUser(string id, string pin)
        {
            var loggedUser = _context.Users.FirstOrDefault(x => x.PayRollID == id);
            if (loggedUser == null)
            {
                return false;
            }
            else
            {
                var payRollId = loggedUser.PayRollID;
                var pinNum = loggedUser.PIN;

                if ((payRollId.Equals(id)) && (pinNum.Equals(pin)))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        // Remove user by PayRollID
        public string Delete(string id)
        {
            var customer = Get(id);
            if (customer != null)
            {
                _context.Users.Remove(_context.Users.Find(id));
                _context.SaveChanges();
                return id;
            }
            else
            {
                return "-1";
            }
        }


    }
}
