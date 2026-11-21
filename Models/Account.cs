using Assignment_4.Enums;

namespace Assignment_4.Models
{
    internal class Account
    {
        public string AccountNumber { get; set; }
        public decimal CurrentBalance { get; set; }
        public AccountType AccountType { get; set; }
        public DateTime OpeningDate { get; set; }

        public string BranchCode { get; set; }         // FK
        public Branch Branch { get; set; }        // Navigation Property

        public ICollection<CustomerAccount> CustomerAccounts { get; set; } = new HashSet<CustomerAccount>();         // Navigation Property
        public ICollection<Transaction> Transactions { get; set; } = new HashSet<Transaction>();              // Navigation Property
    }
}