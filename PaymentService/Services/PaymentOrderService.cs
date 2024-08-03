using Grpc.Core;
using OrderService.Protos;
using PaymentService.Data;
using PaymentService.Models;

namespace PaymentService.Services
{
    public class PaymentOrderService(AppDbContext dbContext,ILogger<PaymentOrderService> logger):OrderPaymentService.OrderPaymentServiceBase
    {
        
        public override  Task<SufficentBalance> sendPaymentMessage(UserBalance request, ServerCallContext context)
        {
            var user = dbContext.users.Find(request.UserId);
            if (user == null || request.TotalPrice > user.Balance)
            {
                logger.LogInformation("User Is NotFound Or Balance Is Not Sufficent");
                return Task.FromResult(new SufficentBalance{Success = false});
            }
            else{
                var balance = user.Balance -request.TotalPrice;
                user.Balance = balance;
                dbContext.SaveChanges();
                logger.LogInformation($"User Balance Now is {balance}");
                return Task.FromResult(new SufficentBalance{Success = true});
            }
            
        }
    }
}