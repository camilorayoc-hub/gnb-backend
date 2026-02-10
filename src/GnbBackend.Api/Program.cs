using GnbBackend.Api.Infrastructure;
using GnbBackend.Core.Interfaces;
using GnbBackend.Core.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IDataRepository, JsonDataRepository>();

builder.Services.AddSingleton<ICurrencyConverter>(sp =>
{
    var repo = sp.GetRequiredService<IDataRepository>();
    var logger = sp.GetRequiredService<ILogger<CurrencyConverter>>();
    var rates = repo.GetAllExchangeRatesAsync().GetAwaiter().GetResult();
    return new CurrencyConverter(rates, logger);
});

builder.Services.AddScoped<ITransactionService, TransactionService>();

var app = builder.Build();

if (app.Environment.IsDevelopment()){
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();