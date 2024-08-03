using Grpc.Net.Client;
using OrderService.proto;
using OrderService.Protos;
using OrderWorker.Models;
using PaymentService.Data;
using ProtoItem = OrderService.proto.Item;
namespace OrderWorker.Services
{
    public class OrderServices(AppDbContext dbContext,ILogger<OrderServices> logger)
    {
        public async Task<Order> CreateOrderWithItems(Order order,CancellationToken cancellationToken)
        {   
            foreach(var orderitem in order.OrderItems)
            {
                var item = await dbContext.Items.FindAsync(orderitem.ItemId);
                if (item == null)
                {
                    throw new Exception($"Item with {orderitem.ItemId} Not exists.");
                }
                orderitem.Item = item;
            }

            //InventoryService
            using var channel1 = GrpcChannel.ForAddress("https://localhost:7087");
            var client1 = new OrderInventoryService.OrderInventoryServiceClient(channel1);
            var ItemQuantityRequest = new ItemQuantity ();
            ItemQuantityRequest.Items.AddRange(order.OrderItems.Select(oi => new ProtoItem {ItemId = oi.ItemId,Quantity = oi.Quantity}));
            var QuantityResponse = await client1.sendInventoryMessageAsync(ItemQuantityRequest);
            if(!QuantityResponse.Success)
            {
                logger.LogWarning("Ordering failed due to insufficient Stock");
                throw new Exception("Quntity IsNot Available");
            }

            // Payment Service
            using var channel2 = GrpcChannel.ForAddress("https://localhost:7230");
            var client2 = new OrderPaymentService.OrderPaymentServiceClient(channel2);
            var UserBalanceRequest = new UserBalance
            {
                UserId = order.UserId,
                TotalPrice = order.TotalPrice,
            };
            var PaymentResponse = await client2.sendPaymentMessageAsync(UserBalanceRequest,cancellationToken:cancellationToken); 

            if(PaymentResponse.Success)
            {
            
            await dbContext.Orders.AddAsync(order);
            await dbContext.SaveChangesAsync(cancellationToken);
            logger.LogInformation($"Order Created Successfully {order.Id}");
            }
            else
            {
                logger.LogWarning("Payment failed due to insufficient balance.");
                throw new Exception("Insufficient balance to place the order.");
            }
            return order;
        }
    }
}