using JemenaGasMeter.WebApi.Data;
using JemenaGasMeter.WebApi.DbModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;

namespace JemenaGasMeter.WebApi.Repository
{
    public class MetersRepository : IDataRepository<Meter, string>
    {
        private readonly AppDbContext _context;
        private readonly Helper helper = new Helper();
        private readonly Validator validater = new Validator();

        public MetersRepository(AppDbContext context)
        {
            _context = context;
        }

        // Get all the meters
        public IEnumerable<Meter> GetAll()
        {
            var meters = _context.Meters.ToList();
            if (meters != null)
            {
                return meters;
            }
            else
            {
                return null;
            }

        }

        // Get single meter by MIRN 
        public Meter Get(string id)
        {
            var meter = _context.Meters.Find(id);
            if (meter != null)
            {
                return meter;
            }
            else
            {
                return null;
            }
        }

        // Add new meter and return new meter's MIRN
        public string Add(Meter meter)
        {
            var m = _context.Meters.FirstOrDefault(x => x.MIRN == meter.MIRN);
            if (m == null)
            {
                _context.Meters.Add(meter);
                _context.SaveChanges();

                return meter.MIRN;
            }
            else
            {
                return "-1";
            }
        }

        // Edit meter by MIRN
        public string Update(string id, Meter meter)
        {
            var m = _context.Meters.FirstOrDefault(x => x.MIRN == id);
            if (m != null)
            {
                m.MeterType = meter.MeterType;
                m.MeterCondition = meter.MeterCondition;
                m.MeterStatus = meter.MeterStatus;
                m.ExpriyDate = meter.ExpriyDate;

                _context.SaveChanges();
                return id;
            }
            else
            {
                return "-1";
            }
        }

        // Update meter status by passing meter ID
        public bool UpdateMeterStatus(string id, Models.MeterStatus meterStatus)
        {
            // convert the meter status to DbModel meter status
            var dbMeterStatus = helper.ConvertMeterStatus(meterStatus);

            var m = _context.Meters.FirstOrDefault(x => x.MIRN == id);
            if (m != null)
            {
                m.MeterStatus = dbMeterStatus;
                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        // Update meter status and meter condition by passing meter ID
        public bool UpdateMeterStatus(string id, Models.MeterStatus meterStatus, Models.MeterCondition meterCondition)
        {
            // convert the meter status to DbModel meter status
            var dbMeterStatus = helper.ConvertMeterStatus(meterStatus);

            // Convert the meter condition to DbModel meter condition
            var dbMeterCondition = helper.ConvertMeterCondition(meterCondition);

            var m = _context.Meters.FirstOrDefault(x => x.MIRN == id);
            if (m != null)
            {
                m.MeterStatus = dbMeterStatus;
                m.MeterCondition = dbMeterCondition;
                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        // Resetting all meters to active status and available for pickup
        public void ResetAll()
        {
            var meters = _context.Meters.ToArray();
            for(int i = 0;i <= meters.Length - 1;i++)
            {   
                meters[i].MeterStatus = helper.ConvertMeterStatus(Models.MeterStatus.Inhouse);
                meters[i].MeterCondition = helper.ConvertMeterCondition(Models.MeterCondition.Active);
                _context.SaveChanges();
            }
        }

        // Update meter type by passing meter ID
        public bool UpdateMeterType(string id, Models.MeterType meterType)
        {
            // convert the meter type to DbModel meter type
            var dbMeterTypes = helper.ConvertMeterType(meterType);

            var m = _context.Meters.FirstOrDefault(x => x.MIRN == id);
            if (m != null)
            {
                m.MeterType = dbMeterTypes;
                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        // Remove meter by MIRN
        public Tuple<bool, string> DeleteMeter(string id)
        {
            var result = Tuple.Create(true, "");
            var meter = Get(id);
            if (meter != null)
            {
                var r = Delete(id);
                if (r != id)
                {
                    return Tuple.Create(false, "Meter ID " + id + " is not found");
                }
                else
                {
                    return Tuple.Create(true, id + " is successfully deleted!");
                }
            }
            else
            {
                return Tuple.Create(false, "Meter ID " + id + " is not found");
            }
        }

        public string Delete(string id)
        {
            if(id != null)
            {
                _context.Meters.Remove(_context.Meters.Find(id));
                _context.SaveChanges();
                return id;
            }
            else
            {
                return "-1";
            }
        }

        public Tuple<bool, string> IsValidMeter(string id)
        {
            if(id != null)
            {
                var result = Tuple.Create(true, "");
                var meter = Get(id);
                if (meter != null)
                {
                    if (!(meter.MeterStatus.Equals(DbModels.MeterStatus.Pickup)))
                    {
                        return Tuple.Create(false, "Meter ID " + id + " is not available for transfer, current status is " + meter.MeterStatus);
                    }

                    if (!(meter.MeterCondition.Equals(DbModels.MeterCondition.Active)))
                    {
                        return Tuple.Create(false, "Meter ID " + id + " is not available for transfer, current condition is " + meter.MeterCondition);
                    }
                }
                else
                {
                    return Tuple.Create(false, "Meter ID " + id + " is not found");
                }
                return Tuple.Create(true, id);
            }
            else
            {
                return Tuple.Create(false, "Meter ID " + id + " is not found");
            }
        }
    }
}
