using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Lastik.Models.Schedules.Entities;
using Lastik.ViewModels.Pages;
using MvvmNavigationLib.Services;
using MvvmNavigationLib.Stores;

namespace Lastik.ViewModels.Pages;

public partial class HelpPageViewModel(Schedule @event) : BasePageViewModel("СПРАВКА")
{
    [ObservableProperty] private Schedule _help = @event;
}

