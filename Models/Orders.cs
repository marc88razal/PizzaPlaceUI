namespace PizzaPlaceUI.Models
{
    public class OrdersModel
    {
        public class Csv
        {
            public int order_id { get; set; }

            public DateOnly date { get; set; }

            public TimeOnly time { get; set; }
        }

        public class Orders
        {
            public int order_id { get; set; }

            public DateOnly date { get; set; }

            public TimeOnly time { get; set; }

            public Orders(Csv o)
            {
                order_id = o.order_id;
                date = o.date;
                time = o.time;
            }
        }

        public class OrdersDisplay
        {
            public int order_id { get; set; }

            public DateOnly date { get; set; }

            public TimeOnly time { get; set; }
        }
    }
}
