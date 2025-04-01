namespace PaymentGateway.Stripe.Models
{
    public class StripeCustomer
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; }  = string.Empty;
    }
}
