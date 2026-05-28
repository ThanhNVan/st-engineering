using Microsoft.OpenApi;
using Napoleon.Fos.Application.Extensions;
using Napoleon.Fos.Infrastructure.Extensions;
using Napoleon.Fos.Presentation.WebApi.Definitions;
using Napoleon.Fos.Presentation.WebApi.ServiceExtension;
using Napoleon.Shared.Contract.Interceptor;
using Napoleon.Shared.Contract.ServiceExtension;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddAutoValidation();
builder.Services.AddExceptionHandlerDefinition();
builder.Services.AddCorsPolicies();
builder.Services.AddControllers(options =>
{
    options.AddApiFormatter();
}).ConfigureApiBehaviorOptions(option =>
{
    option.InvalidModelStateResponseFactory = ValidateFailedResponseFormatter.MakeValidationResponse;
});
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthenticationPolicies(builder.Configuration);
builder.Services.AddSwaggerDefinition();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger(); // c => { c.OpenApiVersion = OpenApiSpecVersion.OpenApi2_0; }
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseExceptionHandler();
app.UseMiddleware<CustomAuthenticationMiddleware>();

await app.RunAsync();

public abstract partial class Program { }

