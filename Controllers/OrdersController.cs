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
    public class OrdersController : Controller
    {
        private readonly IConfiguration _config;
        public OrdersController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            string message = null;
            string apiUrl = _config["api_config:api_url"];
            List<OrdersModel.OrdersDisplay> orders = new List<OrdersModel.OrdersDisplay>();
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync(apiUrl + "/api/Orders");
                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    orders = JsonConvert.DeserializeObject<List<OrdersModel.OrdersDisplay>>(data);
                    return View(orders.Take(10));
                }
                else
                {
                    message = response.Content.ReadAsStringAsync().Result.ToString();
                }
            }
            return View(message);
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
                    List<OrdersModel.Orders> orders = new List<OrdersModel.Orders>();
                    using (var reader = new StreamReader(file.OpenReadStream()))
                    using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                    {
                        var records = csv.GetRecords<OrdersModel.Csv>();
                        var mapped = records.Select(r => new OrdersModel.Orders(r)).ToList();
                        orders = mapped;
                    }
                    string jsonString = JsonConvert.SerializeObject(orders);

                    using (HttpClient client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        HttpContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                        HttpResponseMessage response = await client.PostAsync(apiUrl + "/api/" + fileDetails.FileName + "/Upload", content);
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
