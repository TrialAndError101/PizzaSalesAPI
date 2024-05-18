using PizzaSalesAPI.Models;
using PizzaSalesAPI.Services.Interfaces;

namespace PizzaSalesAPI.Services
{
    public class CSVService : ICSCService
    {
        public CSVService() { }

        public async Task<List<SalesModel>> readCSVSales(string csvPath)
        {
            List<SalesModel> salesModels = new List<SalesModel>();
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
                    SalesModel salesModel = new SalesModel()
                    {
                        order_details_id = Convert.ToInt32(values[0]),
                        order_id = Convert.ToInt32(values[0]),
                        pizza_id = values[0],
                        quantity = Convert.ToInt32(values[0])
                    };
                    salesModels.Add(salesModel);
                }
            }

            return salesModels;
        }
    }
}
