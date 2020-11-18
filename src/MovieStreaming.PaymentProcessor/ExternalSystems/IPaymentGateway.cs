using System.Threading.Tasks;

namespace MovieStreaming.PaymentProcessor.ExternalSystems
{
    interface IPaymentGateway
    {
        Task<PaymentReceipt> Pay(int accountNumber, decimal amount);
    }
}