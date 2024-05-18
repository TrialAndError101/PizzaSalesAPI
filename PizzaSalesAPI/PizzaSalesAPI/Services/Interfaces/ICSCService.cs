using PizzaSalesAPI.Models;

namespace PizzaSalesAPI.Services.Interfaces
{
    public interface ICSCService
    {
        Task<List<SalesEntity>> readCSVSales(string csvPath);
        Task<bool> UploadDataToSalesMemory(string csvPath);
        Task<SalesEntity> AddNewDataToSales(SalesEntity saleData);
        Task<List<SalesEntity>> readCacheSales();
        Task<SalesEntity> updateCacheSalesById(int id,SalesEntity saleData);
        Task<bool> deleteCacheSalesById(int id);

    }
}
