using Grpc.Core;
using InventoryService.Data;
using OrderService.proto;

namespace InventoryService.Services
{
    public class OrderInventoryServices(AppDbContext dbContext,ILogger<OrderInventoryServices> logger):OrderInventoryService.OrderInventoryServiceBase
    {
        public override Task<IsAvailable> sendInventoryMessage(ItemQuantity request, ServerCallContext context)
        {
            foreach (var item in request.Items)
            {
                var itemFound = dbContext.Items.Find(item.ItemId);
                if(itemFound == null)
                {
                    logger.LogWarning($"Not Found Item with Id {item.ItemId}");
                    return Task.FromResult(new IsAvailable {Success = false} );
                }
                else if(itemFound.Quantity < item.Quantity)
                {
                    logger.LogWarning($"{item.Quantity} Quantity Is Not Available Try Buy less than {itemFound.Quantity}");
                    return Task.FromResult(new IsAvailable {Success = false} );
                }
                itemFound.Quantity -= item.Quantity;
                dbContext.SaveChanges();
            }
            logger.LogInformation($"Order Quantity Is Available Please Check The balance");
            return Task.FromResult(new IsAvailable {Success = true});
        }

    }
}