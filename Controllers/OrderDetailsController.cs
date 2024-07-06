using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Session;
using Newtonsoft.Json;
using PizzaPlaceUI.Models;
using System.Globalization;
using System.Net.Http.Headers;
using System.Text;
using CsvHelper;
namespace PizzaPlaceUI.Controllers
{
    public class OrderDetailsController : Controller
    {
        // GET: OrdersController
        private readonly IConfiguration _config;
        public OrderDetailsController(IConfiguration config)
        {
            _config = config;
        }

        public async Task<ActionResult> Index()
        {
            string message = null;
            string apiUrl = _config["api_config:api_url"];
            List<OrderDetailsModel.OrderDetailsDisplay> orderDetails = new List<OrderDetailsModel.OrderDetailsDisplay>();
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync(apiUrl + "/api/OrderDetails").Result;
                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    orderDetails = JsonConvert.DeserializeObject<List<OrderDetailsModel.OrderDetailsDisplay>>(data);
                    ViewData["OrderDetails"] = orderDetails.Take(10).ToList();
                }
                else
                {
                    message = response.Content.ReadAsStringAsync().Result.ToString();
                }
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            string message = null;
            string apiUrl = _config["api_config:api_url"];
            try
            {
                if (file == null)
                {
                    message = "File Not Found";
                }
                else
                {
                    FileDetails fileDetails = new FileDetails();
                    fileDetails.FileName = file.FileName.Split('.')[0].ToLower();
                    List<OrderDetailsModel.OrderDetails> orderDetails = new List<OrderDetailsModel.OrderDetails>();
                    using (var reader = new StreamReader(file.OpenReadStream()))
                    using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                    {
                        var records = csv.GetRecords<OrderDetailsModel.Csv>();
                        var mapped = records.Select(r => new OrderDetailsModel.OrderDetails(r)).ToList();
                        orderDetails = mapped;
                    }
                    string jsonString = JsonConvert.SerializeObject(orderDetails);

                    using (HttpClient client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        HttpContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                        HttpResponseMessage response = await client.PostAsync(apiUrl + "/api/" + fileDetails.FileName.Replace("_", String.Empty) + "/Upload", content);
                        if (response.IsSuccessStatusCode)
                        {
                            var data = response.Content.ReadAsStringAsync().Result;
                            message = fileDetails.FileName + " has been uploaded";
                        }
                        else
                        {
                            message = response.Content.ReadAsStringAsync().Result.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            ViewData["AppMessage"] = message;
            return View("Index");
        }
    }
}
