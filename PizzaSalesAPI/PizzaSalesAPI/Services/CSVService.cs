using PizzaSalesAPI.Models;
using PizzaSalesAPI.Services.Interfaces;

namespace PizzaSalesAPI.Services
{
    public class CSVService : ICSCService
    {
        public CSVService() { }

        public async Task<List<SalesEntity>> readCSVSales(string csvPath)
        {
            List<SalesEntity> salesModels = new List<SalesEntity>();
            using (var reader = new StreamReader(csvPath))
            {

                while (!reader.EndOfStream)
                {
                    string? line = reader.ReadLine();
                    var values = line.Split(',');

                    if (values[0].ToLower() == "order_details_id")
                    {
                        continue;
                    }
                    SalesEntity salesModel = new SalesEntity()
                    {
                        order_details_id = Convert.ToInt32(values[0]),
                        order_id = Convert.ToInt32(values[1]),
                        pizza_id = values[2],
                        quantity = Convert.ToInt32(values[3])
                    };
                    salesModels.Add(salesModel);
                }
            }

            return salesModels;
        }
    }
}
