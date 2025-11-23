namespace OrderManagementSystemApplication.Helpers
{
    public static class PaymentLogMessages
    {
        public const string PaymentProcessStarted =
            "Starting payment process. OrderId: {OrderId}, CustomerId: {CustomerId}, Amount: {Amount}";

        public const string OrderNotFound =
            "Order not found. OrderId: {OrderId}, CustomerId: {CustomerId}";

        public const string AmountMismatch =
            "Payment amount mismatch. Sent: {SentAmount}, OrderTotal: {OrderTotal}";

        public const string ExistingPaymentFound =
            "Order has existing Payment. OrderId: {OrderId}, Status: {PaymentStatus}";

        public const string RetryPayment =
            "Retrying failed payment. PaymentId: {PaymentId}, OrderId: {OrderId}";

        public const string CreatingNewPayment =
            "Creating new payment record. OrderId: {OrderId}, Method: {Method}, Amount: {Amount}";

        public const string PaymentProcessingExternal =
            "Processing online payment via gateway. PaymentId: {PaymentId}, Method: {Method}";

        public const string PaymentCompleted =
            "Payment completed successfully. PaymentId: {PaymentId}, TransactionId: {TransactionId}";

        public const string PaymentFailed =
            "Payment failed. PaymentId: {PaymentId}";

        public const string CodProcessing =
            "Processing Cash on Delivery. OrderId: {OrderId}";

        public const string TransactionCommitted =
            "Payment transaction committed successfully. OrderId: {OrderId}";

        public const string TransactionRolledBack =
            "Payment transaction rolled back. OrderId: {OrderId}";

        public const string UnexpectedError =
            "Unexpected error occurred while processing payment. OrderId: {OrderId}";
        public const string PaymentNotFound = "Payment not found. PaymentId: {PaymentId}";
        public const string PaymentRetrieved = "Payment retrieved successfully. PaymentId: {PaymentId}";
        public const string PaymentByOrderRetrieved = "Payment retrieved for order. OrderId: {OrderId}, PaymentId: {PaymentId}";

    }
}
