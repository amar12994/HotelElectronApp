using AutoMapper;
using ElectronNET.WebApp.Domain.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection;

namespace ElectronNET.WebApp.Domain
{
    public class HotelContext : DbContext
    {
        private readonly IMapper _mapper;
        public DbSet<MenuItem> MenuItems { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CoreConnectContext"/> class.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="mapper"></param>
        public HotelContext(DbContextOptions<HotelContext> options, IMapper mapper) : base(options)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Override this method to further configure the model that was discovered by convention from the entity types       
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for this context. Databases (and other extensions) typically
        /// define extension methods on this object that allow you to configure aspects of the model that are specific
        /// to a given database.</param>
        /// <remarks>
        /// If a model is explicitly set on the options for this context (via <see cref="M:Microsoft.EntityFrameworkCore.DbContextOptionsBuilder.UseModel(Microsoft.EntityFrameworkCore.Metadata.IModel)" />)
        /// then this method will not be run.
        /// </remarks>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }
    }
}