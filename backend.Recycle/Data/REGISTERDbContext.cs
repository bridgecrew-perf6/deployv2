
namespace backend.Recycle.Data
{
    using backend.Recycle.Data.Models;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    public class REGISTERDbContext : IdentityDbContext<Users>
    {
        public DbSet<AvailabilityZone> AvailabilityZones { get; set; }
        public DbSet<RequestEntity> Requests { get; set; }
        public DbSet<ReceivedRequest> ReceivedRequests { get; set; }
        public DbSet<AvailabilityEmployee> AvailabilityEmployee { get; set; }

        public REGISTERDbContext(DbContextOptions<REGISTERDbContext> options) : base(options)
        {
        }

        

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Users>().HasIndex(x => x.SSID).IsUnique(true);
            builder.Entity<AvailabilityZone>().HasIndex(x => new
            {
                x.PostCode,
                x.ZoneName
            }).IsUnique(true);
            builder.Entity<ReceivedRequest>().HasIndex(x => new
            {   x.EmployeeId,
                x.RequestId
            }).IsUnique(true);


            builder.Entity<AvailabilityEmployee>().HasIndex(x => new
            {
                x.EmployeeId,
                x.AvailabilityZoneId
            }).IsUnique(true);

            base.OnModelCreating(builder);
        }
    }
}