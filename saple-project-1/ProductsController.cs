using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}

public class ProductsController : ApiController
{
    private static List<Product> products = new List<Product>
    {
        new Product { Id = 1, Name = "Product 1", Price = 19.99M },
        new Product { Id = 2, Name = "Product 2", Price = 29.99M },
        new Product { Id = 3, Name = "Product 3", Price = 39.99M }
    };

    // GET api/products
    public IHttpActionResult Get()
    {
        return Ok(products);
    }

    // GET api/products/1
    public IHttpActionResult Get(int id)
    {
        var product = products.FirstOrDefault(p => p.Id == id);
        if (product == null)
            return NotFound();

        return Ok(product);
    }

    // POST api/products
    public IHttpActionResult Post([FromBody]Product product)
    {
        product.Id = products.Count + 1;
        products.Add(product);
        return Created(new Uri(Request.RequestUri + "/" + product.Id), product);
    }

    // PUT api/products/1
    public IHttpActionResult Put(int id, [FromBody]Product updatedProduct)
    {
        var product = products.FirstOrDefault(p => p.Id == id);
        if (product == null)
            return NotFound();

        product.Name = updatedProduct.Name;
        product.Price = updatedProduct.Price;

        return Ok(product);
    }

    // DELETE api/products/1
    public IHttpActionResult Delete(int id)
    {
        var product = products.FirstOrDefault(p => p.Id == id);
        if (product == null)
            return NotFound();

        products.Remove(product);
        return Ok(product);
    }
}
