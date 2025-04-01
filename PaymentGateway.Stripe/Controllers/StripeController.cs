using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PaymentGateway.Stripe.Models;
using Stripe;
using Stripe.Checkout;


namespace PaymentGateway.Stripe.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StripeController : ControllerBase
    {
        private readonly StripeModel _stripeModel;
        private readonly CustomerService _customerService;
        private readonly ProductService _productService;
        public StripeController(IOptions<StripeModel> stripeModel, CustomerService customerService, ProductService productService)
        {
            _stripeModel = stripeModel.Value;
            _customerService = customerService;
            _productService = productService;
        }

        [HttpPost("Pay")]
        public IActionResult Pay([FromBody] string priceId)
        {
            //Creating the CheckoutSession
            StripeConfiguration.ApiKey = _stripeModel.SecretKey;
            
            //Make sure you are choosing the SessionCreateOptions "option" in the Stripe.Checkout class.
            //There are 3 options all with the same name and if you mix and match them this will not work.
            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions> 
                {
                    new SessionLineItemOptions
                    {
                        Price = priceId,
                        Quantity = 1,
                    }
                },
                Mode = "payment",
                //The next 2 lines will essentially just tell the front end what page to route to next
                SuccessUrl = "http://localhost:4200/payment_success",
                CancelUrl = "http://localhost:4200/payment_failure"
            };

            //Identifying a specific customer
            //options.Customer = "cus_id";

            //Create the Seesion and pass the options.
            var service = new SessionService();
            Session Session = service.Create(options);

            return Ok(Session.Url);

        }

        [HttpPost("CreateCustomer")]
        public async Task<dynamic> CreateCustomer([FromBody] StripeCustomer customerInfo)
        {
            StripeConfiguration.ApiKey = _stripeModel.SecretKey;

            var customerOptions = new CustomerCreateOptions
            {
                Email = customerInfo.Email,
                Name = customerInfo.Name,
            };

            var customer = await _customerService.CreateAsync(customerOptions);

            return new {customer};
        }

        [HttpGet("GetAllProducts")]
        public IActionResult GetAllProducts()
        {
            StripeConfiguration.ApiKey = _stripeModel.SecretKey;

            var options = new ProductListOptions { Expand = new List<string>() {"data.default_price" }};

            var products = _productService.List(options);

            return Ok(products);

            //How to create a product programmatically
            //var productOptions = new ProductCreateOptions { Name = "Red American Flag Hat"};
            //var service = new ProductService();

            //Session session = service.Create(productOptions);
        }
    }
}
