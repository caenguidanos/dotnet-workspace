using MediatR;
using api.Contexts.Ecommerce.Store.Domain.Service;

namespace api.Contexts.Ecommerce.Store.Application.Command
{
    public class DeleteProductCommand : IRequest<Guid>
    {
        public required Guid Id { get; set; }
    }

    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Guid>
    {
        private readonly IProductService _productService;

        public DeleteProductCommandHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<Guid> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
        {
            await _productService.DeleteProductById(command.Id, cancellationToken);

            return command.Id;
        }
    }
}