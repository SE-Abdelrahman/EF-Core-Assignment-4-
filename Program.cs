using Assignment_4.Context;
using Assignment_4.Enums;
using Assignment_4.Models;
using Microsoft.EntityFrameworkCore;

namespace Assignment_4
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            using AppDbContext dbContext = new AppDbContext();

            await dbContext.Database.MigrateAsync();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("========================================");
                Console.WriteLine("       National Bank Management         ");
                Console.WriteLine("========================================");
                Console.WriteLine("1) Add a new Customer");
                Console.WriteLine("2) Open a new Account for a Customer");
                Console.WriteLine("3) Update Account Status (Active / Closed)");
                Console.WriteLine("4) Remove an Account from a Customer");
                Console.WriteLine("5) List all Customers (with accounts)");
                Console.WriteLine("0) Exit");
                Console.WriteLine("========================================");
                Console.Write("Enter choice: ");

                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":

                        #region 1. Add a new Customer

                        Console.WriteLine("--- Add New Customer ---");

                        long tempNumber;

                        Console.Write("Full Name: ");
                        string fullName = Console.ReadLine();
                        while (string.IsNullOrWhiteSpace(fullName) || fullName.Length < 3 || long.TryParse(fullName, out tempNumber))
                        {
                            Console.Write("Invalid input. Full Name must be at least 3 letters (not numbers): ");
                            fullName = Console.ReadLine();
                        }

                        Console.Write("National ID (14 digits): ");
                        string nationalId = Console.ReadLine();
                        while (string.IsNullOrWhiteSpace(nationalId) || nationalId.Length != 14 || !long.TryParse(nationalId, out tempNumber) || tempNumber <= 0)
                        {
                            Console.Write("Invalid National ID (must be 14 positive digits). Try again: ");
                            nationalId = Console.ReadLine();
                        }

                        bool nationalIdExists = await dbContext.Customers.AnyAsync(c => c.NationalId == nationalId);
                        if (nationalIdExists)
                        {
                            Console.WriteLine("Error: A customer with this National ID already exists.");
                            break;
                        }

                        Console.Write("Date of Birth (yyyy-MM-dd): ");
                        DateTime dob;
                        while (!DateTime.TryParse(Console.ReadLine(), out dob) || dob > DateTime.Now.AddYears(-18) || dob < DateTime.Now.AddYears(-120))
                        {
                            Console.Write("Invalid date. Age must be between 18 and 120 years (yyyy-MM-dd): ");
                        }

                        Console.Write("Email: ");
                        string email = Console.ReadLine();
                        while (string.IsNullOrWhiteSpace(email) || email.Length < 5 || !email.Contains("@") || !email.Contains(".") || email.StartsWith("@"))
                        {
                            Console.Write("Invalid email format. Enter Email again: ");
                            email = Console.ReadLine();
                        }

                        Console.Write("Phone (11 digits): ");
                        string phone = Console.ReadLine();
                        while (string.IsNullOrWhiteSpace(phone) || phone.Length != 11 || !phone.StartsWith("01") || !long.TryParse(phone, out tempNumber))
                        {
                            Console.Write("Invalid phone (must be 11 digits starting with 01). Try again: ");
                            phone = Console.ReadLine();
                        }

                        Console.Write("Address: ");
                        string address = Console.ReadLine();
                        while (string.IsNullOrWhiteSpace(address) || address.Length < 5 || long.TryParse(address, out tempNumber))
                        {
                            Console.Write("Invalid address (must be at least 5 characters and not numbers only). Try again: ");
                            address = Console.ReadLine();
                        }

                        Console.WriteLine("Customer Type:");
                        Console.WriteLine("1) Individual");
                        Console.WriteLine("2) Business");
                        Console.Write("Choice: ");
                        int typeChoice;
                        while (!int.TryParse(Console.ReadLine(), out typeChoice) || (typeChoice != 1 && typeChoice != 2))
                        {
                            Console.Write("Invalid choice. Enter 1 or 2: ");
                        }
                        CustomerType customerType = (CustomerType)typeChoice;

                        Customer newCustomer = new Customer
                        {
                            FullName = fullName.Trim(),
                            NationalId = nationalId,
                            DateOfBirth = dob,
                            Email = email.Trim(),
                            PhoneNumber = phone,
                            Address = address.Trim(),
                            CustomerType = customerType
                        };

                        await dbContext.Customers.AddAsync(newCustomer);
                        await dbContext.SaveChangesAsync();

                        Console.WriteLine($"\nCustomer created successfully. CustomerId = {newCustomer.Id}");

                        #endregion 1. Add a new Customer

                        break;

                    case "2":

                        #region 2. Open a new Account for a Customer

                        Console.WriteLine("--- Open New Account ---");

                        Console.Write("Account Number (4-20 chars): ");
                        string accountNumber = Console.ReadLine();
                        while (string.IsNullOrWhiteSpace(accountNumber) || accountNumber.Trim().Length < 4 || accountNumber.Trim().Length > 20)
                        {
                            Console.Write("Invalid Account Number (must be 4 to 20 characters). Try again: ");
                            accountNumber = Console.ReadLine();
                        }
                        accountNumber = accountNumber.Trim();

                        Console.WriteLine("Account Type:");
                        Console.WriteLine("1) Savings");
                        Console.WriteLine("2) Current");
                        Console.WriteLine("3) Business");
                        Console.Write("Choice: ");
                        int accTypeChoice;
                        while (!int.TryParse(Console.ReadLine(), out accTypeChoice) || accTypeChoice < 1 || accTypeChoice > 3)
                        {
                            Console.Write("Invalid choice. Enter 1, 2, or 3: ");
                        }
                        AccountType accountType = (AccountType)accTypeChoice;

                        Console.Write("Branch Code: ");
                        string branchCode = Console.ReadLine();
                        while (string.IsNullOrWhiteSpace(branchCode))
                        {
                            Console.Write("Branch code cannot be empty. Try again: ");
                            branchCode = Console.ReadLine();
                        }
                        branchCode = branchCode.Trim().ToUpper();

                        Console.Write("Customer Id: ");
                        int customerId;
                        while (!int.TryParse(Console.ReadLine(), out customerId) || customerId <= 0)
                        {
                            Console.Write("Invalid Customer Id. Enter a positive number: ");
                        }

                        Console.WriteLine("Ownership Role:");
                        Console.WriteLine("1) Primary");
                        Console.WriteLine("2) CoHolder");
                        Console.Write("Choice: ");
                        int roleChoice;
                        while (!int.TryParse(Console.ReadLine(), out roleChoice) || (roleChoice != 1 && roleChoice != 2))
                        {
                            Console.Write("Invalid choice. Enter 1 or 2: ");
                        }
                        OwnershipType ownershipRole = (OwnershipType)roleChoice;

                        Console.WriteLine($"Validating branch '{branchCode}' and customer #{customerId}...");

                        Branch branch = await dbContext.Branches.FindAsync(branchCode);
                        Customer customer = await dbContext.Customers.FindAsync(customerId);

                        if (branch == null)
                        {
                            Console.WriteLine($"Error: Branch '{branchCode}' does not exist.");
                            break;
                        }

                        if (customer == null)
                        {
                            Console.WriteLine($"Error: Customer #{customerId} does not exist.");
                            break;
                        }

                        Account existingAccount = await dbContext.Accounts.FindAsync(accountNumber);
                        if (existingAccount == null)
                        {
                            if (ownershipRole == OwnershipType.CoHolder)
                            {
                                Console.WriteLine("Error: A brand-new account must have a Primary owner first.");
                                break;
                            }

                            Account newAccount = new Account
                            {
                                AccountNumber = accountNumber,
                                AccountType = accountType,
                                BranchCode = branchCode,
                                CurrentBalance = 0,
                                OpeningDate = DateTime.Now
                            };
                            await dbContext.Accounts.AddAsync(newAccount);
                        }
                        else
                        {
                            if (existingAccount.BranchCode != branchCode || existingAccount.AccountType != accountType)
                            {
                                Console.WriteLine($"Error: Account '{accountNumber}' already exists under branch '{existingAccount.BranchCode}' as '{existingAccount.AccountType}'.");
                                break;
                            }

                            bool alreadyLinked = await dbContext.CustomerAccounts
                                .AnyAsync(ca => ca.CustomerId == customerId && ca.AccountNumber == accountNumber);

                            if (alreadyLinked)
                            {
                                Console.WriteLine($"Error: Customer #{customerId} is already linked to account '{accountNumber}'.");
                                break;
                            }

                            if (ownershipRole == OwnershipType.Primary)
                            {
                                bool hasPrimary = await dbContext.CustomerAccounts
                                    .AnyAsync(ca => ca.AccountNumber == accountNumber && ca.OwnershipType == OwnershipType.Primary);

                                if (hasPrimary)
                                {
                                    Console.WriteLine($"Error: Account '{accountNumber}' already has a Primary owner. Choose CoHolder instead.");
                                    break;
                                }
                            }
                        }

                        CustomerAccount customerAccount = new CustomerAccount
                        {
                            CustomerId = customerId,
                            AccountNumber = accountNumber,
                            OwnershipType = ownershipRole,
                            OwnershipStartDate = DateTime.Now,
                            AccountStatus = AccountStatus.Active
                        };

                        await dbContext.CustomerAccounts.AddAsync(customerAccount);
                        await dbContext.SaveChangesAsync();

                        Console.WriteLine($"Account '{accountNumber}' created and linked to customer {customerId} as {ownershipRole} owner.");

                        #endregion 2. Open a new Account for a Customer

                        break;

                    case "3":

                        #region 3. Update Account Status

                        Console.WriteLine("--- Update Account Status ---");

                        Console.Write("Account Number: ");
                        string targetAccNum = Console.ReadLine();
                        while (string.IsNullOrWhiteSpace(targetAccNum))
                        {
                            Console.Write("Account Number cannot be empty. Try again: ");
                            targetAccNum = Console.ReadLine();
                        }
                        targetAccNum = targetAccNum.Trim();

                        Console.Write("Customer Id: ");
                        int targetCustId;
                        while (!int.TryParse(Console.ReadLine(), out targetCustId) || targetCustId <= 0)
                        {
                            Console.Write("Invalid Customer Id. Enter a positive number: ");
                        }

                        CustomerAccount linkToUpdate = await dbContext.CustomerAccounts.FindAsync(targetCustId, targetAccNum);
                        if (linkToUpdate == null)
                        {
                            Console.WriteLine($"Error: No account link found for Customer #{targetCustId} and Account '{targetAccNum}'.");
                            break;
                        }

                        Console.WriteLine($"Current Status: {linkToUpdate.AccountStatus}");
                        Console.WriteLine("New Status:");
                        Console.WriteLine("1) Active");
                        Console.WriteLine("2) Closed");
                        Console.Write("Choice: ");
                        int statusChoice;
                        while (!int.TryParse(Console.ReadLine(), out statusChoice) || (statusChoice != 1 && statusChoice != 2))
                        {
                            Console.Write("Invalid choice. Enter 1 or 2: ");
                        }

                        AccountStatus selectedStatus = (AccountStatus)statusChoice;
                        if (linkToUpdate.AccountStatus == selectedStatus)
                        {
                            Console.WriteLine($"Account status is already {selectedStatus}. No changes made.");
                            break;
                        }

                        linkToUpdate.AccountStatus = selectedStatus;
                        await dbContext.SaveChangesAsync();

                        Console.WriteLine($"Status updated to {linkToUpdate.AccountStatus}.");

                        #endregion 3. Update Account Status

                        break;

                    case "4":

                        #region 4. Remove an Account from a Customer

                        Console.WriteLine("--- Remove Account From Customer ---");

                        Console.Write("Account Number: ");
                        string delAccNum = Console.ReadLine();
                        while (string.IsNullOrWhiteSpace(delAccNum))
                        {
                            Console.Write("Account Number cannot be empty. Try again: ");
                            delAccNum = Console.ReadLine();
                        }
                        delAccNum = delAccNum.Trim();

                        Console.Write("Customer Id: ");
                        int delCustId;
                        while (!int.TryParse(Console.ReadLine(), out delCustId) || delCustId <= 0)
                        {
                            Console.Write("Invalid Customer Id. Enter a positive number: ");
                        }

                        CustomerAccount linkToRemove = await dbContext.CustomerAccounts.FindAsync(delCustId, delAccNum);
                        if (linkToRemove == null)
                        {
                            Console.WriteLine($"Error: Ownership link not found for Customer #{delCustId} and Account '{delAccNum}'.");
                            break;
                        }

                        int ownersCount = await dbContext.CustomerAccounts.CountAsync(ca => ca.AccountNumber == delAccNum);
                        if (ownersCount == 1)
                        {
                            Account accountToCheck = await dbContext.Accounts.FindAsync(delAccNum);
                            if (accountToCheck != null && accountToCheck.CurrentBalance > 0)
                            {
                                Console.WriteLine($"Error: Cannot remove the last owner while account '{delAccNum}' still has a balance of {accountToCheck.CurrentBalance:N2}.");
                                break;
                            }
                        }

                        dbContext.CustomerAccounts.Remove(linkToRemove);
                        await dbContext.SaveChangesAsync();
                        Console.WriteLine("Ownership link deleted.");

                        bool hasOtherOwners = await dbContext.CustomerAccounts.AnyAsync(ca => ca.AccountNumber == delAccNum);
                        if (!hasOtherOwners)
                        {
                            Account orphanAccount = await dbContext.Accounts.FindAsync(delAccNum);
                            if (orphanAccount != null)
                            {
                                dbContext.Accounts.Remove(orphanAccount);
                                await dbContext.SaveChangesAsync();
                                Console.WriteLine($"That was the last owner - account '{delAccNum}' was also removed.");
                            }
                        }

                        #endregion 4. Remove an Account from a Customer

                        break;

                    case "5":

                        #region 5. List all Customers (Eager Loading)

                        Console.WriteLine("--- All Customers ---");

                        List<Customer> customers = await dbContext.Customers
                            .AsNoTracking()
                            .Include(c => c.CustomerAccounts)
                                .ThenInclude(ca => ca.Account)
                                    .ThenInclude(a => a.Branch)
                            .ToListAsync();

                        if (customers.Count == 0)
                        {
                            Console.WriteLine("No customers found.");
                        }
                        else
                        {
                            foreach (Customer cust in customers)
                            {
                                Console.WriteLine($"#{cust.Id} {cust.FullName} ({cust.CustomerType})");
                                if (cust.CustomerAccounts.Count == 0)
                                {
                                    Console.WriteLine("   (no accounts)");
                                }
                                else
                                {
                                    foreach (CustomerAccount ca in cust.CustomerAccounts)
                                    {
                                        Console.WriteLine($"   {ca.Account.AccountNumber}  {ca.Account.AccountType}  Balance: {ca.Account.CurrentBalance:N2}  {ca.OwnershipType}  {ca.AccountStatus}  @{ca.Account.Branch.Name}");
                                    }
                                }
                            }
                        }

                        #endregion 5. List all Customers (Eager Loading)

                        break;

                    case "0":
                        Console.WriteLine("Closing connection and exiting...");
                        return;

                    default:
                        Console.WriteLine("Invalid option. Please choose from 0 to 5.");
                        break;
                }

                Console.WriteLine("\nPress any key to return to the menu...");
                Console.ReadKey();
            }
        }
    }
}