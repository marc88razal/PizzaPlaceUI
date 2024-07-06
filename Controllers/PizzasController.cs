using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Session;
using Newtonsoft.Json;
using PizzaPlaceUI.Models;
using System.Globalization;
using System.Net.Http.Headers;
using System.Text;
using CsvHelper;
using static PizzaPlaceUI.Models.OrderDetailsModel;

namespace PizzaPlaceUI.Controllers
{
    public class PizzasController : Controller
    {
        // GET: PizzasController
        private readonly IConfiguration _config;
        public PizzasController(IConfiguration config)
        {
            _config = config;
        }

        public async Task<ActionResult> Pizzas()
        {
            string message = null;
            string apiUrl = _config["api_config:api_url"];
            List<PizzasModel.PizzasDisplay> pizzas = new List<PizzasModel.PizzasDisplay>();
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync(apiUrl + "/api/Pizzas").Result;
                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    pizzas = JsonConvert.DeserializeObject<List<PizzasModel.PizzasDisplay>>(data);
                    ViewData["Pizzas"] = pizzas.Take(10).ToList();
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
                    List<PizzasModel.Pizzas> pizzas = new List<PizzasModel.Pizzas>();
                    using (var reader = new StreamReader(file.OpenReadStream()))
                    using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                    {
                        var records = csv.GetRecords<PizzasModel.Csv>();
                        var mapped = records.Select(r => new PizzasModel.Pizzas(r)).ToList();
                        pizzas = mapped;
                    }
                    string jsonString = JsonConvert.SerializeObject(pizzas);

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
            return View("Pizzas");
        }
    }
}
