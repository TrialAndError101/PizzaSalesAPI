
using PizzaSalesAPI.Models;
using PizzaSalesAPI.Services.Interfaces;
using System.Runtime.Caching;


namespace PizzaSalesAPI.Services
{
    public class CSVService : ICSCService
    {
        MemoryCache cache = MemoryCache.Default;
        string CacheKey = "SalesMemoryKey";
        public CSVService() { }

        public async Task<SalesEntity> AddNewDataToSales(SalesEntity saleData)
        {
            try
            {
                // Check if cache already contains the key
                if (hasCacheData())
                {
                    // Get data from cache
                    List<SalesEntity> sales = (List<SalesEntity>)cache.Get(CacheKey);

                    int maxId = sales.Max(x => x.order_details_id);

                    //increment id
                    saleData.order_details_id = maxId + 1;

                    sales.Add(saleData);

                    // Add data to cache
                    addDataIfCacheHasData(sales);
                }
                else
                {
                    //do something here if you want to updadte something instead
                    Console.WriteLine("No cache found, upload sales data first");
                    return new SalesEntity();
                }
                return saleData;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in adding data: {ex.Message} {ex.StackTrace}");
                return new SalesEntity();
            }
        }

        public async Task<List<SalesEntity>> readCacheSales()
        {
            try
            {
                if (hasCacheData())
                {
                    List<SalesEntity> sales = (List<SalesEntity>)cache.Get(CacheKey);
                    return sales;
                }
                else
                {
                    Console.WriteLine("No data found");
                    return new List<SalesEntity>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in uploading data: {ex.Message} {ex.StackTrace}");
                return new List<SalesEntity>();
            }
        }

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

        public async Task<bool> UploadDataToSalesMemory(string csvPath)
        {
            try
            {
                List<SalesEntity> result = await readCSVSales(csvPath);

                // Check if cache already contains the key
                if (hasCacheData())
                {
                    // Add data to cache
                    Console.WriteLine($"Adding new memory cache");
                    cache.Add(CacheKey, result, DateTimeOffset.Now.AddMinutes(10)); // Expires after 10 minutes
                }
                else
                {
                    // Add data to cache
                    //do something here if you want to updadte something instead
                    //i will just reupload or replace existing
                    Console.WriteLine($"Cache exists, replacing data");
                    cache.Add(CacheKey, result, DateTimeOffset.Now.AddMinutes(10)); // Expires after 10 minutes
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in uploading data: {ex.Message} {ex.StackTrace}");
                return false;
            }
        }

        private bool hasCacheData()
        {            
            return cache.Contains(CacheKey);
        }

        private bool addDataIfCacheHasData(List<SalesEntity> result)
        {
            try
            {
                if (hasCacheData())
                {
                    Console.WriteLine($"Adding data to cache");
                    return cache.Add(CacheKey, result, DateTimeOffset.Now.AddMinutes(10)); // Expires after 10 minutes
                }
                else
                {
                    Console.WriteLine($"Adding data to cache failed.");
                }
                Console.WriteLine($"Adding data to cache success");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in adding data: {ex.Message} {ex.StackTrace}");
                return false;
            }
        }

        public async Task<SalesEntity> updateCacheSalesById(int id,SalesEntity saleData)
        {
            //update data row of sale by order_details_id
            try
            {
                if(hasCacheData())
                {
                    List<SalesEntity> sales = (List<SalesEntity>)cache.Get(CacheKey);
                    SalesEntity? data = sales.Where(x => x.order_details_id == id).FirstOrDefault();
                    if(data != null)
                    {
                        data.order_id = saleData.order_id;
                        data.pizza_id = saleData.pizza_id;
                        data.quantity = saleData.quantity;

                        addDataIfCacheHasData(sales);
                    }
                    return data;
                }
                return new SalesEntity();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in updating data: {ex.Message} {ex.StackTrace}");
                return new SalesEntity();
            }
        }


        public async Task<bool> deleteCacheSalesById(int id)
        {
            //delete data row of sale by order_details_id
            try
            {
                if (hasCacheData())
                {
                    List<SalesEntity> sales = (List<SalesEntity>)cache.Get(CacheKey);
                    SalesEntity? data = sales.Where(x => x.order_details_id == id).FirstOrDefault();
                    if (data != null)
                    {
                        sales.Remove(data);

                        addDataIfCacheHasData(sales);
                    }
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in deleting data: {ex.Message} {ex.StackTrace}");
                return false;
            }
        }
    }
}
