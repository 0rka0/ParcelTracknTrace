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
using SKSGroupF.SKS.Package.BusinessLogic.Logic;
using SKSGroupF.SKS.Package.BusinessLogic.Interfaces;
using SKSGroupF.SKS.Package.BusinessLogic.Entities.Models;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using SKSGroupF.SKS.Package.DataAccess.Sql;

namespace SKSGroupF.SKS.Package.Services.Controllers
{ 
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    public class SenderApiController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IParcelLogic logic;

        [ActivatorUtilitiesConstructor]
        public SenderApiController(IMapper mapper, IParcelLogic logic)
        {
            this.mapper = mapper;
            this.logic = logic;
        }

        /// <summary>
        /// Submit a new parcel to the logistics service. 
        /// </summary>
        /// <param name="body"></param>
        /// <response code="200">Successfully submitted the new parcel</response>
        /// <response code="400">The operation failed due to an error.</response>
        [HttpPost]
        [Route("/parcel")]
        [ValidateModelState]
        [SwaggerOperation("SubmitParcel")]
        [SwaggerResponse(statusCode: 200, type: typeof(NewParcelInfo), description: "Successfully submitted the new parcel")]
        [SwaggerResponse(statusCode: 400, type: typeof(Error), description: "The operation failed due to an error.")]
        public virtual IActionResult SubmitParcel([FromBody]Parcel body)
        {
            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(200, default(NewParcelInfo));

            //TODO: Uncomment the next line to return response 400 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(400, default(Error));

            string trackingIdJson = null;

            try
            {
                BLParcel blParcel = mapper.Map<BLParcel>(body);
                string trackingId = logic.SubmitParcel(blParcel);

                trackingIdJson = $"{{\n  \"trackingId\" : \"{trackingId}\"\n}}";
            }
            catch
            {
                return StatusCode(400, default(Error));
            }
            
            var returnObject = trackingIdJson != null
                ? JsonConvert.DeserializeObject<NewParcelInfo>(trackingIdJson)
                : default(NewParcelInfo);          
            return new ObjectResult(returnObject);
        }
    }
}
