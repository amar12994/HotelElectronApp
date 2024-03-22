using System;

namespace ElectronNET.WebApp.Domain.Common
{
    /// <summary>
    /// The class contains Auditable entity. 
    /// </summary>

    public abstract class AuditableEntity : BaseEntity
    {
        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        /// <value>
        /// The created date.
        /// </value>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        /// <value>
        /// The created by.
        /// </value>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the last modified date.
        /// </summary>
        /// <value>
        /// The last modified date.
        /// </value>
        public DateTime LastModifiedDate { get; set; }

        /// <summary>
        /// Gets or sets the last modified by.
        /// </summary>
        /// <value>
        /// The last modified by.
        /// </value>
        public string LastModifiedBy { get; set; }
    }
}
