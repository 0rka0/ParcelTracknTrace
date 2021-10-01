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
    public class LogisticsPartnerApiController : ControllerBase
    {
        private readonly IMapper _mapper;

        public LogisticsPartnerApiController(IMapper mapper)
        {
            _mapper = mapper;
        }
        /// <summary>
        /// Transfer an existing parcel into the system from the service of a logistics partner. 
        /// </summary>
        /// <param name="body"></param>
        /// <param name="trackingId">The tracking ID of the parcel. E.g. PYJRB4HZ6 </param>
        /// <response code="200">Successfully transitioned the parcel</response>
        /// <response code="400">The operation failed due to an error.</response>
        [HttpPost]
        [Route("/parcel/{trackingId}")]
        [ValidateModelState]
        [SwaggerOperation("TransitionParcel")]
        [SwaggerResponse(statusCode: 200, type: typeof(NewParcelInfo), description: "Successfully transitioned the parcel")]
        [SwaggerResponse(statusCode: 400, type: typeof(Error), description: "The operation failed due to an error.")]
        public virtual IActionResult TransitionParcel([FromBody]Parcel body, [FromRoute][Required][RegularExpression("^[A-Z0-9]{9}$")]string trackingId)
        {
            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(200, default(NewParcelInfo));

            //TODO: Uncomment the next line to return response 400 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(400, default(Error));

            IParcelLogic logic = new ParcelLogic();

            Regex trackingIdRgx = new Regex(@"^[A-Z0-9]{9}$");

            if (!trackingIdRgx.IsMatch(trackingId))
                throw new ArgumentOutOfRangeException();

            if (body.Sender == null || body.Receipient == null || body.Weight == null)
                throw new ArgumentOutOfRangeException();

            BLParcel blParcel = _mapper.Map<BLParcel>(body);
            logic.TransitionParcel(blParcel, trackingId);

            string exampleJson = null;
            exampleJson = "{\n  \"trackingId\" : \"PYJRB4HZ6\"\n}";
            
                        var example = exampleJson != null
                        ? JsonConvert.DeserializeObject<NewParcelInfo>(exampleJson)
                        : default(NewParcelInfo);            //TODO: Change the data returned
            return new ObjectResult(example);
        }
    }
}
