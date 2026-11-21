using Assignment_4.Enums;

namespace Assignment_4.Models
{
    internal class Customer
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string NationalId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public CustomerType CustomerType { get; set; }

        public ICollection<CustomerAccount> CustomerAccounts { get; set; } = new HashSet<CustomerAccount>();         // Navigation Property
    }
}