namespace Assignment_4.Models
{
    internal class Branch
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }

        public int ManagerId { get; set; }       // FK
        public Manager Manager { get; set; }       // Navigation Property

        public ICollection<Account> Accounts { get; set; } = new HashSet<Account>();     // Navigation Property
    }
}