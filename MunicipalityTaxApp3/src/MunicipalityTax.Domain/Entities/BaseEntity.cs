namespace MunicipalityTax.Domain.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public abstract class BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
    }
}
