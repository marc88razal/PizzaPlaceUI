using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Session;
using Newtonsoft.Json;
using PizzaPlaceUI.Models;
using System.Globalization;
using System.Net.Http.Headers;
using System.Text;
using static PizzaPlaceUI.Models.OrderDetailsModel;
using CsvHelper;

namespace PizzaPlaceUI.Controllers
{
    public class PizzaTypesController : Controller
    {
        // GET: PizzaTypesController
        private readonly IConfiguration _config;
        public PizzaTypesController(IConfiguration config)
        {
            _config = config;
        }

        public async Task<ActionResult> PizzaTypes()
        {
            string message = null;
            string apiUrl = _config["api_config:api_url"];
            List<PizzaTypesModel.PizzaTypesDisplay> pizzaTypes = new List<PizzaTypesModel.PizzaTypesDisplay>();
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync(apiUrl + "/api/PizzaTypes").Result;
                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    pizzaTypes = JsonConvert.DeserializeObject<List<PizzaTypesModel.PizzaTypesDisplay>>(data);
                    ViewData["PizzaTypes"] = pizzaTypes.Take(10).ToList();
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
                    List<PizzaTypesModel.PizzaTypes> pizzaTypes = new List<PizzaTypesModel.PizzaTypes>();
                    using (var reader = new StreamReader(file.OpenReadStream()))
                    using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                    {
                        var records = csv.GetRecords<PizzaTypesModel.Csv>();
                        var mapped = records.Select(r => new PizzaTypesModel.PizzaTypes(r)).ToList();
                        pizzaTypes = mapped;
                    }
                    string jsonString = JsonConvert.SerializeObject(pizzaTypes);

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
            return View("PizzaTypes");
        }
    }
}
