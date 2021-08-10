using System;
using System.Collections.Generic;

namespace ExampleCom.EventHub.POCOs
{
    public class Order
    {
        public int OrderId { get; set; }
        public List<Product> Products { get; set; }
        public int CustomerId { get; set; }
    }
}
