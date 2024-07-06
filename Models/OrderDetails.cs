using System.ComponentModel;

namespace PizzaPlaceUI.Models
{
    public class OrderDetailsModel
    {
        public class Csv
        {
            public int order_details_id { get; set; }

            public int order_id { get; set; }

            public string pizza_id { get; set; }

            public int quantity { get; set; }
        }

        public class OrderDetails
        {
            public int order_details_id { get; set; }

            public int order_id { get; set; }

            public string pizza_id { get; set; }

            public int quantity { get; set; }

            public OrderDetails(Csv o)
            {
                order_details_id = o.order_details_id;
                order_id = o.order_id;
                pizza_id = o.pizza_id;
                quantity = o.quantity;
            }
        }

        public class OrderDetailsDisplay
        {
            public int order_details_id { get; set; }

            public int order_id { get; set; }

            public string pizza_id { get; set; }

            public int quantity { get; set; }
        }
    }
}
