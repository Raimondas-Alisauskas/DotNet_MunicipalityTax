namespace MunicipalityTax.Contracts.In
{
    public class TaxScheduleRequest
    {
        private const int MaxPageSize = 50;

        public string Id { get; set; }

        public string ScheduleType { get; set; }

        // todo: public DateTime TaxStartDate { get; set; }

        // todo: public decimal Tax { get; set; }

        // todo: public Guid MunicipalityId { get; set; }

        public string SearchQuery { get; set; }

        public string OrderBy { get; set; } = "ScheduleType";

        public int PageNumber { get; set; } = 1;

        private int pageSize = 20;

        public int PageSize
        {
            get => this.pageSize;
            set => this.pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

        public string Fields { get; set; }
    }
}
