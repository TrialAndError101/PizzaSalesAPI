using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Extensions;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using PizzaSalesAPI.Models;
using PizzaSalesAPI.Services;
using PizzaSalesAPI.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddScoped<ICSCService, CSVService>();
//builder.Services.AddControllers().AddOData(options => options//add OData to the services to allow for querying
//.Select()//alklows to filter "  https://localhost:7029/api/Sales?$filter=order_details_id gt 10" || //allows to order by " https://localhost:7029/api/Sales?$orderby=order_details_id desc" || //allows to select "  https://localhost:7029/api/Sales?$Select=order_details_id"
//.Filter()
//.OrderBy()
//.SetMaxTop(100)
//.AddRouteComponents("odata", GetEdmModel()));
////.SetMaxTop(1000));


builder.Services.AddControllers().AddOData(opt => opt
    .AddRouteComponents("odata", GetEdmModel())
    .Select().Expand().Filter().OrderBy().Count().SetMaxTop(100).SkipToken());



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



IEdmModel GetEdmModel()
{
    var odataBuilder = new ODataConventionModelBuilder();
    odataBuilder.EntitySet<SalesEntity>("Sales");
    return odataBuilder.GetEdmModel();
}