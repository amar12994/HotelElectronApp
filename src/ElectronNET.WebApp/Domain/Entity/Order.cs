using System;
using System.ComponentModel.DataAnnotations;

namespace ElectronNET.WebApp.Domain.Entity
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public string MenuCode { get; set; }
        public string Description { get; set; }
        public string CompanyCode { get; set; }
        public string StatusCode { get; set; }
        public DateTime EffectiveDate { get; set; }
    }
}
