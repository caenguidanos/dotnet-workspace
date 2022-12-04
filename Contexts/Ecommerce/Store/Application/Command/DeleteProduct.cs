// <copyright file="DeleteProduct.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Ecommerce.Store.Application.Command;

using Ecommerce.Store.Domain.Service;

public class DeleteProductCommand : IRequest<Guid>
{
    required public Guid Id { get; set; }
}

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Guid>
{
    private readonly IProductService productService;

    public DeleteProductCommandHandler(IProductService productService)
    {
        this.productService = productService;
    }

    public async Task<Guid> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        await this.productService.DeleteProductById(command.Id, cancellationToken);

        return command.Id;
    }
}
