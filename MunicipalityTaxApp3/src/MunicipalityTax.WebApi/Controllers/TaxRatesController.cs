namespace MunicipalityTax.WebApi.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using MunicipalityTax.Contracts.In;
    using MunicipalityTax.Contracts.Out;
    using MunicipalityTax.Services.Services;

    [Consumes("application/json", "text/xml")]
    [Produces("application/json", "text/xml")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class TaxRatesController : ControllerBase
    {
        private readonly IMunicipalityService mService;
        private readonly ITaxRatesService taxRatesService;

        public TaxRatesController(ITaxRatesService taxRatesService, IMunicipalityService mService)
        {
            this.taxRatesService = taxRatesService;
            this.mService = mService;
        }

        /// <summary>
        /// Get taxes applied in certain municipality at the given day.
        /// </summary>
        /// <param name="request">Request contains municipality name and date.</param>
        /// <returns>Taxes applied in certain municipality at the given day.</returns>
        /// <response code="400">If request is wrong.</response>
        /// <response code="404">If Municipality or tax rate doesn't exist.</response>
        [HttpHead]
        [HttpGet]// GET: api/v1/TaxRates
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<TaxRateDto>> GetMunicipalTaxRatesAtGivenDay([FromQueryAttribute] TaxRateRequest request)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest();
            }

            var municipality = this.mService.ReadAll().FirstOrDefault(m => m.MunicipalityName == request.MunicipalityName);

            if (municipality == null)
            {
                return this.NotFound("Municipality doesn't exists");
            }

            var municipalityId = municipality.Id;

            var taxRatesDto = this.taxRatesService.ReadMunicipalTaxRatesAtGivenDay(municipalityId, request);

            if (taxRatesDto == Enumerable.Empty<TaxRateDto>())
            {
                return this.NotFound("Tax rate doesn't exists");
            }

            return this.Ok(taxRatesDto);
        }
    }
}
