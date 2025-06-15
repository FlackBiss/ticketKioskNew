using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Lastik.ViewModels;

public class CloseLoaderMessage(bool value) : ValueChangedMessage<bool>(value);
public class CalendarDateSelectedMessage(DateTime value) : ValueChangedMessage<DateTime>(value);
public class StandByChangedMessage(bool value) : ValueChangedMessage<bool>(value);


public static class MessagingExtensions
{
    public static IMessenger SendChain<T>(this IMessenger messenger,T message)
    where T : class,new()
    {
        messenger.Send(message);
        return messenger;
    }
}