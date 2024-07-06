namespace PizzaPlaceUI.Models
{
    public class PizzasModel
    {
        public class Csv
        {
            public string pizza_id { get; set; }
            public string pizza_type_id { get; set; }

            public string size { get; set; }
            public decimal? price { get; set; }
        }

        public class Pizzas
        {
            public string pizza_id { get; set; }
            public string pizza_type_id { get; set; }

            public string size { get; set; }
            public decimal? price { get; set; }

            public Pizzas(Csv o)
            {
                pizza_id = o.pizza_id;
                pizza_type_id = o.pizza_type_id;
                size = o.size;
                price = o.price;
            }
        }

        public class PizzasDisplay
        {
            public string pizza_id { get; set; }
            public string pizza_type_id { get; set; }

            public string size { get; set; }
            public decimal? price { get; set; }
        }
    }
}
