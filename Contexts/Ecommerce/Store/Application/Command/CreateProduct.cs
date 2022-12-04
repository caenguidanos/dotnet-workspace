// <copyright file="CreateProduct.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Ecommerce.Store.Application.Command;

using Ecommerce.Store.Domain.Service;

public class CreateProductCommand : IRequest<Guid>
{
    required public int Price { get; set; }

    required public string Title { get; set; }

    required public string Description { get; set; }

    required public int Status { get; set; }
}

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
{
    private readonly IProductService productService;

    public CreateProductCommandHandler(IProductService productService)
    {
        this.productService = productService;
    }

    public async Task<Guid> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var id = await this.productService.AddNewProduct(
            command.Title,
            command.Description,
            command.Status,
            command.Price,
            cancellationToken);

        return id;
    }
}
