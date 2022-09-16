

using Microsoft.OpenApi.Models;
using RetailManagerDataManagemeent.Api.StartupConfig;

var builder = WebApplication.CreateBuilder(args);

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(MyAllowSpecificOrigins,
        policy =>
        {
            policy.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});
builder.AddAuthenticationServices();
builder.AddCostumeServices();
builder.AddSwaggerServices();

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
    c.RoutePrefix =  String.Empty;
});
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors(MyAllowSpecificOrigins);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


app.Run();