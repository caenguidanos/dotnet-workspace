// <copyright file="GetById.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Ecommerce.Store.Application.Query;

using Ecommerce.Store.Domain.Entity;
using Ecommerce.Store.Domain.Repository;

public class GetByIdQuery : IRequest<Product>
{
    required public Guid Id { get; set; }
}

public class GetByIdQueryHandler : IRequestHandler<GetByIdQuery, Product>
{
    private readonly IProductRepository productRepository;

    public GetByIdQueryHandler(IProductRepository productRepository)
    {
        this.productRepository = productRepository;
    }

    public async Task<Product> Handle(GetByIdQuery query, CancellationToken cancellationToken)
    {
        var product = await this.productRepository.GetById(query.Id, cancellationToken);

        return product;
    }
}
