using Lastik.Models.Loggining;
using Lastik.Models.Schedules.Entities;
using Lastik.ViewModels.Controls;
using Lastik.ViewModels.HallGeometry;
using Lastik.ViewModels.Pages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MvvmNavigationLib.Services;

namespace Lastik.HostBuilders.ViewModels
{
    public static class BuildBodyViewModelsExtensions
    {
        public static IHostBuilder BuildBodyViewModels(this IHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddTransient<ScheduleListViewModel>();

                services.AddSingleton<CreateViewModel<EventItemViewModel, Schedule>>(
                    s => @event => new EventItemViewModel(
                        s.GetRequiredService<ParameterNavigationService<EventDetailsViewModel,Schedule>>(), 
                        @event,
                        s.GetRequiredService<UserSessionStore>()));

                services.AddSingleton<CreateViewModel<NewsItemViewModel, Schedule>>(
                    s => @event => new NewsItemViewModel(
                        s.GetRequiredService<ParameterNavigationService<ChoosePlaceViewModel, Schedule>>(),
                        @event,
                        s.GetRequiredService<UserSessionStore>()));

                services.AddSingleton<CreateViewModel<HelpViewModel, Schedule>>(
                    s => @event => new HelpViewModel(@event));
            });
            return builder;
        }
    }
}
