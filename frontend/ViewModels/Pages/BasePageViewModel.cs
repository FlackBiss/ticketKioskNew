using CommunityToolkit.Mvvm.ComponentModel;

namespace Lastik.ViewModels.Pages;

public abstract partial class BasePageViewModel(string title) : ObservableObject
{
    [ObservableProperty] private string _title = title;
}