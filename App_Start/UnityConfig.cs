using Accommodation.DAL.Implementation;
using Accommodation.DAL.Interface;
using Accommodation.Models;
using Accommodation.Services.Implementation;
using Accommodation.Services.Interface;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System;
using System.Web;
using Unity;
using Unity.Injection;

namespace Accommodation
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public static class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container =
          new Lazy<IUnityContainer>(() =>
          {
              var container = new UnityContainer();
              RegisterTypes(container);
              return container;
          });

        /// <summary>
        /// Configured Unity Container.
        /// </summary>
        public static IUnityContainer Container => container.Value;
        #endregion

        /// <summary>
        /// Registers the type mappings with the Unity container.
        /// </summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>
        /// There is no need to register concrete types such as controllers or
        /// API controllers (unless you want to change the defaults), as Unity
        /// allows resolving a concrete type even if it was not previously
        /// registered.
        /// </remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below.
            // Make sure to add a Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            // TODO: Register your type's mappings here.
            // container.RegisterType<IProductRepository, ProductRepository>();
            container.RegisterType<ApplicationDbContext>();
            container.RegisterType<ApplicationSignInManager>();
            container.RegisterType<ApplicationUserManager>();

            container.RegisterType<IAuthenticationManager>(
               new InjectionFactory(c => HttpContext.Current.GetOwinContext().Authentication));

            container.RegisterType<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>(
                new InjectionConstructor(typeof(ApplicationDbContext)));

            container.RegisterType<IBuildingRepository, BuildingRepository>();
            container.RegisterType<IBuildingService, BuildingService>();

            container.RegisterType<IRoomRepository, RoomRepository>();
            container.RegisterType<IRoomService, RoomService>();

            container.RegisterType<IRoomTypeRepository, RoomTypeRepository>();
            container.RegisterType<IRoomTypeService, RoomTypeService>();

            container.RegisterType<IAppointmentRepository, AppointmentRepository>();
            container.RegisterType<IAppointmentService, AppointmentService>();

            container.RegisterType<IManagerRepository, ManagerRepository>();
            container.RegisterType<IManagerService, ManagerService>();

            container.RegisterType<IManagerBuildingRepository, ManagerBuildingRepository>();
            container.RegisterType<IManagerBuildingService, ManagerBuildingService>();

            container.RegisterType<ISubscriptionPriceRepository, SubscriptionPriceRepository>();
            container.RegisterType<ISubscriptionPriceService, SubscriptionPriceService>();

            container.RegisterType<IManagerTimeSlotRepository, ManagerTimeSlotRepository>();

   
            //container.RegisterType<ITimeSlotManagerService, T>();
            //container.RegisterType<INoOfPeopleInRoomRepository, NoOfPeopleInRoomRepository>();
            //container.RegisterType<INoOfPeopleInRoomService, NoOfPeopleInRoomService>();

        }
    }
}