using AutoMapper;
using FluentValidation;
using SKSGroupF.SKS.Package.BusinessLogic.Entities.Models;
using SKSGroupF.SKS.Package.BusinessLogic.Interfaces;
using SKSGroupF.SKS.Package.BusinessLogic.Validators;
using SKSGroupF.SKS.Package.DataAccess.Entities.Models;
using SKSGroupF.SKS.Package.DataAccess.Interfaces;
using System;
using System.Collections.Generic;

namespace SKSGroupF.SKS.Package.BusinessLogic.Logic
{
    public class WarehouseLogic : IWarehouseLogic
    {
        private readonly IHopRepository repo;
        private readonly IMapper mapper;

        public WarehouseLogic(IMapper mapper, IHopRepository repo)
        {
            this.mapper = mapper;
            this.repo = repo;
        }

        public IEnumerable<BLHop> ExportWarehouses()
        {
            try
            {
                var tmpHopList = repo.GetAll();

                List<BLHop> hopList = new List<BLHop>();
                foreach (var i in tmpHopList)
                {
                    hopList.Add(mapper.Map<BLHop>(i));
                }

                return hopList;
            }
            catch
            {
                throw new Exception();
            }
        }

        public BLHop GetWarehouse(string code)
        {
            BLHop tmpHop;

            IValidator<string> validator = new CodeValidator();

            var result = validator.Validate(code);

            if (result.IsValid)
            {
                try
                {
                    tmpHop = mapper.Map<BLHop>(repo.GetByCode(code));
                    return tmpHop;
                }
                catch
                {
                    throw;
                }
            }

            throw new ArgumentOutOfRangeException();
        }

        public void ImportWarehouses(BLWarehouse warehouse)
        {
            IValidator<BLWarehouse> validator = new WarehouseValidator();
            
            var result = validator.Validate(warehouse);

            if (!result.IsValid)
            {
                throw new ArgumentOutOfRangeException();
            }

            try
            {
                repo.Create(mapper.Map<DALWarehouse>(warehouse));
            }
            catch
            {
                throw;
            }
        }
    }
}
