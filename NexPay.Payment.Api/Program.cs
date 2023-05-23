using NexPay.Payment.Api.Model;
using NexPay.Payment.Api.Repository;
using NexPay.Payment.Api.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var emailConfig = builder.Configuration
        .GetSection("EmailConfiguration")
        .Get<EmailConfiguration>();

builder.Services.AddSingleton(emailConfig);
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped<IContractService, ContractService>();
builder.Services.AddScoped<IContractRepository, ContractRepository>();
builder.Services.AddScoped<ILoginApiProxyService, LoginApiProxyService>();
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
