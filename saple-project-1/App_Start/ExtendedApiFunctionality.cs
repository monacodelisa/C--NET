using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Practices.Unity;

public class Customer
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [EmailAddress]
    public string Email { get; set; }
}

public interface ICustomerService
{
    IHttpActionResult Get();
    IHttpActionResult Get(int id);
    IHttpActionResult Post(Customer customer);
    IHttpActionResult Put(int id, Customer updatedCustomer);
    IHttpActionResult Delete(int id);
}

public class CustomerService : ICustomerService
{
    private readonly List<Customer> customers = new List<Customer>
    {
        // Initial customers
    };

    public IHttpActionResult Get()
    {
        return Ok(customers);
    }

    public IHttpActionResult Get(int id)
    {
        // Implementation
    }

    public IHttpActionResult Post(Customer customer)
    {
        // Implementation
    }

    public IHttpActionResult Put(int id, Customer updatedCustomer)
    {
        // Implementation
    }

    public IHttpActionResult Delete(int id)
    {
        // Implementation
    }
}

public class CustomersController : ApiController
{
    private readonly ICustomerService customerService;

    public CustomersController(ICustomerService customerService)
    {
        this.customerService = customerService;
    }

    // ... Other CRUD actions for customers

    [AllowAnonymous]
    [HttpGet]
    [Route("api/customers/public")]
    public IHttpActionResult PublicMethod()
    {
        // Implementation of a public method
    }
}

public class ExtendedApiFunctionality
{
    // Additional features, such as background tasks, caching, etc.

    public void PerformBackgroundTask()
    {
        // Implementation of a background task
    }
}

public class JwtAuthenticationHandler : DelegatingHandler
{
    // Implementation of JWT authentication
}

public static class ExtendedApiConfig
{
    public static void Register(HttpConfiguration config)
    {
        var container = new UnityContainer();
        container.RegisterType<ICustomerService, CustomerService>();
        config.DependencyResolver = new UnityResolver(container);

        // Enable CORS
        config.EnableCors();

        // Configure routing for customers
        config.Routes.MapHttpRoute(
            name: "CustomersApi",
            routeTemplate: "api/customers/{id}",
            defaults: new { controller = "Customers", id = RouteParameter.Optional }
        );

        // Enable JWT authentication
        config.Filters.Add(new AuthorizeAttribute());
        config.MessageHandlers.Add(new JwtAuthenticationHandler());

        // Exception handling
        config.Services.Replace(typeof(IExceptionHandler), new GlobalExceptionHandler());
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
