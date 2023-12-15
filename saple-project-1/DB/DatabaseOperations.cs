using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;

public class Product
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Range(0.01, 1000)]
    public decimal Price { get; set; }
}

public class ApplicationDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }
}

public class DatabaseOperations
{
    public void AddProduct(Product product)
    {
        using (var context = new ApplicationDbContext())
        {
            context.Products.Add(product);
            context.SaveChanges();
        }
    }

    public List<Product> GetProducts()
    {
        using (var context = new ApplicationDbContext())
        {
            return context.Products.ToList();
        }
    }

    public Product GetProductById(int id)
    {
        using (var context = new ApplicationDbContext())
        {
            return context.Products.Find(id);
        }
    }

    public void UpdateProduct(int id, Product updatedProduct)
    {
        using (var context = new ApplicationDbContext())
        {
            var existingProduct = context.Products.Find(id);
            if (existingProduct != null)
            {
                existingProduct.Name = updatedProduct.Name;
                existingProduct.Price = updatedProduct.Price;
                context.SaveChanges();
            }
        }
    }

    public void DeleteProduct(int id)
    {
        using (var context = new ApplicationDbContext())
        {
            var productToDelete = context.Products.Find(id);
            if (productToDelete != null)
            {
                context.Products.Remove(productToDelete);
                context.SaveChanges();
            }
        }
    }
}

class Program
{
    static void Main()
    {
        var dbOperations = new DatabaseOperations();

        // Add a new product
        var newProduct = new Product { Name = "New Product", Price = 49.99M };
        dbOperations.AddProduct(newProduct);

        // Get all products and display them
        Console.WriteLine("All Products:");
        var allProducts = dbOperations.GetProducts();
        DisplayProducts(allProducts);

        // Update an existing product
        var productIdToUpdate = allProducts.First().Id;
        var updatedProduct = new Product { Name = "Updated Product", Price = 59.99M };
        dbOperations.UpdateProduct(productIdToUpdate, updatedProduct);

        // Get the updated product and display it
        Console.WriteLine("\nUpdated Product:");
        var updatedProductFromDb = dbOperations.GetProductById(productIdToUpdate);
        DisplayProducts(new List<Product> { updatedProductFromDb });

        // Delete the product
        dbOperations.DeleteProduct(productIdToUpdate);

        // Get all products after deletion and display them
        Console.WriteLine("\nProducts after Deletion:");
        var remainingProducts = dbOperations.GetProducts();
        DisplayProducts(remainingProducts);
    }

    static void DisplayProducts(List<Product> products)
    {
        foreach (var product in products)
        {
            Console.WriteLine($"ID: {product.Id}, Name: {product.Name}, Price: {product.Price:C}");
        }
    }
}
