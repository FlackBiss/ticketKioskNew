using Lastik.Helpers.Logging;
using Newtonsoft.Json;

namespace Lastik.Models.Order;

public record Answers(
    [property: JsonProperty("order")] IReadOnlyList<object> Order,
    [property: JsonProperty("ticket")] IReadOnlyList<object> Ticket
);

public record Contacts(
    [property: JsonProperty("email")] string Email,
    [property: JsonProperty("lastName")] string LastName,
    [property: JsonProperty("firstName")] string FirstName
);

public record Order(
    [property: JsonProperty("contacts")] Contacts Contacts,
    [property: JsonProperty("answers")] Answers Answers,
    [property: JsonProperty("lang")] string Lang,
    [property: JsonProperty("payment_method")] string PaymentMethod
);



public class OrderStore(IApiHttpClient client,ILoggingService logger)
{
    //public async Task CreateOrder(Contacts contacts)
    //    =>await client.CreateOrder(new Order(contacts, new Answers([], []), "ru", "2a902653-a71d-4bbf-a176-c09e77090a93"));
}