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

        [HttpGet]
        [EnableQuery(PageSize = 1000)]
        public async Task<ActionResult<SalesEntity>> GetAllPatient([FromBody] RequestBodyModel requestBody)
        {
            Console.WriteLine("C# HTTP trigger AddPatients processed a request.");

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
    }
}
