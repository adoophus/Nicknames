using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Nicknames.Server.Filters;
using Nicknames.Server.Middleware;
using Nicknames.Server.Services;
using Nicknames.Server.Storage;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ** Services **
//
// Adding all the services required for the application
// component.

builder.Services.AddStorage(builder.Configuration);
builder.Services.AddServices(builder.Configuration);

// ** Data validation **
//
// 1. User retrieval filter - grabs the stored user for
// the JWT token we are receiving from the endpoint.
//
// 2. Model state validation filter - Entities have tags
// on them that dictate their bounds, ensures that
// data we are receiving is within them.

builder.Services.AddScoped<UserRetrievalFilter>();
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.AddService<UserRetrievalFilter>();
    options.Filters.Add<ModelStateValidationFilter>();
});

// ** User Authentication **
//
// 1. Determine the standards we'll be signing the JSON
// web tokens with.
// 2. Setting up default authentication challenges so
// the controllers know to be challenging the tokens.

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["auth:private"])),
        ValidateIssuer = false,
        ValidateAudience = false,
    };
});

// ** User Authorisation **
//
// 1. Add an authorisation policy named 'BearerPolicy'
// (this is set up on the controller if needed)

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("BearerPolicy", policy =>
    {
        policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
        policy.RequireAuthenticatedUser();
    });
});

// ** Application Settings **
//

var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
