using Microsoft.AspNetCore.Http.Features;

namespace OrderWorker.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>(); 
        public int TotalPrice { 
            get
            {
                return OrderItems.Sum(o=>o.Item.Price * o.Quantity);
            } 
        }
    }
}