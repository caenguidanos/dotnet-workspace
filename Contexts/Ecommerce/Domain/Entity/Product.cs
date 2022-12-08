namespace Contexts.Ecommerce.Domain.Entity;

using Contexts.Common.Domain;
using Contexts.Ecommerce.Domain.Model;
using Contexts.Ecommerce.Domain.ValueObject;

public class Product : Schema
{
    private readonly ProductId _id;
    private readonly ProductTitle _title;
    private readonly ProductDescription _description;
    private readonly ProductStatus _status;
    private readonly ProductPrice _price;

    public Product(ProductId id, ProductTitle title, ProductDescription description, ProductStatus status, ProductPrice price)
    {
        _id = id;
        _title = title;
        _description = description;
        _status = status;
        _price = price;
    }

    public Guid Id
    {
        get
        {
            return _id.GetValue();
        }
    }

    public int Price
    {
        get
        {
            return _price.GetValue();
        }
    }

    public string Title
    {
        get
        {
            return _title.GetValue();
        }
    }

    public string Description
    {
        get
        {
            return _description.GetValue();
        }
    }

    public ProductStatusValue Status
    {
        get
        {
            return _status.GetValue();
        }
    }
}

public class ProductEvent : Schema
{
    private readonly ProductEventId _id;
    private readonly ProductId _product;
    private readonly ProductEventName _name;

    public ProductEvent(ProductEventId id, ProductId product, ProductEventName name)
    {
        _id = id;
        _product = product;
        _name = name;
    }

    public Guid Id
    {
        get
        {
            return _id.GetValue();
        }
    }

    public Guid Product
    {
        get
        {
            return _product.GetValue();
        }
    }

    public string Name
    {
        get
        {
            return _name.GetValue();
        }
    }
}
