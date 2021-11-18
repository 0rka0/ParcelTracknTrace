﻿using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using SKSGroupF.SKS.Package.BusinessLogic.Entities.Models;
using SKSGroupF.SKS.Package.BusinessLogic.Interfaces;
using SKSGroupF.SKS.Package.BusinessLogic.Interfaces.Exceptions;
using SKSGroupF.SKS.Package.BusinessLogic.Validators;
using SKSGroupF.SKS.Package.DataAccess.Entities.Models;
using SKSGroupF.SKS.Package.DataAccess.Interfaces;
using SKSGroupF.SKS.Package.DataAccess.Interfaces.Exceptions;
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
                catch (DALDataNotFoundException ex)
                {
                    string errorMsg = "Failed to get any hops from database";
                    logger.LogError(errorMsg);
                    throw new BLDataNotFoundException(nameof(WarehouseLogic), errorMsg, ex);
                }               
            }
            catch (Exception ex)
            {
                string errorMsg = "Failed to export warehouses from database.";
                logger.LogError(errorMsg);
                throw new BLLogicException(nameof(WarehouseLogic), errorMsg, ex);
            }
        }

        public BLHop GetWarehouse(string code)
        {
            BLHop tmpHop;
            logger.LogInformation("Validating code.");
            IValidator<string> validator = new CodeValidator();
            var result = validator.Validate(code);

            try
            {
                if (!result.IsValid)
                {
                    string errorMsg = "Failed to validate code.";
                    logger.LogError(errorMsg);
                    throw new BLValidationException(nameof(WarehouseLogic), errorMsg);
                }

                logger.LogInformation("Validation of code successful.");
                try
                {
                    logger.LogDebug("Trying to get hop by code from database.");
                    tmpHop = mapper.Map<BLHop>(repo.GetByCode(code));
                    return tmpHop;
                }
                catch (DALDataNotFoundException ex)
                {
                    string errorMsg = "Failed to get the hop by code.";
                    logger.LogError(errorMsg);
                    throw new BLDataNotFoundException(nameof(WarehouseLogic), errorMsg, ex);
                }
                catch (Exception ex)
                {
                    string errorMsg = "Failed to get hop.";
                    logger.LogError(errorMsg);
                    throw new BLDataException(nameof(WarehouseLogic), errorMsg, ex);
                }
            }
            catch (Exception ex)
            {
                string errorMsg = "Failed to get warehouse from database.";
                logger.LogError(errorMsg);
                throw new BLLogicException(nameof(WarehouseLogic), errorMsg, ex);
            }  
        }

        public void ImportWarehouses(BLHop warehouse)
        {
            logger.LogDebug("Try to validate hop.");
            IValidator<BLHop> validator = new HopValidator();
            
            var result = validator.Validate(warehouse);

            try
            {
                if (!result.IsValid)
                {
                    string errorMsg = "Failed to validate hop.";
                    logger.LogError(errorMsg);
                    throw new BLValidationException(nameof(WarehouseLogic), errorMsg);
                }
                logger.LogInformation("Validation of hop successful.");

                try
                {
                    logger.LogDebug("Trying to create hop for database");
                    repo.Create(mapper.Map<DALHop>(warehouse));
                }
                catch (Exception ex)
                {
                    string errorMsg = "Failed to create hop for database.";
                    logger.LogError(errorMsg);
                    throw new BLDataException(nameof(WarehouseLogic), errorMsg, ex);
                }
            }
            catch (Exception ex)
            {
                string errorMsg = "Failed to import warehouse into database.";
                logger.LogError(errorMsg);
                throw new BLLogicException(nameof(WarehouseLogic), errorMsg, ex);
            }
        }
    }
}
