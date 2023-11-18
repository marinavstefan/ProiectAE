namespace ProiectAE2.Models
{
    public class Product
    {
        public Product()
        {
            Name = string.Empty;
            Description = string.Empty;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal Review { get; set; }
        public bool IsAvailable { get; set; }
        public string? ImagePath { get; set; }

        public static List<Product> GetAll()
        {
            var products = new List<Product>();

            products.Add(new Product { Id = 1, Name = "Adidas", Description = "Alergare", ImagePath = "img/adidas.jpg", IsAvailable = true, Price = 450 , Review=5});
            products.Add(new Product { Id = 2, Name = "Nike", Description = "Zi cu zi", ImagePath = "img/nike.jpeg", IsAvailable = true, Price = 300 , Review = 4 });
            products.Add(new Product { Id = 3, Name = "Pepe Jeans", Description = "Zi cu zi", ImagePath = "img/pepejeans.jpg", IsAvailable = true, Price = 250 , Review = 3 });
            products.Add(new Product { Id = 4, Name = "Adidas", Description = "Zi cu zi", ImagePath = "img/adidas2.jpg", IsAvailable = true, Price = 350, Review = 5 });
            products.Add(new Product { Id = 5, Name = "Nike", Description = "Zi cu zi", ImagePath = "img/nike2.jpeg", IsAvailable = true, Price = 350, Review = 5 });
            products.Add(new Product { Id = 6, Name = "Asics", Description = "Drumetii", ImagePath = "img/asics.jpg", IsAvailable = true, Price = 400, Review = 5 });

            return products;
        }
    }
}
