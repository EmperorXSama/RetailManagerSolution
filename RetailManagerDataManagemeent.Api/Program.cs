using BridgeAuthenticationConfigBetweenProject.Library;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using RetailManagerDataManagemeent.Api.AuthPolicy;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(options =>
        {
            builder.Configuration.Bind(B2CConstants.AzureAdConfigSection, options);

            options.TokenValidationParameters.NameClaimType = "name";
        },
        options => { builder.Configuration.Bind(B2CConstants.AzureAdConfigSection, options); });

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddAuthorization(options =>
{
    // Create policy to check for the scope 'read'
    options.AddPolicy("ReadScope",
        policy => policy.Requirements.Add(new ScopeRequirement("data.view")));
    options.AddPolicy("WriteScope", 
        policy => policy.Requirements.Add(new ScopeRequirement("data.write")) );
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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