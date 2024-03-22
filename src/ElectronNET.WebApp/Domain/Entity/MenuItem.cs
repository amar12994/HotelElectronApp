using ElectronNET.WebApp.Domain.Common;
using System;

namespace ElectronNET.WebApp.Domain.Entity
{
    public class MenuItem : AuditableEntity
    {
        public string MenuCode { get; set; }
        public string Description { get; set; }
        public string CompanyCode { get; set; }
        public string StatusCode { get; set; }
        public DateTime EffectiveDate { get; set; }
    }
}