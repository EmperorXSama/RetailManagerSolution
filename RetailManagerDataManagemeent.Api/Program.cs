

using RetailManagerDataManagemeent.Api.StartupConfig;

var builder = WebApplication.CreateBuilder(args);

builder.AddAuthenticationServices();
builder.AddCostumeServices();
builder.AddSwaggerServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();