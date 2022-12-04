using MediatR;
using api.Contexts.Ecommerce.Store.Domain.Service;

namespace api.Contexts.Ecommerce.Store.Application.Command
{
    public class DeleteProductCommand : IRequest<string>
    {
        public required string Id { get; set; }
    }

    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, string>
    {
        private readonly IProductService _productService;

        public DeleteProductCommandHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<string> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
        {
            await _productService.DeleteProductById(command.Id, cancellationToken);

            return command.Id;
        }
    }
}