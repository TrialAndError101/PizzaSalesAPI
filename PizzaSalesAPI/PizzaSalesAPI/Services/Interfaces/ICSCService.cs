using PizzaSalesAPI.Models;

namespace PizzaSalesAPI.Services.Interfaces
{
    public interface ICSCService
    {
        Task<List<SalesModel>> readCSVSales(string csvPath);
    }
}
