namespace MunicipalityTax.WebApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using MunicipalityTax.Contracts.In;
    using MunicipalityTax.Services.Services;

    // todo: implement async requests?
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

        // GET api/v1/Municipalities/1/TaxSchedules?
        [HttpHead]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

            var ids = this.service.CreateAll(body);

            return this.Created($"api/Municipalities/{municipalityId}/TaxSchedules/", ids);
        }

        // PUT api/v1/Municipalities/5/TaxSchedules/1
        [HttpPut("{taxScheduleId}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status304NotModified)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status412PreconditionFailed)]
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
                return this.NotFound($"Municipality doesn't exists or has no schedules");
            }

            if (body.MunicipalityId != municipalityId)
            {
                return this.BadRequest("Municipality Id in the route and the body have match eatch other");
            }

            int retVal = this.service.Update(taxScheduleId, body);
            if (retVal == 0)
            {
                return this.StatusCode(304, "Not Modified");
            }
            else if (retVal == -1)
            {
                return this.StatusCode(412, "DbUpdateConcurrencyException");
            }
            else
            {
                return this.Accepted(body);
            }
        }

        // DELETE api/v1/Municipalities/5/TaxSchedules/1
        [HttpDelete("{taxScheduleId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status412PreconditionFailed)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteScheduleForMunicipality(Guid taxScheduleId)
        {
            int retVal = this.service.Delete(taxScheduleId);
            if (retVal == 0)
            {
                return this.NotFound();
            }
            else if (retVal == -1)
            {
                return this.StatusCode(412, "DbUpdateConcurrencyException");
            }
            else
            {
                return this.NoContent();
            }
        }
    }
}
