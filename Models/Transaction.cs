using Assignment_4.Enums;

namespace Assignment_4.Models
{
    internal class Transaction
    {
        public string TransactionNumber { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal Amount { get; set; }
        public TransactionType TransactionType { get; set; }
        public string? Note { get; set; }

        public string AccountNumber { get; set; }     // FK
        public Account Account { get; set; }      // Navigation Property
    }
}