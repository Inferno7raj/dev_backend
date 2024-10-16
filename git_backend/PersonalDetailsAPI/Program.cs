using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PersonalDetailsAPI.DAL;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddScoped<PersonalDetailsDAL>(); // Register your DAL service
builder.Services.AddSwaggerGen();

// Add CORS policy to allow specific origins or all origins.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin() // Allow requests from any origin.
              .AllowAnyMethod() // Allow any HTTP method (GET, POST, etc.).
              .AllowAnyHeader()); // Allow any headers.
});

var app = builder.Build();

// Use the error handling middleware.
app.UseMiddleware<ErrorHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Enable the CORS policy.
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
