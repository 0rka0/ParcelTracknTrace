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
using SKSGroupF.SKS.Package.BusinessLogic.Interfaces;
using SKSGroupF.SKS.Package.BusinessLogic.Logic;
using AutoMapper;

namespace SKSGroupF.SKS.Package.Services.Controllers
{ 
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    public class ReceipientApiController : ControllerBase
    {
        private readonly IMapper _mapper;

        public ReceipientApiController(IMapper mapper)
        {
            _mapper = mapper;
        }
        /// <summary>
        /// Find the latest state of a parcel by its tracking ID. 
        /// </summary>
        /// <param name="trackingId">The tracking ID of the parcel. E.g. PYJRB4HZ6 </param>
        /// <response code="200">Parcel exists, here&#x27;s the tracking information.</response>
        /// <response code="400">The operation failed due to an error.</response>
        /// <response code="404">Parcel does not exist with this tracking ID.</response>
        [HttpGet]
        [Route("/parcel/{trackingId}")]
        [ValidateModelState]
        [SwaggerOperation("TrackParcel")]
        [SwaggerResponse(statusCode: 200, type: typeof(TrackingInformation), description: "Parcel exists, here&#x27;s the tracking information.")]
        [SwaggerResponse(statusCode: 400, type: typeof(Error), description: "The operation failed due to an error.")]
        public virtual IActionResult TrackParcel([FromRoute][Required][RegularExpression("^[A-Z0-9]{9}$")]string trackingId)
        {
            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(200, default(TrackingInformation));

            //TODO: Uncomment the next line to return response 400 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(400, default(Error));

            //TODO: Uncomment the next line to return response 404 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(404);

            ITrackingLogic logic = new TrackingLogic();

            Regex trackingIdRgx = new Regex(@"^[A-Z0-9]{9}$");

            if (!trackingIdRgx.IsMatch(trackingId))
                throw new ArgumentOutOfRangeException();

            string trackingInformationJson = null;

            try
            {
                var blParcel = logic.TrackParcel(trackingId);

                Parcel parcel = _mapper.Map<Parcel>(blParcel);
                TrackingInformation trackingInfo = _mapper.Map<TrackingInformation>(blParcel);

                trackingInformationJson = JsonConvert.SerializeObject(trackingInfo);
                //trackingInformationJson = "{\n  \"visitedHops\" : [ {\n    \"dateTime\" : \"2000-01-23T04:56:07.000+00:00\",\n    \"code\" : \"code\",\n    \"description\" : \"description\"\n  }, {\n    \"dateTime\" : \"2000-01-23T04:56:07.000+00:00\",\n    \"code\" : \"code\",\n    \"description\" : \"description\"\n  } ],\n  \"futureHops\" : [ null, null ],\n  \"state\" : \"Pickup\"\n}";
            }
            catch
            {
                return StatusCode(404, default(Error));
            }
                       
            var returnObject = trackingInformationJson != null
                ? JsonConvert.DeserializeObject<TrackingInformation>(trackingInformationJson)
                : default(TrackingInformation);            //TODO: Change the data returned
            return new ObjectResult(returnObject);
        }
    }
}
