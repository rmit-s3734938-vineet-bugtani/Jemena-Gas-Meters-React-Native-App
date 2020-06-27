using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using JemenaGasMeter.WebApi.Models;
using JemenaGasMeter.WebApi.DbModels;

namespace JemenaGasMeter.WebApi.Mappers
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<DbModels.User, Models.User>();
            CreateMap<DbModels.Meter, Models.Meter>();
            CreateMap<DbModels.Depot, Models.Depot>();
            CreateMap<DbModels.Transfer, Models.Transfer>();
            CreateMap<DbModels.Warehouse, Models.Warehouse>();
            CreateMap<DbModels.Installation, Models.Installation>();
            CreateMap<DbModels.MeterHistory, Models.MeterHistory>();
        }
    }
}
