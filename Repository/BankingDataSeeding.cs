using Core.Entities;
using Repository.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Repository
{
    public class BankingDataSeeding
    {
        public static async Task SeedAsync(BankingSystemDbContext dbContext)
        {
            // Roles
            if (!dbContext.Roles.Any())
            {
                var roles = File.ReadAllText("../Repository/DataSeed/roles.json");
                var rolesData = JsonSerializer.Deserialize<List<Role>>(roles);
                if (rolesData?.Count > 0)
                {
                    foreach(var role in rolesData)
                    {
                        await dbContext.Roles.AddAsync(role);
                    }
                    await dbContext.SaveChangesAsync();
                }
            }

            // Users
            if (!dbContext.Users.Any())
            {
                var users = File.ReadAllText("../Repository/DataSeed/users.json");
                var usersData = JsonSerializer.Deserialize<List<User>>(users);
                if (usersData?.Count > 0)
                {
                    foreach (var user in usersData)
                    {
                        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
                        await dbContext.Users.AddAsync(user);
                    }
                    await dbContext.SaveChangesAsync();
                }
            }

            // Branches
            if (!dbContext.Branches.Any())
            {
                var branches = File.ReadAllText("../Repository/DataSeed/branches.json");
                var branchesData = JsonSerializer.Deserialize<List<Branch>>(branches);
                if (branchesData?.Count > 0)
                {
                    foreach (var branch in branchesData)
                    {
                        await dbContext.Branches.AddAsync(branch);
                    }
                    await dbContext.SaveChangesAsync();
                }
            }

            // Employees
            if (!dbContext.Employees.Any())
            {
                var employees = File.ReadAllText("../Repository/DataSeed/employees.json");
                var employeesData = JsonSerializer.Deserialize<List<Employee>>(employees);
                if (employeesData?.Count > 0)
                {
                    foreach (var employee in employeesData)
                    {
                        await dbContext.Employees.AddAsync(employee);
                    }
                    await dbContext.SaveChangesAsync();
                }
            }

            // Customers
            if (!dbContext.Customers.Any())
            {
                var customers = File.ReadAllText("../Repository/DataSeed/customers.json");
                var customersData = JsonSerializer.Deserialize<List<Customer>>(customers);
                if (customersData?.Count > 0)
                {
                    foreach (var customer in customersData)
                    {
                        await dbContext.Customers.AddAsync(customer);
                    }
                    await dbContext.SaveChangesAsync();
                }
            }

            // Accounts
            if (!dbContext.Accounts.Any())
            {
                var accounts = File.ReadAllText("../Repository/DataSeed/accounts.json");
                var accountsData = JsonSerializer.Deserialize<List<Account>>(accounts);
                if (accountsData?.Count > 0)
                {
                    foreach (var account in accountsData)
                    {
                        await dbContext.Accounts.AddAsync(account);
                    }
                    await dbContext.SaveChangesAsync();
                }
            }

            // Cards
            if (!dbContext.Cards.Any())
            {
                var cards = File.ReadAllText("../Repository/DataSeed/cards.json");
                var cardsData = JsonSerializer.Deserialize<List<Card>>(cards);
                if (cardsData?.Count > 0)
                {
                    foreach (var card in cardsData)
                    {
                        await dbContext.Cards.AddAsync(card);
                    }
                    await dbContext.SaveChangesAsync();
                }
            }

            // Loans
            if (!dbContext.Loans.Any())
            {
                var loans = File.ReadAllText("../Repository/DataSeed/loans.json");
                var loansData = JsonSerializer.Deserialize<List<Loan>>(loans);
                if (loansData?.Count > 0)
                {
                    foreach (var loan in loansData)
                    {
                        await dbContext.Loans.AddAsync(loan);
                    }
                    await dbContext.SaveChangesAsync();
                }
            }

            // Transactions
            if (!dbContext.Transactions.Any())
            {
                var transactions = File.ReadAllText("../Repository/DataSeed/transactions.json");
                var transactionsData = JsonSerializer.Deserialize<List<Transaction>>(transactions);
                if (transactionsData?.Count > 0)
                {
                    foreach (var transaction in transactionsData)
                    {
                        await dbContext.Transactions.AddAsync(transaction);
                    }
                    await dbContext.SaveChangesAsync();
                }
            }

            // Payments
            if (!dbContext.Payments.Any())
            {
                var payments = File.ReadAllText("../Repository/DataSeed/payments.json");
                var paymentsData = JsonSerializer.Deserialize<List<Payment>>(payments);
                if (paymentsData?.Count > 0)
                {
                    foreach (var payment in paymentsData)
                    {
                        await dbContext.Payments.AddAsync(payment);
                    }
                    await dbContext.SaveChangesAsync();
                }
            }

            // Beneficiaries
            if (!dbContext.Benificiaries.Any())
            {
                var beneficiaries = File.ReadAllText("../Repository/DataSeed/beneficiaries.json");
                var beneficiariesData = JsonSerializer.Deserialize<List<Benificiary>>(beneficiaries);
                if (beneficiariesData?.Count > 0)
                {
                    foreach (var benificiary in beneficiariesData)
                    {
                        await dbContext.Benificiaries.AddAsync(benificiary);
                    }
                    await dbContext.SaveChangesAsync();
                }
            }

        }
    }
}
