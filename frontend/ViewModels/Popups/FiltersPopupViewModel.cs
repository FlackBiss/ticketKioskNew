using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Lastik.Models.Schedules.Entities;
using Lastik.Models.Schedules.Messages;
using Lastik.Models.Schedules.Stores;
using MvvmNavigationLib.Services;
using MvvmNavigationLib.Stores;

namespace Lastik.ViewModels.Popups;


public partial class FiltersPopupViewModel:BasePopupViewModel
{
    private readonly IMessenger _messenger;

    private readonly FiltersStore _filtersStore;

    [ObservableProperty] private List<Hall> _filterHalls;
    [ObservableProperty] private ObservableCollection<Hall> _selectedFilterHalls=[];

    [ObservableProperty] private List<Hall> _filterTicketTypes;
    [ObservableProperty] private ObservableCollection<Hall> _selectedFilterTicketTypes = [];

    public FiltersPopupViewModel(
        IMessenger messenger,
        FiltersStore filtersStore,
        CloseNavigationService<ModalNavigationStore> closeModalNavigationService) : base(closeModalNavigationService)
    {
        _messenger = messenger;
        _filtersStore = filtersStore;
    }

    [RelayCommand]
    private async Task Loaded()
    {
        FilterHalls = await _filtersStore.GetAllFilters();
        SelectedFilterHalls = [.._filtersStore.SelectedFilterHalls];

        FilterTicketTypes = _filtersStore.EventTicketType;
        SelectedFilterTicketTypes = [.._filtersStore.SelectedEventTicketType];
    }

    [RelayCommand]
    private void ApplyFilters()
    {
        _filtersStore.SelectedFilterHalls = SelectedFilterHalls.ToList();
        _filtersStore.SelectedEventTicketType = SelectedFilterTicketTypes.ToList();
        _messenger.Send(new FiltersChangedMessage(new Tuple<List<int>, List<string>>
        (
            SelectedFilterHalls.Select(f => f.Id).ToList(),
            SelectedFilterTicketTypes.Select(f => f.Name).ToList()!)));
        CloseContainerCommand.Execute(null);
    }

    [RelayCommand]
    private void ClearFilters()
    {
        _filtersStore.SelectedFilterHalls.Clear();
        _filtersStore.SelectedEventTicketType.Clear();
        _messenger.Send(new FiltersResetMessage());
        CloseContainerCommand.Execute(null);
    }
}