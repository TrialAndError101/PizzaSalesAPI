using Microsoft.AspNetCore.OData;
using PizzaSalesAPI.Services;
using PizzaSalesAPI.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddScoped<ICSCService, CSVService>();
builder.Services.AddControllers().AddOData(options => options//add OData to the services to allow for querying
.Select()//allows to select "  https://localhost:7029/api/Sales?$Select=order_details_id"
.Filter()//alklows to filter "  https://localhost:7029/api/Sales?$filter=order_details_id gt 10"
.OrderBy());//allows to order by " https://localhost:7029/api/Sales?$orderby=order_details_id desc"
//.SetMaxTop(1000));


// Add services to the container.

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
