using JemenaGasMeter.WebApi.Data;
using JemenaGasMeter.WebApi.DbModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JemenaGasMeter.WebApi.Repository
{
    public class InstallationsRepository : IDataRepository<Installation, int>
    {
        private readonly AppDbContext _context;

        public InstallationsRepository(AppDbContext context)
        {
            _context = context;
        }

        // Get all the installation
        public IEnumerable<Installation> GetAll()
        {
            var installations = _context.Installations.ToList();
            if(installations != null)
            {
                return installations;
            }
            else
            {
                return null;
            }
        }

        // Get single installation by ID 
        public Installation Get(int id)
        {
            var installation = _context.Installations.Find(id);
            if(installation != null)
            {
                return installation;
            }
            else
            {
                return null;
            }
        }

        // Remove All Installation histories
        public void DeleteAll()
        {   
            _context.Installations.RemoveRange(_context.Installations);
            _context.SaveChanges();

        }

        // Add new installation and return new installation's InstallationID
        public int Add(Installation installation)
        {
            _context.Installations.Add(installation);
            _context.SaveChanges();

            return  installation.InstallationID;
        }

        // Edit installation by InstallationID
        public int Update(int id, Installation installation)
        {
            var i = _context.Installations.FirstOrDefault(x => x.InstallationID == id);
            if (i != null)
            {
                i.MIRN = installation.MIRN;
                i.StreetNo = installation.StreetNo;
                i.StreetName = installation.StreetName;
                i.Suburb = installation.Suburb;
                i.State = installation.State;
                i.PostCode = installation.PostCode;

                _context.SaveChanges();
                return id;
            }
            else
            {
                return -1;
            }
        }
      
        // Remove Installations by InstallationID
        public int Delete(int id)
        {
            _context.Installations.Remove(_context.Installations.Find(id));
            _context.SaveChanges();

            return id;
        }

    }
}
