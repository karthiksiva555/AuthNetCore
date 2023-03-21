using IdentityResourceApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<List<string>>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",
        corsPolicy => corsPolicy
            .WithOrigins(allowedOrigins.ToArray())
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
    );
});

// Add services to the container.
var identityAuthority = builder.Configuration["IdentityAuthority"];
var identityResource = builder.Configuration["IdentityResource"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.Authority = identityAuthority;
    options.Audience = identityResource;
});

builder.Services
    .AddAuthorization(options =>
    {
        options.AddPolicy(
            "read:books",
            policy => policy.Requirements.Add(
                new HasScopeRequirement("read:books", identityAuthority)
            )
        );
        
        options.AddPolicy(
            "admin:users",
            policy => policy.Requirements.Add(
                new HasScopeRequirement("admin:users", identityAuthority)
            )
        );
    });

builder.Services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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
app.UseCors("AllowSpecificOrigins");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();