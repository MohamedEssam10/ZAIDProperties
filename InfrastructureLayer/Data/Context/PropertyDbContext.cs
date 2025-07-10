using DomainLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer.Data.Context
{
    public class PropertyDbContext : IdentityDbContext<IdentityUser<int>, IdentityRole<int>, int>
    {
        public PropertyDbContext(DbContextOptions<PropertyDbContext> options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(builder);

            // Apply IsDeleted filter to all entities inheriting BaseEntity
            //foreach (var entityType in builder.Model.GetEntityTypes())
            //{
            //    if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType)
            //        && entityType.BaseType == null)
            //    {
            //        // e => e.IsDeleted == false
            //        var parameter = Expression.Parameter(entityType.ClrType, "e");
            //        var isDeletedProperty = Expression.Property(parameter, nameof(BaseEntity.IsDeleted));
            //        var filter = Expression.Lambda(
            //            Expression.Equal(isDeletedProperty, Expression.Constant(false)),
            //            parameter);

            //        builder.Entity(entityType.ClrType).HasQueryFilter(filter);
            //    }
            //}
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<Property> properties { get; set; }
        public DbSet<Images> images { get; set; }
        public DbSet<Message> messages { get; set; }



    }
}
