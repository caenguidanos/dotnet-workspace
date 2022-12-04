using FaunaDB.Types;


namespace api.Contexts.Ecommerce.Store.Infrastructure.Model
{
    class GetProductQueryBindModel
    {
        [FaunaField("id")]
        public string Id { get; set; }
        [FaunaField("title")]
        public string Title { get; set; }
        [FaunaField("description")]
        public string Description { get; set; }
        [FaunaField("price")]
        public int Price { get; set; }
        [FaunaField("status")]
        public int Status { get; set; }

        [FaunaConstructor]
        public GetProductQueryBindModel(string id, string title, string description, int price, int status)
        {
            Id = id;
            Title = title;
            Description = description;
            Price = price;
            Status = status;
        }
    }

    class SaveProductQueryBindModel
    {
        [FaunaField("title")]
        public string Title { get; set; }
        [FaunaField("description")]
        public string Description { get; set; }
        [FaunaField("price")]
        public int Price { get; set; }
        [FaunaField("status")]
        public int Status { get; set; }

        [FaunaConstructor]
        public SaveProductQueryBindModel(string title, string description, int price, int status)
        {
            Title = title;
            Description = description;
            Price = price;
            Status = status;
        }
    }
}