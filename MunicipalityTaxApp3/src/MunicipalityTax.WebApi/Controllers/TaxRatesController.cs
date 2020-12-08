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

            var municipalityWithName = this.mService.ReadAll().FirstOrDefault(m => m.MunicipalityName == request.MunicipalityName);

            if (municipalityWithName == null)
            {
                return this.NotFound("Municipality doesn't exist");
            }

            var taxRatesDto = this.taxRatesService.ReadMunicipalTaxRatesAtGivenDay(request);

            if (taxRatesDto == Enumerable.Empty<TaxRateDto>())
            {
                return this.NotFound("Tax rate doesn't exist");
            }

            return this.Ok(taxRatesDto);
        }
    }
}
