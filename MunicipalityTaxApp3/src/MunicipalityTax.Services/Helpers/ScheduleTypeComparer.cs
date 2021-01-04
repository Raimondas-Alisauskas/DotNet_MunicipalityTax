namespace MunicipalityTax.Services.Helpers
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using MunicipalityTax.Domain.Entities;

    public class ScheduleTypeComparer : IComparer<TaxSchedule>
    {
        public int Compare([AllowNull] TaxSchedule x, [AllowNull] TaxSchedule y)
        {
            if ((int)x.ScheduleType > (int)y.ScheduleType)
            {
                return 1;
            }

            if ((int)x.ScheduleType < (int)y.ScheduleType)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }
}
