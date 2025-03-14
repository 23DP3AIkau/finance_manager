using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;

namespace PersonalFinanceManager
{
    class Expense
    {
        public string ItemName { get; set; }
        public decimal Amount { get; set; }
    }

    class Account
    {
        public string AccountName { get; set; }
        public List<decimal> Incomes { get; set; }
        public List<Expense> Expenses { get; set; }

        public Account()
        {
            Incomes = new List<decimal>();
            Expenses = new List<Expense>();
        }
    }

    class Program
    {
        // Pagaidam izmantoju debug caur bin folder
        static string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\data\\accounts.json");
        static List<Account> accounts = new List<Account>();
        static Account currentAccount;

        public static void Main(string[] args)
        {
            Console.Title = "Personal Finance Manager";
            Console.ForegroundColor = ConsoleColor.Cyan;

            LoadAccounts();

            if (accounts.Count == 0)
            {
                Console.WriteLine("No accounts found. Please create a new account.");
                CreateNewAccount();
            }
            else
            {
                Console.WriteLine("Select an account by number or type 'create' to create a new account:");
                for (int i = 0; i < accounts.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {accounts[i].AccountName}");
                }
                string input = Console.ReadLine();
                if (input.ToLower() == "create")
                {
                    CreateNewAccount();
                }
                else
                {
                    if (int.TryParse(input, out int selectedIndex) && selectedIndex >= 1 && selectedIndex <= accounts.Count)
                    {
                        currentAccount = accounts[selectedIndex - 1];
                    }
                    else
                    {
                        Console.WriteLine("Invalid selection. Creating new account.");
                        CreateNewAccount();
                    }
                }
            }

            Console.Clear();
            Console.WriteLine($"Current account: {currentAccount.AccountName}");

            while (true)
            {
                Console.WriteLine("\n===========================");
                Console.WriteLine("1. Add Income");
                Console.WriteLine("2. Add Expense");
                Console.WriteLine("3. Calculate Totals");
                Console.WriteLine("4. Set Budgets");
                Console.WriteLine("5. Exit");
                Console.WriteLine("===========================");

                Console.Write("\nEnter your choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddIncome();
                        break;
                    case "2":
                        AddExpense();
                        break;
                    case "3":
                        CalculateTotals();
                        break;
                    case "4":
                        SetBudgets();
                        break;
                    case "5":
                        Console.WriteLine("\nThank you for using Personal Finance Manager. Goodbye!");
                        return;
                    default:
                        Console.WriteLine("\nInvalid choice. Please try again");
                        break;
                }
            }
        }

        static void LoadAccounts()
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                try
                {
                    accounts = JsonSerializer.Deserialize<List<Account>>(json);
                    if (accounts == null)
                    {
                        accounts = new List<Account>();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error reading accounts file: " + ex.Message);
                    accounts = new List<Account>();
                }
            }
        }

        static void SaveAccounts()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(accounts, options);
            File.WriteAllText(filePath, json);
        }

        static void CreateNewAccount()
        {
            Console.WriteLine("Enter new account name:");
            string name = Console.ReadLine();
            currentAccount = new Account { AccountName = name };
            accounts.Add(currentAccount);
            SaveAccounts();
        }

        static void AddIncome()
        {
            Console.Clear();
            Console.WriteLine("===========================");
            Console.WriteLine("Add Income");
            Console.WriteLine("===========================");

            Console.Write("Enter the income amount: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal income))
            {
                currentAccount.Incomes.Add(income);
                SaveAccounts();
                Console.WriteLine("\nIncome added successfully!");
            }
            else
            {
                Console.WriteLine("\nInvalid input. Please enter a valid decimal value.");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            Console.Clear();
        }

        static void AddExpense()
        {
            Console.Clear();
            Console.WriteLine("===========================");
            Console.WriteLine("Add Expense");
            Console.WriteLine("===========================");

            Console.Write("Enter the item name: ");
            string itemName = Console.ReadLine();

            Console.Write("Enter the expense amount: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal expenseAmount))
            {
                Expense expense = new Expense { ItemName = itemName, Amount = expenseAmount };
                currentAccount.Expenses.Add(expense);
                SaveAccounts();
                Console.WriteLine("\nExpense added successfully!");
            }
            else
            {
                Console.WriteLine("\nInvalid input. Please enter a valid decimal value.");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            Console.Clear();
        }

        static void CalculateTotals()
        {
            Console.Clear();
            Console.WriteLine("===========================");
            Console.WriteLine("Calculate Totals");
            Console.WriteLine("===========================");

            decimal totalIncome = 0;
            decimal totalExpenses = 0;

            foreach (var income in currentAccount.Incomes)
            {
                totalIncome += income;
            }

            Console.WriteLine("\nExpenses:");
            foreach (var expense in currentAccount.Expenses)
            {
                totalExpenses += expense.Amount;
                Console.WriteLine($"{expense.ItemName}: {expense.Amount:C}");
            }

            decimal netIncome = totalIncome - totalExpenses;

            Console.WriteLine($"\nTotal Income: {totalIncome:C}");
            Console.WriteLine($"Total Expenses: {totalExpenses:C}");
            Console.WriteLine($"Net Income: {netIncome:C}");

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            Console.Clear();
        }

        static void SetBudgets()
        {
            Console.Clear();
            Console.WriteLine("===========================");
            Console.WriteLine("Set Budgets");
            Console.WriteLine("===========================");

            Console.Write("Enter the budget amount: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal budget))
            {
                Console.WriteLine("\nPlease select a category:");
                Console.WriteLine("1. Food");
                Console.WriteLine("2. Transportation");
                Console.WriteLine("3. Housing");
                Console.WriteLine("4. Entertainment");
                Console.Write("Enter your choice: ");
                string categoryChoice = Console.ReadLine();

                switch (categoryChoice)
                {
                    case "1":
                        DisplayBudget("Food", budget);
                        break;
                    case "2":
                        DisplayBudget("Transportation", budget);
                        break;
                    case "3":
                        DisplayBudget("Housing", budget);
                        break;
                    case "4":
                        DisplayBudget("Entertainment", budget);
                        break;
                    default:
                        Console.WriteLine("\nInvalid choice. Please try again.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("\nInvalid input. Please enter a valid decimal value.");
            }
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            Console.Clear();
        }

        static void DisplayBudget(string category, decimal budget)
        {
            Console.WriteLine($"\nBudget for {category} set to {budget:C}");
            // Papildu funkcionalitāti – šeit var pievienot budžeta saglabāšanu konta datos
        }
    }
}
