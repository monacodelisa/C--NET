using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Net;
using System.Web.Http;
using Microsoft.Practices.Unity;

public class Product
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Range(0.01, 1000)]
    public decimal Price { get; set; }
}

public interface IProductService
{
    IHttpActionResult Get();
    IHttpActionResult Get(int id);
    IHttpActionResult Post(Product product);
    IHttpActionResult Put(int id, Product updatedProduct);
    IHttpActionResult Delete(int id);
}

public class ProductService : IProductService
{
    private readonly List<Product> products = new List<Product>
    {
        // Initial products
    };

    public IHttpActionResult Get()
    {
        return Ok(products);
    }

    public IHttpActionResult Get(int id)
    {
        // Implementation
    }

    public IHttpActionResult Post(Product product)
    {
        // Implementation
    }

    public IHttpActionResult Put(int id, Product updatedProduct)
    {
        // Implementation
    }

    public IHttpActionResult Delete(int id)
    {
        // Implementation
    }
}

public class ProductsController : ApiController
{
    private readonly IProductService productService;

    public ProductsController(IProductService productService)
    {
        this.productService = productService;
    }

    // ... Other CRUD actions

    [Authorize]
    [Route("api/products/secure")]
    public IHttpActionResult SecureMethod()
    {
        // Implementation of a secure method
    }
}

public class ApplicationDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }
}

public static class WebApiConfig
{
    public static void Register(HttpConfiguration config)
    {
        var container = new UnityContainer();
        container.RegisterType<IProductService, ProductService>();
        config.DependencyResolver = new UnityResolver(container);

        // Enable CORS
        config.EnableCors();

        // Configure routing
        config.MapHttpAttributeRoutes();

        config.Routes.MapHttpRoute(
            name: "DefaultApi",
            routeTemplate: "api/{controller}/{id}",
            defaults: new { id = RouteParameter.Optional }
        );

        // Enable JWT authentication
        config.Filters.Add(new AuthorizeAttribute());
        config.MessageHandlers.Add(new JwtAuthenticationHandler());

        // Exception handling
        config.Services.Replace(typeof(IExceptionHandler), new GlobalExceptionHandler());
    }
}

public static class SwaggerConfig
{
    public static void Register()
    {
        GlobalConfiguration.Configuration.EnableSwagger(c => { /* Swagger configuration */ })
                                      .EnableSwaggerUi();
    }
}

public class GlobalExceptionHandler : ExceptionHandler
{
    public override void Handle(ExceptionHandlerContext context)
    {
        // Log the exception
        Log.Error(context.Exception);

        // Return a custom error response
        context.Result = new TextPlainErrorResult
        {
            Request = context.ExceptionContext.Request,
            Content = "An unexpected error occurred. Please try again later."
        };
    }
}

public class TextPlainErrorResult : IHttpActionResult
{
    public HttpRequestMessage Request { get; set; }
    public string Content { get; set; }

    public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
    {
        var response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
        {
            Content = new StringContent(Content),
            RequestMessage = Request
        };
        return Task.FromResult(response);
    }
}

public class JwtAuthenticationHandler : DelegatingHandler
{
    // Implementation of JWT authentication
}

[TestClass]
public class ProductsControllerTests
{
    [TestMethod]
    public void Get_ReturnsProducts()
    {
        // Arrange
        var controller = new ProductsController(new ProductService());

        // Act
        var result = controller.Get();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
    }
}
