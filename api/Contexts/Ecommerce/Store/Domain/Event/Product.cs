using MediatR;

namespace api.Contexts.Ecommerce.Store.Domain.Event
{
    public class ProductCreatedEvent : INotification
    {
        public required string Id { get; set; }
    }

    public class ProductRemovedEvent : INotification
    {
        public required string Id { get; set; }
    }
}