using MediatR;

namespace api.Contexts.Ecommerce.Store.Domain.Event
{
    public class ProductCreatedEvent : INotification
    {
        public required Guid Id { get; set; }
    }

    public class ProductRemovedEvent : INotification
    {
        public required Guid Id { get; set; }
    }
}