using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MvvmNavigationLib.Services;

namespace Lastik.ViewModels.Popups
{
    public partial class BasePopupViewModel(INavigationService closeModalNavigationService) : ObservableObject
    {
        [ObservableProperty] private bool _toClose;
        [ObservableProperty] private bool _toCloseWithModal;

        protected virtual void OnClosed(){}

        [RelayCommand]
        private void Close()
        {
            if(ToCloseWithModal) closeModalNavigationService.Navigate();
            OnClosed();
        }

        [RelayCommand]
        private void CloseContainer(object withoutModal)
        {
            if (withoutModal is not true) ToCloseWithModal = true;
            else ToClose = true;
        }
    }
}
