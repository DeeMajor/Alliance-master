using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Accommodation.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        public DbSet<Building> buildings { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Owner> owners { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<timeslot> timeslots { get; set; }
        public DbSet<ManagerTimeSlot> managerTimeSlots { get; set; }
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<Cleaners> cleaners { get; set; }
        public System.Data.Entity.DbSet<Accommodation.Models.ApprovedOwnerss> ApprovedOwners { get; set; }

        public System.Data.Entity.DbSet<Accommodation.Models.ManagerBuilding> ManagerBuildings { get; set; }

        public System.Data.Entity.DbSet<Accommodation.Models.RoomBooking> RoomBookings { get; set; }

        public System.Data.Entity.DbSet<Accommodation.Models.SubscriptionPrice> SubscriptionPrices { get; set; }

        public System.Data.Entity.DbSet<Accommodation.Models.Service> Services { get; set; }

        public System.Data.Entity.DbSet<Accommodation.Models.ServiceType> ServiceTypes { get; set; }

        public System.Data.Entity.DbSet<Accommodation.Models.RequestService> RequestServices { get; set; }

        public System.Data.Entity.DbSet<Accommodation.Models.CleanerRoaster> CleanerRoasters { get; set; }

        public System.Data.Entity.DbSet<Accommodation.Models.Maintenance> Maintenances { get; set; }

        public System.Data.Entity.DbSet<Accommodation.Models.Truck> Trucks { get; set; }

        public System.Data.Entity.DbSet<Accommodation.Models.TruckMake> TruckMakes { get; set; }

        public System.Data.Entity.DbSet<Accommodation.Models.TruckModel> TruckModels { get; set; }

        public System.Data.Entity.DbSet<Accommodation.Models.RequestTruck> RequestTrucks { get; set; }

        public System.Data.Entity.DbSet<Accommodation.Models.TruckPrice> TruckPrices { get; set; }

        public System.Data.Entity.DbSet<Accommodation.Models.CheckOut> CheckOuts { get; set; }

        //public System.Data.Entity.DbSet<Accommodation.Models.CleanerRoaster> CleanerRoasters { get; set; }

        //public System.Data.Entity.DbSet<Accommodation.Models.Service> Services { get; set; }

        //public System.Data.Entity.DbSet<Accommodation.Models.ServiceType> ServiceTypes { get; set; }
    }
}