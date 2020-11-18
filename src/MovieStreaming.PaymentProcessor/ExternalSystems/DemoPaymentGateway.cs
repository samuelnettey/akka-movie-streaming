using System;
using System.Threading.Tasks;

namespace MovieStreaming.PaymentProcessor.ExternalSystems
{
    class DemoPaymentGateway : IPaymentGateway
    {
        public async Task<PaymentReceipt> Pay(int accountNumber, decimal amount)
        {
            return await Task.Delay(2000)
                .ContinueWith<PaymentReceipt>(
                    task =>
                    {
                        return new PaymentReceipt()
                        {
                            AccountNumber = accountNumber,
                            PaymentConfirmationReceipt = Guid.NewGuid().ToString()
                        };
                    });
        }
    }
}