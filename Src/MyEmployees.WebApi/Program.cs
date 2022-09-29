using Amazon.DynamoDBv2;
using MyEmployees.Application.Repositories;
using MyEmployees.Application.Services;
using MyEmployees.Infrastructure.Persistence;
using MyEmployees.Infrastructure.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
if (builder.Environment.IsDevelopment())
{
    var amazonDynamoDBConfig = new AmazonDynamoDBConfig
    {
        ServiceURL = "http://localhost:8000"
    };

    builder.Services.AddSingleton<IAmazonDynamoDB>(_ => new AmazonDynamoDBClient(amazonDynamoDBConfig));
}
else
{
    builder.Services.AddSingleton<IAmazonDynamoDB>(_ => new AmazonDynamoDBClient());
}

builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection(DatabaseSettings.KeyName));

builder.Services.AddSingleton<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddSingleton<IEmployeeService, EmployeeService>();

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