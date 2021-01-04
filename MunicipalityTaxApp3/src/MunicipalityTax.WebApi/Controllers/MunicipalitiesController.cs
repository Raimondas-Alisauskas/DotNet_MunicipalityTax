namespace MunicipalityTax.WebApi.Controllers
{
    using System;
    using System.Linq;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using MunicipalityTax.Contracts.In;
    using MunicipalityTax.Services.Services;

    /// <summary>
    /// CRUD operations on municipalities.
    /// </summary>
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

        /// <summary>Gets all municipalities.</summary>
        /// <param name="request">Request parameters.</param>
        /// <returns>Municipalities data.</returns>
        /// <response code="200">If request completed.</response>
        /// <response code="400">If request provided is wrong.</response>
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

        /// <summary>Gets municipality.</summary>
        /// <param name="municipalityId">Municipality id.</param>
        /// <returns>Municipality data.</returns>
        /// <response code="200">If request completed.</response>
        /// <response code="404">If municipality doesn't exists.</response>
        // GET api/v1/Municipalities/5
        [HttpGet("{municipalityId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Read(Guid municipalityId)
        {
            var item = this.service.Read(municipalityId);

            if (item == null)
            {
                return this.NotFound($"Item doesn't exists");
            }

            return this.Ok(item);
        }

        /// <summary>Adds municipality to database.</summary>
        /// <remarks>
        /// Sample request body:
        ///
        ///     {
        ///         "municipalityName": "TestMunicipality"
        ///     }
        ///
        /// </remarks>
        /// <param name="body">Request body.</param>
        /// <returns>Created data id.</returns>
        /// <response code="201">Returns if data sucessfuly added to database.</response>
        /// <response code="400">If data provided is wrong.</response>
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

        /// <summary>Changes existing municipality data.</summary>
        /// <remarks>
        /// Sample request body:
        ///
        ///     {
        ///         "municipalityName": "TestMunicipality"
        ///     }
        ///
        /// </remarks>
        /// <param name="municipalityId">Municipality id.</param>
        /// <param name="body">Request body.</param>
        /// <returns>Created body.</returns>
        /// <response code="204">Returns if data sucessfuly changed.</response>
        /// <response code="400">If data provided is wrong.</response>
        // PUT api/v1/Municipalities/5
        [HttpPut("{municipalityId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Update(Guid municipalityId, [FromBody] MunicipalityCreateDto body)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest();
            }

            int retVal = this.service.Update(municipalityId, body);
            if (retVal == 0)
            {
                return this.BadRequest($"No entities found to update");
            }
            else
            {
                return this.NoContent();
            }
        }

        /// <summary>Deletes existing municipality.</summary>
        /// <param name="municipalityId">Municipality id.</param>
        /// <returns>No content.</returns>
        /// <response code="204">Returns if request successful.</response>
        /// <response code="404">If data provided is wrong.</response>
        // DELETE api/v1/<Municipalities>/5
        [HttpDelete("{municipalityId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Delete(Guid municipalityId)
        {
            int retVal = this.service.Delete(municipalityId);
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
