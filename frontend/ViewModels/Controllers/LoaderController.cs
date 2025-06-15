using CommunityToolkit.Mvvm.Messaging;
using MvvmNavigationLib.Services;

namespace Lastik.ViewModels.Controllers;

public class LoaderController(INavigationService openLoaderNavigationService,
        IMessenger messenger)
{
    private readonly List<Func<Task>> _queue = [];
    public void ShowLoader() => openLoaderNavigationService.Navigate();
    public void CloseLoader() => messenger.Send(new CloseLoaderMessage(true));

    public async Task RunLoadingTask(Func<Task> task)
    {
        ShowLoader();
        _queue.Add(task);
        await task();
        _queue.Remove(task);
        if(_queue.Count==0) CloseLoader();
    }
}