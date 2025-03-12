using System;
using System.Collections.Generic;


namespace PersonalFinanceManager
{
    class Expense
    {
        public string ItemName { get; set; }
        public decimal Amount { get; set; }
    }

    class Program
    {
        static List<decimal> incomes = new List<decimal>();
        static List<Expense> expenses = new List<Expense>();
        public static void Main(string[] args)
        {
            Console.Title = "Personal Finance Manager";
            Console.ForegroundColor = ConsoleColor.Cyan;

            Console.WriteLine("Welcome to Alan's Personal FInance Manager!");

            while (true) 
            {
                Console.WriteLine("\n===========================");
                Console.WriteLine("1. Add Income");
                Console.WriteLine("2. Add Expense");
                Console.WriteLine("3. Calculate Totals");
                Console.WriteLine("4. Set Budgets");
                Console.WriteLine("5. Exit");
                Console.WriteLine("===========================");

                Console.WriteLine("\n");
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

                        break;
                        
                }
                    
            }
        }

        static void AddIncome()
        {
            Console.Clear();
            Console.WriteLine("===========================");
            Console.WriteLine("Add Income");
            Console.WriteLine("===========================");

            Console.WriteLine("Enter the income amount: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal income))
            {
                incomes.Add(income);
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

            Console.WriteLine("Enter the item name: ");
            string itemName = Console.ReadLine();

            Console.WriteLine("Enter the expense amount: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal expenseAmount))
            {
                Expense expense = new Expense { ItemName = itemName, Amount = expenseAmount };
                expenses.Add(expense);
                Console.WriteLine("\nExpense added succefully!");
            }
            else
            {
                Console.WriteLine("\nPress any key to continue...");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            Console.Clear();
        }

        static void CalculateTotals()
        {
            Console.Clear();
            Console.WriteLine("===========================");
            Console.WriteLine("Add Expense");
            Console.WriteLine("===========================");

            decimal totalIncome = 0;
            decimal totalExpenses = 0;

            foreach (var income in incomes)
            {
                totalIncome += income;
            }

            Console.WriteLine("\nExpenses:");
            foreach (var expense in expenses)
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
            Console.WriteLine("Add Expense");
            Console.WriteLine("===========================");

            Console.WriteLine("Enter the budget amount: ");
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
        }
    }
}