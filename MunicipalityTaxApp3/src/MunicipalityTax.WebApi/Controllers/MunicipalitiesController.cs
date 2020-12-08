namespace MunicipalityTax.WebApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using MunicipalityTax.Contracts.In;
    using MunicipalityTax.Services.Services;

    [Consumes("application/json", "text/xml")]
    [Produces("application/json", "text/xml")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class MunicipalitiesController : ControllerBase
    {
        private readonly IMunicipalityService service;

        public MunicipalitiesController(IMunicipalityService service)
        {
            this.service = service;
        }

        // todo: add cash
        // todo: 304: Not Modified if GET request result can be cached

        // GET api/v1/Municipalities
        [HttpHead]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces(typeof(MunicipalityDto))]
        public ActionResult ReadAll([FromQuery] MunicipalityRequest request)
        {
            if (!this.service.HasFields(request.Fields))
            {
                return this.BadRequest();
            }

            var items = this.service.ReadWithParameters(request);

            return this.Ok(items);
        }

        // GET api/v1/Municipalities/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Read(Guid id)
        {
            var item = this.service.Read(id);

            if (item == null)
            {
                return this.NotFound($"Item doesn't exists");
            }

            return this.Ok(item);
        }

        // POST api/v1/Municipalities/
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Post([FromBody] MunicipalityCreateDto body)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest();
            }

            var municipalitiesWithName = this.service.ReadAll().Where(m => m.MunicipalityName == body.MunicipalityName);

            if (municipalitiesWithName.Count() != 0)
            {
                return this.BadRequest("Municipality with the same name exists");
            }

            var id = this.service.Create(body);

            return this.Created($"api/Municipalities/{id}", id);
        }

        // PUT api/v1/Municipalities/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status304NotModified)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status412PreconditionFailed)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Update(Guid id, [FromBody] MunicipalityCreateDto body)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest();
            }

            int retVal = this.service.Update(id, body);
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

        // DELETE api/v1/<Municipalities>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status412PreconditionFailed)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Delete(Guid id)
        {
            int retVal = this.service.Delete(id);
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
