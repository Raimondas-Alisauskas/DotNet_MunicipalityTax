namespace MunicipalityTax.Contracts.In
{
    using System;

    public class MunicipalityRequest : IDto
    {
        private const int MaxPageSize = 20;

        public Guid Id { get; set; }

        public string MunicipalityName { get; set; }

        public string SearchQuery { get; set; }

        public string OrderBy { get; set; } = "MunicipalityName";

        public int PageNumber { get; set; } = 1;

        private int pageSize = 5;

        public int PageSize
        {
            get => this.pageSize;
            set => this.pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

        public string Fields { get; set; }
    }
}
