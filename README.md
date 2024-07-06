# PizzaPlaceUI

<h1>Pizza Place UI using .NET 8, c#, CsvHelper and Newtonsoft.Json</h1>

CsvHelper - to read Csv stream from IFormFile
<br />
<br />
<b>Packages</b>
<table>
  <tr>
    <td>
      CsvHelper - 33.0.1
    </td>
  </tr>
  <tr>
    <td>
      Newtonsoft.Json - 13.0.3
    </td>
  </tr>
</table>
<br />
To install CsvHelper
In the Package Manager Console type the following:
<br />
<br />
PM> Install-Package CsvHelper
<br />
<br />
and hit 'Enter'

<br />
<br />
CSV files for download and to be used for Importation: https://www.kaggle.com/datasets/mysarahmadbhat/pizza-place-sales
<br />
<br />
In the appsettings.json, please change api_url just in case the PizzaPlaceAPI url had been changed

Please make sure that PizzaPlaceAPI project is running in order for the Imports and Data retrieval to work.

<br />
<br />
1. The CSV file names are the same as the API controller name. So changing the file name will mess up the API calls<br />
   - pizzas > api/pizzas<br />
   - orders > api/orders<br />
   - order_details > api/orderdetails (the _ is removed)<br />
   - pizza_types > api/pizzatypes (the _ is removed)
