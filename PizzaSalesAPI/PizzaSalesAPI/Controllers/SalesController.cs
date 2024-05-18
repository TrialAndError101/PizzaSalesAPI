using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.OData.ModelBuilder;
using PizzaSalesAPI.Models;
using PizzaSalesAPI.Services.Interfaces;

namespace PizzaSalesAPI.Controllers
{

    [Route("odata/[controller]")]
    //[ApiController] //deleted to try odata route
    public class SalesController : ODataController
    {
        private readonly ICSCService _cSCVService;

        public SalesController(ICSCService cSCVService)
        {
            _cSCVService = cSCVService;
        }


        //incase you can't run this in swagger, please use postman
        // https://localhost:<port_here>/api/Patient

        //for checking only of the csv content
        [HttpGet]
        [EnableQuery(PageSize = 1000)]
        public async Task<ActionResult<SalesEntity>> CheckCSVFile([FromBody] RequestBodyModel requestBody)
        {
            Console.WriteLine("C# HTTP trigger GetAllPatient processed a request.");

            try
            {
                List<SalesEntity> result = await _cSCVService.readCSVSales(requestBody.CSVPath);

                if (result != null)
                {
                    Console.WriteLine("Process complete.");
                    return new OkObjectResult(result);
                }
                else
                {
                    Console.WriteLine("No data found.");
                    return new BadRequestObjectResult("No data found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Something went wrong: {ex.Message} {ex.StackTrace}");
                return new BadRequestObjectResult("Something went wrong.");
                throw;
            }
        }

        //for uploading to csv to cache
        [HttpPost]
        public async Task<ActionResult<SalesEntity>> UploadSales([FromBody] RequestBodyModel requestBody)
        {
            Console.WriteLine("C# HTTP trigger UploadSales processed a request.");

            try
            {
                //List<SalesEntity> result = await _cSCVService.readCSVSales(requestBody.CSVPath);

                var result = await _cSCVService.UploadDataToSalesMemory(requestBody.CSVPath);

                if (result)
                {
                    Console.WriteLine("Process complete.");
                    return new OkObjectResult("upload complete.");
                }
                else
                {
                    Console.WriteLine("Upload failed");
                    return new BadRequestObjectResult("Upload failed");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Something went wrong: {ex.Message} {ex.StackTrace}");
                return new BadRequestObjectResult("Something went wrong.");
                throw;
            }
        }


        //for adding new data to sales cache
        [Route("cache")]
        [HttpPost]
        public async Task<ActionResult<SalesEntity>> InsertSale([FromBody] SalesEntity requestBody)
        {
            Console.WriteLine("C# HTTP trigger UploadSales processed a request.");

            try
            {
                //List<SalesEntity> result = await _cSCVService.readCSVSales(requestBody.CSVPath);

                SalesEntity result = await _cSCVService.AddNewDataToSales(requestBody);

                if (result != null)
                {
                    Console.WriteLine("Process complete.");
                    return new OkObjectResult(result);
                }
                else
                {
                    Console.WriteLine("Upload failed");
                    return new BadRequestObjectResult("Upload failed");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Something went wrong: {ex.Message} {ex.StackTrace}");
                return new BadRequestObjectResult("Something went wrong.");
                throw;
            }
        }

        //for getting data from cache
        [Route("cache")]
        [HttpGet]
        [EnableQuery(PageSize = 1000)]
        public async Task<ActionResult<List<SalesEntity>>> GetDataFromCache()
        {
            Console.WriteLine("C# HTTP trigger UploadSales processed a request.");

            try
            {
                //List<SalesEntity> result = await _cSCVService.readCSVSales(requestBody.CSVPath);

                List<SalesEntity> result = await _cSCVService.readCacheSales();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Something went wrong: {ex.Message} {ex.StackTrace}");
                return new BadRequestObjectResult("Something went wrong.");
                throw;
            }
        }


        //for updating data from cache
        [Route("cache/{id}")]
        [HttpPut]
        public async Task<ActionResult<SalesEntity>> GetDataFromCache(int id, [FromBody]SalesEntity requestBody)
        {
            Console.WriteLine("C# HTTP trigger UploadSales processed a request.");

            try
            {
                //List<SalesEntity> result = await _cSCVService.readCSVSales(requestBody.CSVPath);

                SalesEntity result = await _cSCVService.updateCacheSalesById(id, requestBody);

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Something went wrong: {ex.Message} {ex.StackTrace}");
                return new BadRequestObjectResult("Something went wrong.");
                throw;
            }
        }

        [Route("cache/{id}")]
        [HttpDelete]
        public async Task<ActionResult<bool>> GetDataFromCache(int id)
        {
            Console.WriteLine("C# HTTP trigger UploadSales processed a request.");

            try
            {
                return await _cSCVService.deleteCacheSalesById(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Something went wrong: {ex.Message} {ex.StackTrace}");
                return new BadRequestObjectResult("Something went wrong.");
                throw;
            }
        }
    }
}
