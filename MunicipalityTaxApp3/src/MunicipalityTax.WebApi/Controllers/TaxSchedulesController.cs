namespace MunicipalityTax.WebApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using MunicipalityTax.Contracts.In;
    using MunicipalityTax.Services.Services;

    /// <summary>
    /// CRUD operations on tax schedules.
    /// </summary>
    // todo: implement async requests
    [Consumes("application/json", "text/xml")]
    [Produces("application/json", "text/xml")]
    [Route("api/v{version:apiVersion}/Municipalities/{municipalityId}/TaxSchedules")]
    [ApiController]
    public class TaxSchedulesController : ControllerBase
    {
        private readonly ITaxScheduleService service;

        public TaxSchedulesController(ITaxScheduleService service)
        {
            this.service = service;
        }

        /// <summary>Gets all tax schedules when municipality id is provided.</summary>
        /// <param name="municipalityId">Municipality id.</param>
        /// <param name="request">Request parameters.</param>
        /// <returns>Tax schedules.</returns>
        /// <response code="200">If request completed.</response>
        /// <response code="400">If request provided is wrong.</response>
        /// <response code="404">If Municipality or tax schedules doesn't exists.</response>
        // GET api/v1/Municipalities/1/TaxSchedules?
        [HttpHead]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces(typeof(TaxScheduleDto))]
        public ActionResult ReadAll(Guid municipalityId, [FromQuery] TaxScheduleRequest request)
        {
            var municipalityExist = this.service.IsMunicipalityExist(municipalityId);
            if (!municipalityExist)
            {
                return this.NotFound($"Municipality doesn't exists or has no schedules");
            }

            if (!this.service.HasFields(request.Fields))
            {
                return this.BadRequest();
            }

            var items = this.service.ReadWithParameters(request);

            return this.Ok(items);
        }

        /// <summary>Gets tax schedule when municipality id and tax schedule id is provided.</summary>
        /// <param name="municipalityId">Municipality id.</param>
        /// <param name="taxScheduleId">Tax schedule id.</param>
        /// <returns>Tax schedule.</returns>
        /// <response code="200">If request completed.</response>
        /// <response code="404">If municipality or tax schedule doesn't exists.</response>
        // GET api/v1/Municipalities/5/TaxSchedules/1
        [HttpGet("{taxScheduleId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetScheduleForMunicipality(Guid municipalityId, Guid taxScheduleId)
        {
            var municipalityExist = this.service.IsMunicipalityExist(municipalityId);

            if (!municipalityExist)
            {
                return this.NotFound($"Municipality doesn't exists or has no schedules");
            }

            var schedule = this.service.Read(taxScheduleId);

            return this.Ok(schedule);
        }

        /// <summary>Adds tax schedules to database.</summary>
        /// <remarks>
        /// Sample request body:
        ///
        ///     [
        ///         {
        ///             "scheduleType": "Daily",
        ///             "taxStartDate": "2021-01-02",
        ///             "tax": 0.1,
        ///             "municipalityId": "7ebced2b-e2f9-45e0-bf75-111111111100"
        ///         }
        ///     ]
        ///
        /// </remarks>
        /// <param name="municipalityId">Municipality id.</param>
        /// <param name="body">Request body.</param>
        /// <returns>Created data id.</returns>
        /// <response code="201">Returns if data sucessfuly added to database.</response>
        /// <response code="400">If data provided is wrong.</response>
        // POST api/v1/Municipalities/5/TaxSchedules/
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateAllSchedulesForMunicipality(Guid municipalityId, [FromBody] IEnumerable<TaxScheduleCreateDto> body)
        {
            var municipalityExist = this.service.IsMunicipalityExist(municipalityId);
            if (!municipalityExist)
            {
                return this.NotFound($"Municipality doesn't exists or has no schedules");
            }

            if (body.Any(x => x.MunicipalityId != municipalityId))
            {
                return this.BadRequest("Municipality Id in the route and in schedules have match eatch other");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest();
            }

            var ids = this.service.CreateAllSchedules(body);

            return this.Created($"api/Municipalities/{municipalityId}/TaxSchedules/", ids);
        }

        /// <summary>Changes existing tax schedules.</summary>
        /// <remarks>
        /// Sample request body:
        ///
        ///     [
        ///         {
        ///             "scheduleType": "Daily",
        ///             "taxStartDate": "2021-01-01",
        ///             "tax": 0.6,
        ///             "municipalityId": "7ebced2b-e2f9-45e0-bf75-111111111100"
        ///         }
        ///     ]
        ///
        /// </remarks>
        /// <param name="municipalityId">Municipality id.</param>
        /// <param name="taxScheduleId">Tax schedule id.</param>
        /// <param name="body">Request body.</param>
        /// <returns>Created body.</returns>
        /// <response code="204">Returns if data sucessfuly changed.</response>
        /// <response code="400">If data provided is wrong.</response>
        // PUT api/v1/Municipalities/5/TaxSchedules/1
        [HttpPut("{taxScheduleId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateScheduleForMunicipality(Guid municipalityId, Guid taxScheduleId, [FromBody] TaxScheduleCreateDto body)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest();
            }

            var municipalityExist = this.service.IsMunicipalityExist(municipalityId);
            if (!municipalityExist)
            {
                return this.BadRequest($"Municipality doesn't exists or has no schedules");
            }

            if (body.MunicipalityId != municipalityId)
            {
                return this.BadRequest("Municipality Id in the route and the body have match eatch other");
            }

            int retVal = this.service.UpdateSchedule(taxScheduleId, body);
            if (retVal == 0)
            {
                return this.BadRequest($"No entities found to update");
            }
            else
            {
                return this.NoContent();
            }
        }

        /// <summary>Deletes existing tax schedules.</summary>
        /// <param name="taxScheduleId">Tax schedule id.</param>
        /// <returns>No content.</returns>
        /// <response code="204">Returns if request successful.</response>
        /// <response code="404">If data provided is wrong.</response>
        // DELETE api/v1/Municipalities/5/TaxSchedules/1
        [HttpDelete("{taxScheduleId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteScheduleForMunicipality(Guid taxScheduleId)
        {
            int retVal = this.service.Delete(taxScheduleId);
            if (retVal == 0)
            {
                return this.NotFound();
            }
            else
            {
                return this.NoContent();
            }
        }
    }
}
