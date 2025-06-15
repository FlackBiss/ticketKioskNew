using CommunityToolkit.Mvvm.Messaging;
using Lastik.Helpers.Logging;
using Lastik.Models;
using Lastik.Models.Cart.Entities;
using Lastik.Models.Cart.Stores;
using Lastik.Models.EventCalendar;
using Lastik.Models.Geometry.Entities.New;
using Lastik.Models.Kassa;
using Lastik.Models.Loggining;
using Lastik.Models.Terminal;
using Lastik.ViewModels;
using Lastik.ViewModels.Controls;
using Lastik.ViewModels.HallGeometry;
using Lastik.ViewModels.Popups;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MvvmNavigationLib.Services;
using MvvmNavigationLib.Services.ServiceCollectionExtensions;
using MvvmNavigationLib.Stores;

namespace Lastik.HostBuilders.Navigation
{
    public static class BuildModalNavigationExtension
    {
        public static IHostBuilder BuildModalNavigation(this IHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                services.AddSingleton<ModalNavigationStore>();
                services.AddUtilityNavigationServices<ModalNavigationStore>();

                services.AddSingleton<ServerErrorModalStore>(s => new ServerErrorModalStore(
                    s.GetRequiredService<IMessenger>(),
                    s.GetRequiredService<TerminalStore>(),
                    context.Configuration.GetValue<string>("defaultAdministrativeInfo") ?? string.Empty));

                services.AddNavigationService<LoaderViewModel, ModalNavigationStore>(s =>
                    new LoaderViewModel(
                        s.GetRequiredService<IMessenger>(),
                        s.GetRequiredService<CloseNavigationService<ModalNavigationStore>>()));


                services.AddParameterNavigationService<PaymentPopupViewModel, ModalNavigationStore, Cart>(s => cart =>
                    new PaymentPopupViewModel(
                        s.GetRequiredService<CloseNavigationService<ModalNavigationStore>>(),
                        cart,
                        s.GetRequiredService<IKassaService>(),
                        s.GetRequiredService<NavigationService<ScheduleListViewModel>>(),
                        s.GetRequiredService<CartStore>(),
                        s.GetRequiredService<UserSessionStore>()));

                services.AddParameterNavigationService<PasswordPopupViewModel, ModalNavigationStore, string>(s =>
                    password =>
                        new PasswordPopupViewModel(
                            s.GetRequiredService<CloseNavigationService<ModalNavigationStore>>(), 
                            s.GetRequiredService<TerminalStore>(),
                            password));

                services
                    .AddParameterNavigationService<CalendarPopupViewModel, ModalNavigationStore, DateTime>(
                        s=>param=>new CalendarPopupViewModel(
                            param,
                            s.GetRequiredService<IMessenger>(),
                            s.GetRequiredService<CloseNavigationService<ModalNavigationStore>>(),
                            s.GetRequiredService<EventCalendarStore>()));

                services.AddNavigationService<FiltersPopupViewModel, ModalNavigationStore>();

                services.AddParameterNavigationService<PlaceViewPopupViewModel, ModalNavigationStore,SchemeDataJson>(
                    s=>place=>new PlaceViewPopupViewModel(
                        s.GetRequiredService<CloseNavigationService<ModalNavigationStore>>(),
                        s.GetRequiredService<IMessenger>(),
                        place,
                        s.GetRequiredService<UserSessionStore>()));

            });
            return builder;
        }
    }
}
