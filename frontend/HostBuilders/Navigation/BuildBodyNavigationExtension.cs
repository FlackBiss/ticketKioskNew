using CommunityToolkit.Mvvm.Messaging;
using Lastik.Models;
using Lastik.Models.Cart.Entities;
using Lastik.Models.Cart.Stores;
using Lastik.Models.Geometry.Entities.New;
using Lastik.Models.Geometry.Stores;
using Lastik.Models.Loggining;
using Lastik.Models.Order;
using Lastik.Models.Schedules.Entities;
using Lastik.Models.Tickets.Stores;
using Lastik.ViewModels.Controllers;
using Lastik.ViewModels.Controls;
using Lastik.ViewModels.HallGeometry;
using Lastik.ViewModels.Pages;
using Lastik.ViewModels.Popups;
using Lastik.Views.Controls;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualBasic.ApplicationServices;
using MvvmNavigationLib.Services;
using MvvmNavigationLib.Services.ServiceCollectionExtensions;
using MvvmNavigationLib.Stores;

namespace Lastik.HostBuilders.Navigation
{
    public static class BuildBodyNavigationExtension
    {
        public static IHostBuilder BuildBodyNavigation(this IHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddSingleton<NavigationStore>();
                services.AddUtilityNavigationServices<NavigationStore>();

                services.AddNavigationService<ScheduleListViewModel, NavigationStore>();

                services.AddSingleton<CreateScheduleItem>(s => (schedules,page) => page switch
                {
                    PageTypes.Events => schedules.Select(sc=>new EventItemViewModel(
                        s.GetRequiredService<ParameterNavigationService<EventDetailsViewModel,Schedule>>(),
                        sc,
                        s.GetRequiredService<UserSessionStore>())),
                    PageTypes.News => schedules.Select(sc=>new NewsItemViewModel(
                        s.GetRequiredService<ParameterNavigationService<EventDetailsViewModel, Schedule>>(),
                        sc,
                        s.GetRequiredService<UserSessionStore>())),
                    PageTypes.Help => schedules.Select(sc => new HelpViewModel(sc)),
                    _ => throw new ArgumentOutOfRangeException(nameof(page), page, null)
                });

                services.AddParameterNavigationService<ChoosePlaceViewModel, NavigationStore, Schedule>(
                    s => schedule =>
                        new ChoosePlaceViewModel(
                            s.GetRequiredService<LoaderController>(),
                            schedule,
                            s.GetRequiredService<GeometryStore>(),
                            s.GetRequiredService<GoBackNavigationService<NavigationStore>>(),
                            s.GetRequiredService<ParameterNavigationService<CartPageViewModel, (Schedule, IEnumerable<SchemeDataJson>)>>(),
                            s.GetRequiredService<ParameterNavigationService<PlaceViewPopupViewModel, SchemeDataJson>>(),
                            s.GetRequiredService<UserSessionStore>(),
                            s.GetRequiredService<IMessenger>()));

                services.AddParameterNavigationService<EventDetailsViewModel, NavigationStore, Schedule>(
                    s => @event =>
                        new EventDetailsViewModel(
                            s.GetRequiredService<ParameterNavigationService<EventDetailsViewModel, Schedule>>(),
                            s.GetRequiredService<ParameterNavigationService<ChoosePlaceViewModel, Schedule>>(),
                            s.GetRequiredService<ParameterNavigationService<CartPageViewModel, Schedule>>(),
                            s.GetRequiredService<GoBackNavigationService<NavigationStore>>(),
                            @event,
                            s.GetRequiredService<UserSessionStore>()));

                services.AddParameterNavigationService<CartPageViewModel, ModalNavigationStore, Schedule>(
                    s => f =>
                        new CartPageViewModel(
                            s.GetRequiredService<CloseNavigationService<ModalNavigationStore>>(),
                            s.GetRequiredService<ParameterNavigationService<PaymentPopupViewModel, Cart>>(),
                            s.GetRequiredService<CartStore>(),
                            f,
                            s.GetRequiredService<NavigationService<ScheduleListViewModel>>(),
                            s.GetRequiredService<OrderStore>()));

                services.AddParameterNavigationService<CartPageViewModel, ModalNavigationStore, (Schedule cartPreview,
                        IEnumerable<SchemeDataJson> places)>(
                        s => f =>
                    new CartPageViewModel(
                        s.GetRequiredService<CloseNavigationService<ModalNavigationStore>>(),
                        s.GetRequiredService<ParameterNavigationService<PaymentPopupViewModel, Cart>>(), 
                        s.GetRequiredService<CartStore>(), 
                        f.cartPreview, 
                        f.places,
                        s.GetRequiredService<NavigationService<ScheduleListViewModel>>(),
                        s.GetRequiredService<OrderStore>()));

                services.AddParameterNavigationService<StandByPageViewModel, NavigationStore, List<MainAd>>(
                    s => mainAds => new StandByPageViewModel(
                        s.GetRequiredService<NavigationService<ScheduleListViewModel>>(),
                        s.GetRequiredService<UserSessionStore>(),
                        mainAds));

            });
            return builder;
        }
    }
}
