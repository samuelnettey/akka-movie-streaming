namespace MovieStreaming.PaymentProcessor.Messages
{
    internal class PaymentSentMessage
    {
        public int AccountNumber { get; private set; }
        public string ReceiptReference { get; private set; }

        public PaymentSentMessage(int accountNumber, string receiptReference)
        {
            AccountNumber = accountNumber;
            ReceiptReference = receiptReference;
        }
    }
}