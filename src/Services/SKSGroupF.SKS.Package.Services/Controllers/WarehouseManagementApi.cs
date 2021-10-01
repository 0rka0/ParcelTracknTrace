/*
 * Parcel Logistics Service
 *
 * No description provided (generated by Swagger Codegen https://github.com/swagger-api/swagger-codegen)
 *
 * OpenAPI spec version: 1.20.0
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.SwaggerGen;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using SKSGroupF.SKS.Package.Services.Attributes;

using Microsoft.AspNetCore.Authorization;
using SKSGroupF.SKS.Package.Services.DTOs.Models;
using System.Text.RegularExpressions;
using AutoMapper;
using SKSGroupF.SKS.Package.BusinessLogic.Interfaces;
using SKSGroupF.SKS.Package.BusinessLogic;
using SKSGroupF.SKS.Package.BusinessLogic.Entities.Models;

namespace SKSGroupF.SKS.Package.Services.Controllers
{ 
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    public class WarehouseManagementApiController : ControllerBase
    {
        private readonly IMapper _mapper;

        public WarehouseManagementApiController(IMapper mapper)
        {
            _mapper = mapper;
        }
        /// <summary>
        /// Exports the hierarchy of Warehouse and Truck objects. 
        /// </summary>
        /// <response code="200">Successful response</response>
        /// <response code="400">An error occurred loading.</response>
        /// <response code="404">No hierarchy loaded yet.</response>
        [HttpGet]
        [Route("/warehouse")]
        [ValidateModelState]
        [SwaggerOperation("ExportWarehouses")]
        [SwaggerResponse(statusCode: 200, type: typeof(Warehouse), description: "Successful response")]
        [SwaggerResponse(statusCode: 400, type: typeof(Error), description: "An error occurred loading.")]
        public virtual IActionResult ExportWarehouses()
        {
            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(200, default(Warehouse));

            //TODO: Uncomment the next line to return response 400 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(400, default(Error));

            //TODO: Uncomment the next line to return response 404 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(404);

            IWarehouseLogic logic = new WarehouseLogic();
            List<Warehouse> returnObject = new List<Warehouse>();

            try
            {
                var tmp = logic.ExportWarehouses();

                List<Warehouse> warehouseList = new List<Warehouse>();
                foreach (var i in tmp)
                {
                    warehouseList.Add(_mapper.Map<Warehouse>(i));
                }

                var warehouseListJson = JsonConvert.SerializeObject(warehouseList);
                returnObject = warehouseListJson != null
                    ? JsonConvert.DeserializeObject<List<Warehouse>>(warehouseListJson)
                    : default(List<Warehouse>);
            }
            catch
            {
                return StatusCode(400, default(Error));
            }

            return new ObjectResult(returnObject);
        }

        /// <summary>
        /// Get a certain warehouse or truck by code
        /// </summary>
        /// <param name="code"></param>
        /// <response code="200">Successful response</response>
        /// <response code="400">An error occurred loading.</response>
        /// <response code="404">Warehouse id not found</response>
        [HttpGet]
        [Route("/warehouse/{code}")]
        [ValidateModelState]
        [SwaggerOperation("GetWarehouse")]
        [SwaggerResponse(statusCode: 200, type: typeof(Warehouse), description: "Successful response")]
        [SwaggerResponse(statusCode: 400, type: typeof(Error), description: "An error occurred loading.")]
        public virtual IActionResult GetWarehouse([FromRoute][Required]string code)
        {
            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(200, default(Warehouse));

            //TODO: Uncomment the next line to return response 400 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(400, default(Error));

            //TODO: Uncomment the next line to return response 404 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(404);

            IWarehouseLogic logic = new WarehouseLogic();
            string warehouseJson = null;

            Regex codeRgx = new Regex(@"^[A-Z]{4}\\d{1,4}$");

            if (!codeRgx.IsMatch(code))
                throw new ArgumentOutOfRangeException();

            try
            {
                var blWarehouse = logic.GetWarehouse(code);
                Warehouse warehouse = _mapper.Map<Warehouse>(blWarehouse);

                warehouseJson = JsonConvert.SerializeObject(warehouse);
            }
            catch
            {
                return StatusCode(404);
            }
            
            var returnObject = warehouseJson != null
                ? JsonConvert.DeserializeObject<Warehouse>(warehouseJson)
                : default(Warehouse);            //TODO: Change the data returned
            return new ObjectResult(returnObject);
        }

        /// <summary>
        /// Imports a hierarchy of Warehouse and Truck objects. 
        /// </summary>
        /// <param name="body"></param>
        /// <response code="200">Successfully loaded.</response>
        /// <response code="400">The operation failed due to an error.</response>
        [HttpPost]
        [Route("/warehouse")]
        [ValidateModelState]
        [SwaggerOperation("ImportWarehouses")]
        [SwaggerResponse(statusCode: 400, type: typeof(Error), description: "The operation failed due to an error.")]
        public virtual IActionResult ImportWarehouses([FromBody]Warehouse body)
        {
            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(200);

            //TODO: Uncomment the next line to return response 400 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(400, default(Error));

            IWarehouseLogic logic = new WarehouseLogic();


            if (body == null || body.NextHops == null)
                throw new ArgumentOutOfRangeException();

            try
            {
                BLWarehouse blWarehouse = _mapper.Map<BLWarehouse>(body);
                logic.ImportWarehouses(blWarehouse);
            }
            catch
            {
                return StatusCode(400, default(Error));
            }

            return StatusCode(200);
        }
    }
}
