using JemenaGasMeter.WebApi.Data;
using JemenaGasMeter.WebApi.DbModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JemenaGasMeter.WebApi.Repository
{
    public class DepotsRepository : IDataRepository<Depot, string>
    {
        private readonly AppDbContext _context;

        public DepotsRepository(AppDbContext context)
        {
            _context = context;
        }

        // Get all the Depots
        public IEnumerable<Depot> GetAll()
        {
            var depots = _context.Depots.ToList();
           if(depots != null)
            {
                return depots;
            }
            else
            {
                return null;
            }
        }

        // Get single Depot by ID 
        public Depot Get(string id)
        {
            var depot = _context.Depots.Find(id);
            if(depot != null)
            {
                return depot;
            }
            else
            {
                return null;
            }
        }

        // Add new depot and return new depot's DepotID
        public string Add(Depot depot)
        {
            var d = _context.Depots.FirstOrDefault(x => x.DepotID == depot.DepotID);
            if (d == null)
            {
                _context.Depots.Add(depot);
                _context.SaveChanges();

                return depot.DepotID;
            }
            else
            {
                return "-1";
            }
        }

        // Edit depot by DepotID
        public string Update(string id, Depot depot)
        {
            var d = _context.Depots.FirstOrDefault(x => x.DepotID == id);
            if (d != null)
            {
                d.StreetName = depot.StreetName;
                d.Suburb = depot.Suburb;
                d.PostCode = depot.PostCode;
                d.Status = depot.Status;

                _context.SaveChanges();
                return id;
            }
            else
            {
                return "-1";
            }
        }

        // Remove depot by DepotID
        public Tuple<bool, string> DeleteDepot(string id)
        {
            var result = Tuple.Create(true, "");
            var depot = Get(id);
            if (depot != null)
            {
                var r = Delete(id);
                if (r != id)
                {
                    return Tuple.Create(false, "Depot ID " + id + " is not found");
                }
                else
                {
                    return Tuple.Create(true, id + " is successfully deleted!");
                }
            }
            else
            {
                return Tuple.Create(false, "Depot ID " + id + " is not found");
            }
        }

        // Remove depot by DepotID
        public string Delete(string id)
        {
            if (id != null)
            {
                _context.Depots.Remove(_context.Depots.Find(id));
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
