using PaymentGateway.Stripe.Models;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<StripeModel>(builder.Configuration.GetSection("Stripe"));
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<ProductService>();

//Cors Policy so if we hit one the endpoints on this api from a frontend
//builder.Services.AddCors(opt =>
//{
//    opt.AddPolicy(name: "CorsPolicy", builder =>
//    {
          //Change this as neccessary
//        builder.WithOrigins("http://localhost:4200")
//        .AllowAnyHeader()
//        .AllowAnyMethod();
//    });
//});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//app.UseCors("CorsPolicy");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
