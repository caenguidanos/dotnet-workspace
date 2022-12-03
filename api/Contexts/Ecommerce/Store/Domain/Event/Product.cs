using MediatR;

namespace api.Contexts.Ecommerce.Store.Domain.Event
{
    public class ProductCreatedEvent : INotification
    {
        public int Id { get; set; }
    }
}