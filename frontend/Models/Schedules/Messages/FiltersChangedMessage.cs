using CommunityToolkit.Mvvm.Messaging.Messages;
using Lastik.Models.Schedules.Entities;

namespace Lastik.Models.Schedules.Messages;

public class FiltersChangedMessage(Tuple<List<int>, List<string>> filters)
    : ValueChangedMessage<Tuple<List<int>, List<string>>>(filters);

public class FiltersResetMessage;