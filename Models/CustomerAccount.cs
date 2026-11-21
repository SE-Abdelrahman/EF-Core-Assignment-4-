using Assignment_4.Enums;

namespace Assignment_4.Models
{
    internal class CustomerAccount
    {
        public int CustomerId { get; set; }    // FK
        public Customer Customer { get; set; }        // Navigation Property

        public string AccountNumber { get; set; }    // FK
        public Account Account { get; set; }        // Navigation Property

        public OwnershipType OwnershipType { get; set; }
        public DateTime OwnershipStartDate { get; set; }
        public AccountStatus AccountStatus { get; set; }
    }
}
