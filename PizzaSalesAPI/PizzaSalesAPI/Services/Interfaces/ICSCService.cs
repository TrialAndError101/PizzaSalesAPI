using PizzaSalesAPI.Models;

namespace PizzaSalesAPI.Services.Interfaces
{
    public interface ICSCService
    {
        Task<List<SalesEntity>> readCSVSales(string csvPath);
    }
}
