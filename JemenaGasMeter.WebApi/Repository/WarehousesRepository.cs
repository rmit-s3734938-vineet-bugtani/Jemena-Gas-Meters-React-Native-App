using JemenaGasMeter.WebApi.Data;
using JemenaGasMeter.WebApi.DbModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JemenaGasMeter.WebApi.Repository
{
    public class WarehousesRepository : IDataRepository<Warehouse, string>
    {
        private readonly AppDbContext _context;

        public WarehousesRepository(AppDbContext context)
        {
            _context = context;
        }

        // Get all the Warehouses
        public IEnumerable<Warehouse> GetAll()
        {
            var warehouses = _context.Warehouses.ToList();
            if(warehouses != null)
            {
                return warehouses;
            }
            else
            {
                return null;
            }
        }

        // Get single Warehouses by ID 
        public Warehouse Get(string id)
        {
            var warehouse = _context.Warehouses.Find(id);
            if(warehouse != null)
            {
                return warehouse;
            }
            else
            {
                return null;
            }
        }

        // Add new Warehouse and return new Warehouse's WarehouseID
        public string Add(Warehouse warehouse)
        {
            var d = _context.Warehouses.FirstOrDefault(x => x.WarehouseID == warehouse.WarehouseID);
            if (d == null)
            {
                _context.Warehouses.Add(warehouse);
                _context.SaveChanges();

                return warehouse.WarehouseID;
            }
            else
            {
                return "-1";
            }
        }

        // Edit warehouse by WarehouseID
        public string Update(string id, Warehouse warehouse)
        {
            var w = _context.Warehouses.FirstOrDefault(x => x.WarehouseID == id);
            if (w != null)
            {
                w.StreetName = warehouse.StreetName;
                w.Suburb = warehouse.Suburb;
                w.PostCode = warehouse.PostCode;
                w.Status = warehouse.Status;

                _context.SaveChanges();
                return id;
            }
            else
            {
                return "-1";
            }
        }

        // Remove depot by DepotID
        public Tuple<bool, string> DeleteWarehouse(string id)
        {
            var result = Tuple.Create(true, "");
            var warehouse = Get(id);
            if (warehouse != null)
            {
                var r = Delete(id);
                if (r != id)
                {
                    return Tuple.Create(false, "Warehouse ID " + id + " is not found");
                }
                else
                {
                    return Tuple.Create(true, id + " is successfully deleted!");
                }
            }
            else
            {
                return Tuple.Create(false, "Warehouse ID " + id + " is not found");
            }
        }

        // Remove Warehouses by WarehouseID
        public string Delete(string id)
        {
            _context.Warehouses.Remove(_context.Warehouses.Find(id));
            _context.SaveChanges();

            return id;
        }
    }
}
