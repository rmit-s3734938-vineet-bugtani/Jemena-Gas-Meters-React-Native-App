using JemenaGasMeter.WebApi.Data;
using JemenaGasMeter.WebApi.DbModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JemenaGasMeter.WebApi.Repository
{
    public class TransfersRepository : IDataRepository<Transfer, int>
    {
        private readonly AppDbContext _context;

        public TransfersRepository(AppDbContext context)
        {
            _context = context;
        }

        // Get all the transfer
        public IEnumerable<Transfer> GetAll()
        {
            var transfers = _context.Transfers.ToList();
            if(transfers != null)
            {
                return transfers;
            }
            else
            {
                return null;
            }
        }

        // Get single transfer by ID 
        public Transfer Get(int id)
        {
            var transfer = _context.Transfers.Find(id);
            if(transfer != null)
            {
                return transfer;
            }
            else
            {
                return null;
            }
        }

        // Add new transfer and return new transfer's TransferID
        public int Add(Transfer transfer)
        {
            _context.Transfers.Add(transfer);
            _context.SaveChanges();

            return  transfer.TransferID;
        }

        // Edit transfer by TransferID
        public int Update(int id, Transfer transfer)
        {
            var t = _context.Transfers.FirstOrDefault(x => x.TransferID == id);
            if (t != null)
            {
                t.Name = transfer.Name;
                t.Company = transfer.Company;

                _context.SaveChanges();
                return id;
            }
            else
            {
                return -1;
            }
        }
      
        // Remove Transfers by TransferID
        public int Delete(int id)
        {
            _context.Transfers.Remove(_context.Transfers.Find(id));
            _context.SaveChanges();

            return id;
        }

        // Remove All Transfer histories
        public void DeleteAll()
        {   
            _context.Transfers.RemoveRange(_context.Transfers);
            _context.SaveChanges();

        }

    }
}
