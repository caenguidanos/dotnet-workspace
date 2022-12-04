using api.Contexts.Ecommerce.Store.Domain.Entity;
using api.Contexts.Ecommerce.Store.Domain.Model;
using api.Contexts.Ecommerce.Store.Domain.Repository;
using api.Contexts.Ecommerce.Store.Domain.ValueObject;
using api.Contexts.Ecommerce.Store.Infrastructure.Model;
using api.Contexts.Ecommerce.Store.Infrastructure.Persistence;
using FaunaDB.Client;
using FaunaDB.Types;

using static FaunaDB.Query.Language;

namespace api.Contexts.Ecommerce.Store.Infrastructure.Repository
{

    public class ProductRepository : IProductRepository
    {
        private readonly FaunaClient _db;

        public ProductRepository(DatabaseClient database)
        {
            _db = database.client;
        }

        public async Task<string> GenerateID()
        {
            var result = await _db.Query(NewId());

            IResult<string> id = result.To<string>();

            return id.Value;
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            var result = await _db.Query(
                Map(
                    Paginate(
                        Match(
                            Index("product_sort_by_ts_desc")
                        )
                    ),
                    Lambda(
                        Arr("ts", "ref"),
                        Merge(
                            Obj("id", Select(Arr("id"), Var("ref"))),
                            Select(Arr("data"), Get(Var("ref")))
                        )
                    )
                )
            );

            IEnumerable<GetProductQueryBindModel> products = Decoder.Decode<IEnumerable<GetProductQueryBindModel>>(result.At("data"));

            return products.Select(product =>
            {
                return new Product(
                    new ProductId(product.Id),
                    new ProductTitle(product.Title),
                    new ProductDescription(product.Description),
                    new ProductStatus((ProductStatusValue)product.Status),
                    new ProductPrice(product.Price)
                );
            });
        }


        public async Task<Product> GetById(string id)
        {
            try
            {
                var result = await _db.Query(
                    Merge(
                        Obj("id", id),
                        Get(Ref(Collection("Product"), id))
                    )
                );

                GetProductQueryBindModel product = Decoder.Decode<GetProductQueryBindModel>(result.At("data"));

                return new Product(
                    new ProductId(product.Id),
                    new ProductTitle(product.Title),
                    new ProductDescription(product.Description),
                    new ProductStatus((ProductStatusValue)product.Status),
                    new ProductPrice(product.Price)
                );
            }
            catch (System.Exception exception)
            {
                throw new ProductNotFoundException(exception.ToString());
            }
        }

        public async Task Save(Product product)
        {
            var bind = new SaveProductQueryBindModel(
                title: product.Title,
                description: product.Description,
                price: product.Price,
                status: (int)product.Status
            );

            var result = await _db.Query(
                Create(
                    Ref(Collection("Product"), product.Id),
                    Obj("data", Encoder.Encode(bind))
                )
            );
        }

        public async Task DeleteById(string id)
        {
            await _db.Query(
                Delete(
                    Ref(Collection("Product"), id)
                )
            );
        }
    }
}
