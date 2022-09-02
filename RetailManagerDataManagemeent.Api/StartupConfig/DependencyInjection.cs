using BridgeAuthenticationConfigBetweenProject.Library;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using RetailManagerDataManagemeent.Api.AuthPolicy;
using RM.Library.DataAccesss;
using RM.Library.Internal.DataAccess;

namespace RetailManagerDataManagemeent.Api.StartupConfig;

public static class DependencyInjection
{
    public static void AddAuthenticationServices(this WebApplicationBuilder builder)
    {
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
    }

    public static void AddCostumeServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<ISqlDataAccess, SqlDataAccess>();
        builder.Services.AddSingleton<IUserData, UserData>();
    }

    public static void AddSwaggerServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen();
        builder.Services.AddSwaggerGen(option =>
        {
            option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
            option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            option.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        }
                    },
                    new string[]{}
                }
            });
        });
    }
}