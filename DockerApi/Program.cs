using DockerApi.Application.Interfaces;
using DockerApi.Application.Services;
using DockerApi.Infra;
using FluentValidation;
using DockerApi.Infra.Interfaces;
using DockerApi.Infra.Repositories;
using DockerApi.Application.ViewModels;

var builder = WebApplication.CreateBuilder(args);

//Add repository to container
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();



builder.Services.AddDbContext<DockerApiDbContext>();
// Add services to the container.
builder.Services.AddTransient<ICustomerService, CustomerService>();


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

app.UseAuthorization();

app.MapControllers();

app.Run();
