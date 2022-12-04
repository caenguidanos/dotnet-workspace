using MediatR;
using api.Contexts.Ecommerce.Store.Domain.Service;

namespace api.Contexts.Ecommerce.Store.Application.Command
{
    public class CreateProductCommand : IRequest<Guid>
    {
        public required int Price { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required int Status { get; set; }
    }

    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
    {
        private readonly IProductService _productService;

        public CreateProductCommandHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<Guid> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            var id = await _productService.AddNewProduct(
                command.Title,
                command.Description,
                command.Status,
                command.Price,
                cancellationToken
            );

            return id;
        }
    }
}