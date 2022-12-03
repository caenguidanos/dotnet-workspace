using api.Contexts.Shared;
using api.Contexts.Ecommerce.Store.Domain.Model;

namespace api.Contexts.Ecommerce.Store.Domain.ValueObject
{
    public class ProductId : ValueObject<int>
    {
        public ProductId(int value) : base(value) { }

        public override int validate(int value)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(value.ToString());
            }

            return value;
        }
    }

    public class ProductPrice : ValueObject<int>
    {
        public ProductPrice(int value) : base(value) { }

        public override int validate(int value)
        {
            int MIN = 100;
            int MAX = 100_000;

            if (value < MIN)
            {
                throw new ArgumentOutOfRangeException(value.ToString());
            }

            if (value > MAX)
            {
                throw new ArgumentOutOfRangeException(value.ToString());
            }

            return value;
        }
    }

    public class ProductDescription : ValueObject<string>
    {
        public ProductDescription(string value) : base(value) { }

        public override string validate(string value)
        {
            int MIN_LENGTH = 5;
            int MAX_LENGTH = 600;

            if (value.Length < MIN_LENGTH)
            {
                throw new ArgumentOutOfRangeException(value);
            }

            if (value.Length > MAX_LENGTH)
            {
                throw new ArgumentOutOfRangeException(value);
            }

            return value;
        }
    }

    public class ProductStatus : ValueObject<ProductStatusValue>
    {
        public ProductStatus(ProductStatusValue value) : base(value) { }

        public override ProductStatusValue validate(ProductStatusValue value)
        {
            if (!Enum.IsDefined<ProductStatusValue>(value))
            {
                throw new ArgumentOutOfRangeException(value.ToString());
            }

            return value;
        }
    }

    public class ProductTitle : ValueObject<string>
    {
        public ProductTitle(string value) : base(value) { }

        public override string validate(string value)
        {
            int MIN_LENGTH = 5;
            int MAX_LENGTH = 256;

            if (value.Length < MIN_LENGTH)
            {
                throw new ArgumentOutOfRangeException(value);
            }

            if (value.Length > MAX_LENGTH)
            {
                throw new ArgumentOutOfRangeException(value);
            }

            return value;
        }
    }
}

