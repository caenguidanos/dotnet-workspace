using MediatR;
using api.Contexts.Ecommerce.Store.Domain.Service;

namespace api.Contexts.Ecommerce.Store.Application.Command
{
    public class CreateProductCommand : IRequest<string>
    {
        public required string Id { get; set; }

        public required int Price { get; set; }

        public required string Title { get; set; }

        public required string Description { get; set; }

        public required int Status { get; set; }
    }

    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, string>
    {
        private readonly IProductService _productService;

        public CreateProductCommandHandler(IProductService productService)
        {
            _productService = productService;
        }

        public Task<string> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            var id = _productService.AddNewProduct(
                command.Id,
                command.Title,
                command.Description,
                command.Status,
                command.Price
            );

            return Task.FromResult(id);
        }
    }
}