

using Microsoft.OpenApi.Models;
using RetailManagerDataManagemeent.Api.StartupConfig;

var builder = WebApplication.CreateBuilder(args);

builder.AddAuthenticationServices();
builder.AddCostumeServices();
builder.AddSwaggerServices();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Retail Manager Api", Version = "v1" });
});

// add authorization policy 
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin" , option =>
    {
        option.RequireClaim("jobTitle", "Admin");
    });
    options.AddPolicy("Cashier" , option =>
    {
        option.RequireClaim("jobTitle", "Cashier");
    });
});

var app = builder.Build();


app.UseSwagger();
// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
// specifying the Swagger JSON endpoint.
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
});
app.UseHttpsRedirection();
app.UseRouting();


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();