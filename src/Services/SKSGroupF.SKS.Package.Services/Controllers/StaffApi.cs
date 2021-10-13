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
using SKSGroupF.SKS.Package.BusinessLogic.Logic;
using Microsoft.Extensions.DependencyInjection;

namespace SKSGroupF.SKS.Package.Services.Controllers
{ 
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    public class StaffApiController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly ITrackingLogic logic;

        [ActivatorUtilitiesConstructor]
        public StaffApiController(IMapper mapper)
        {
            this.mapper = mapper;
            this.logic = new TrackingLogic();
        }

        public StaffApiController(IMapper mapper, ITrackingLogic logic)
        {
            this.mapper = mapper;
            this.logic = logic;
        }

        /// <summary>
        /// Report that a Parcel has been delivered at it&#x27;s final destination address. 
        /// </summary>
        /// <param name="trackingId">The tracking ID of the parcel. E.g. PYJRB4HZ6 </param>
        /// <response code="200">Successfully reported hop.</response>
        /// <response code="400">The operation failed due to an error.</response>
        /// <response code="404">Parcel does not exist with this tracking ID. </response>
        [HttpPost]
        [Route("/parcel/{trackingId}/reportDelivery/")]
        [ValidateModelState]
        [SwaggerOperation("ReportParcelDelivery")]
        [SwaggerResponse(statusCode: 400, type: typeof(Error), description: "The operation failed due to an error.")]
        public virtual IActionResult ReportParcelDelivery([FromRoute][Required][RegularExpression("^[A-Z0-9]{9}$")]string trackingId)
        {
            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(200);

            //TODO: Uncomment the next line to return response 400 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(400, default(Error));

            //TODO: Uncomment the next line to return response 404 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(404);

            try
            {
                logic.ReportParcelDelivery(trackingId);
            }
            catch
            {
                return StatusCode(404);
            }

            return StatusCode(200);
        }

        /// <summary>
        /// Report that a Parcel has arrived at a certain hop either Warehouse or Truck. 
        /// </summary>
        /// <param name="trackingId">The tracking ID of the parcel. E.g. PYJRB4HZ6 </param>
        /// <param name="code">The Code of the hop (Warehouse or Truck).</param>
        /// <response code="200">Successfully reported hop.</response>
        /// <response code="400">The operation failed due to an error.</response>
        /// <response code="404">Parcel does not exist with this tracking ID or hop with code not found. </response>
        [HttpPost]
        [Route("/parcel/{trackingId}/reportHop/{code}")]
        [ValidateModelState]
        [SwaggerOperation("ReportParcelHop")]
        [SwaggerResponse(statusCode: 400, type: typeof(Error), description: "The operation failed due to an error.")]
        public virtual IActionResult ReportParcelHop([FromRoute][Required][RegularExpression("^[A-Z0-9]{9}$")] string trackingId, [FromRoute][Required]/*[RegularExpression("^[A-Z]{4}\\d{1,4}$")]*/ string code)
        {
            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(200);

            //TODO: Uncomment the next line to return response 400 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(400, default(Error));

            //TODO: Uncomment the next line to return response 404 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(404);

            try
            {
                logic.ReportParcelHop(trackingId, code);
            }
            catch
            {
                return StatusCode(404);
            }

            return StatusCode(200);
        }
    }
}
