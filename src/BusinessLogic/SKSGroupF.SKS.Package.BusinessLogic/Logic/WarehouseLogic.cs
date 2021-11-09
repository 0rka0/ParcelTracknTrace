using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger logger;

        public WarehouseLogic(IMapper mapper, IHopRepository repo, ILogger<WarehouseLogic> logger)
        {
            this.mapper = mapper;
            this.repo = repo;
            this.logger = logger;
        }

        public IEnumerable<BLHop> ExportWarehouses()
        {
            try
            {
                logger.LogDebug("Trying to get all hops from database");
                var tmpHopList = repo.GetAll();

                List<BLHop> hopList = new List<BLHop>();
                foreach (var i in tmpHopList)
                {
                    hopList.Add(mapper.Map<BLHop>(i));
                }

                logger.LogDebug("Built a list with all hops successfully");
                return hopList;
            }
            catch
            {
                logger.LogError("Failed get all hops from database.");
                throw new Exception();
            }
        }

        public BLHop GetWarehouse(string code)
        {
            BLHop tmpHop;

            logger.LogInformation("Validating code.");
            IValidator<string> validator = new CodeValidator();
            var result = validator.Validate(code);

            if (result.IsValid)
            {
                logger.LogInformation("Validation of code successful.");
                try
                {
                    logger.LogDebug("Trying to get hop by code from database.");
                    tmpHop = mapper.Map<BLHop>(repo.GetByCode(code));
                    return tmpHop;
                }
                catch
                {
                    logger.LogError("Failed to get hop.");
                    throw;
                }
            }

            logger.LogError("Failed to validate code.");
            throw new ArgumentOutOfRangeException();
        }

        public void ImportWarehouses(BLHop warehouse)
        {
            logger.LogInformation("Validating hop.");
            IValidator<BLHop> validator = new HopValidator();
            
            var result = validator.Validate(warehouse);

            if (!result.IsValid)
            {
                logger.LogError("Failed to validate hop.");
                throw new ArgumentOutOfRangeException();
            }
            logger.LogInformation("Validation of hop successful.");

            try
            {
                logger.LogDebug("Trying to create hop for database");
                repo.Create(mapper.Map<DALHop>(warehouse));
            }
            catch
            {
                logger.LogError("Failed to create hop for database.");
                throw;
            }
        }
    }
}
