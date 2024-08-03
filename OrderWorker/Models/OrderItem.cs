using OrderService.proto;

namespace OrderWorker.Models
{
    public class OrderItem
{
    public int OrderId { get; set; }
    public Order Order { get; set; } = new Order();

    public int ItemId { get; set; }
    public Item Item { get; set; } = new Item();

    public int Quantity { get; set; }
}
}