using System.ComponentModel;

namespace PizzaPlaceUI.Models
{
    public class PizzaTypesModel
    {
        public class Csv
        {
            public string pizza_type_id { get; set; }

            public string name { get; set; }

            public string category { get; set; }

            public string ingredients { get; set; }
        }

        public class PizzaTypes
        {
            public string pizza_type_id { get; set; }

            public string name { get; set; }

            public string category { get; set; }

            public string ingredients { get; set; }

            public PizzaTypes(Csv o)
            {
                pizza_type_id = o.pizza_type_id;
                name = o.name;
                category = o.category;
                ingredients = o.ingredients;
            }
        }

        public class PizzaTypesDisplay
        {
            public string pizza_type_id { get; set; }

            public string name { get; set; }

            public string category { get; set; }

            public string ingredients { get; set; }
        }
    }
}
