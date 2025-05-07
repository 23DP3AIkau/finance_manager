using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;

namespace PersonalFinanceManager
{
    public partial class Form1 : Form
    {
        private static string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data\\accounts.json");
        private List<Account> accounts = new List<Account>();
        private Account? currentAccount;

        public Form1()
        {
            InitializeComponent();
            InitializeUI();
            LoadAccounts();
            ShowAccountSelection();
        }

        private void InitializeUI()
        {
            this.Text = "Personal Finance Manager";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Segoe UI", 10);
        }

        private void LoadAccounts()
        {
            string directoryPath = Path.GetDirectoryName(filePath);

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, "[]");
            }

            try
            {
                string json = File.ReadAllText(filePath);
                accounts = JsonSerializer.Deserialize<List<Account>>(json) ?? new List<Account>();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error reading accounts file: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                accounts = new List<Account>();
            }




            // TEST ACCOUNT CREATION
            bool testAccountExists = accounts.Any(a => a.AccountName == "Test Account");
            
            if (!testAccountExists)
            {
                accounts.Add(CreateTestAccount());
                SaveAccounts();
            }
        }

        private void SaveAccounts()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(accounts, options);
            File.WriteAllText(filePath, json);
        }

        private void ShowAccountSelection()
        {
            mainPanel.Controls.Clear();

            if (accounts.Count == 0)
            {
                var createLabel = new Label
                {
                    Text = "No accounts found. Please create a new account.",
                    Location = new Point(20, 20),
                    AutoSize = true
                };
                mainPanel.Controls.Add(createLabel);

                ShowCreateAccountForm();
                return;
            }

            var selectLabel = new Label
            {
                Text = "Select an account:",
                Location = new Point(20, 20),
                AutoSize = true
            };
            mainPanel.Controls.Add(selectLabel);

            var accountList = new ListBox
            {
                Location = new Point(20, 50),
                Size = new Size(300, 200)
            };
            accountList.Items.AddRange(accounts.Select(a => a.AccountName).ToArray());
            mainPanel.Controls.Add(accountList);

            // Select Account Button
            var selectButton = new Button
            {
                Text = "Select Account",
                Location = new Point(20, 260),
                Size = new Size(120, 30)
            };
            selectButton.Click += (s, e) =>
            {
                if (accountList.SelectedIndex >= 0)
                {
                    currentAccount = accounts[accountList.SelectedIndex];
                    ShowMainMenu();
                }
                else
                {
                    MessageBox.Show("Please select an account", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            };
            mainPanel.Controls.Add(selectButton);

            // Create New Account Button
            var createButton = new Button
            {
                Text = "Create New Account",
                Location = new Point(160, 260),
                Size = new Size(160, 30)
            };
            createButton.Click += (s, e) => ShowCreateAccountForm();
            mainPanel.Controls.Add(createButton);

            // Create Test Account Button
            var createTestButton = new Button
            {
                Text = "Create Test Account",
                Location = new Point(20, 300),
                Size = new Size(160, 30)
            };
            createTestButton.Click += (s, e) =>
            {
                accounts.Add(CreateTestAccount());
                SaveAccounts();
                ShowAccountSelection(); // Refresh the account list
                MessageBox.Show("Test account created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };
            mainPanel.Controls.Add(createTestButton);

            // Delete Account Button
            var deleteButton = new Button
            {
                Text = "Delete Account",
                Location = new Point(190, 300),
                Size = new Size(130, 30)
            };
            deleteButton.Click += (s, e) =>
            {
                if (accountList.SelectedIndex >= 0)
                {
                    var accountToDelete = accounts[accountList.SelectedIndex];

                    if (MessageBox.Show($"Are you sure you want to delete account '{accountToDelete.AccountName}'?",
                        "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        accounts.RemoveAt(accountList.SelectedIndex);
                        SaveAccounts();

                        // Clear current account if it was the deleted one
                        if (currentAccount != null && currentAccount.AccountName == accountToDelete.AccountName)
                        {
                            currentAccount = null;
                        }

                        ShowAccountSelection(); // Refresh the account list
                        MessageBox.Show("Account deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Please select an account to delete", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            };
            mainPanel.Controls.Add(deleteButton);

            // Exit Program Button
            var exitButton = new Button
            {
                Text = "Exit Program",
                Location = new Point(20, 340),
                Size = new Size(300, 30)
            };
            exitButton.Click += (s, e) => this.Close();
            mainPanel.Controls.Add(exitButton);
        }

        private void ShowCreateAccountForm()
        {
            mainPanel.Controls.Clear();

            var nameLabel = new Label
            {
                Text = "Enter new account name:",
                Location = new Point(20, 20),
                AutoSize = true
            };
            mainPanel.Controls.Add(nameLabel);

            var nameTextBox = new TextBox
            {
                Location = new Point(20, 50),
                Size = new Size(300, 30)
            };
            mainPanel.Controls.Add(nameTextBox);

            var createButton = new Button
            {
                Text = "Create Account",
                Location = new Point(20, 90),
                Size = new Size(120, 30)
            };
            createButton.Click += (s, e) =>
            {
                if (!string.IsNullOrWhiteSpace(nameTextBox.Text))
                {
                    currentAccount = new Account { AccountName = nameTextBox.Text };
                    accounts.Add(currentAccount);
                    SaveAccounts();
                    ShowMainMenu();
                }
                else
                {
                    MessageBox.Show("Account name cannot be empty", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            };
            mainPanel.Controls.Add(createButton);

            var cancelButton = new Button
            {
                Text = "Cancel",
                Location = new Point(160, 90),
                Size = new Size(120, 30)
            };
            cancelButton.Click += (s, e) => ShowAccountSelection();
            mainPanel.Controls.Add(cancelButton);
        }

        private void ShowMainMenu()
        {
            mainPanel.Controls.Clear();

            var accountLabel = new Label
            {
                Text = $"Current account: {currentAccount?.AccountName}",
                Location = new Point(20, 20),
                AutoSize = true,
                Font = new Font(this.Font, FontStyle.Bold)
            };
            mainPanel.Controls.Add(accountLabel);

            // Exit Account button (top-right corner)
            var exitButton = new Button
            {
                Text = "Exit Account",
                Location = new Point(mainPanel.Width - 120, 20),
                Size = new Size(100, 30)
            };
            exitButton.Click += (s, e) => ShowAccountSelection();
            mainPanel.Controls.Add(exitButton);

            var menuPanel = new Panel
            {
                Location = new Point(20, 60),
                Size = new Size(300, 350),
                BorderStyle = BorderStyle.FixedSingle
            };
            mainPanel.Controls.Add(menuPanel);

            var buttons = new[]
            {
                new Button { Text = "1. Income", Tag = "1" },
                new Button { Text = "2. Expense", Tag = "2" },
                new Button { Text = "3. Calculate Totals", Tag = "3" },
                new Button { Text = "4. Set Budgets", Tag = "4" },
                new Button { Text = "5. Monthly Overview", Tag = "5" },
                new Button { Text = "6. Yearly Overview", Tag = "6" },
                new Button { Text = "7. Exit Program", Tag = "7" }
            };

            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].Size = new Size(250, 40);
                buttons[i].Location = new Point(25, 20 + i * 45);
                buttons[i].Click += MenuButton_Click;
                menuPanel.Controls.Add(buttons[i]);
            }
        }

        private void MenuButton_Click(object? sender, EventArgs e)
        {
            var button = sender as Button;
            switch (button?.Tag?.ToString())
            {
                case "1":
                    ShowAddIncomeForm();
                    break;
                case "2":
                    ShowExpenseForm();
                    break;
                case "3":
                    ShowCalculateTotals();
                    break;
                case "4":
                    ShowSetBudgetsForm();
                    break;
                case "5":
                    ShowMonthlyOverview();
                    break;
                case "6":
                    ShowYearlyOverview();
                    break;
                case "7":
                    this.Close();
                    break;
            }
        }

        private void ShowAddIncomeForm()
        {
            mainPanel.Controls.Clear();

            var backButton = new Button
            {
                Text = "← Back",
                Location = new Point(mainPanel.Width - 100, 20),
                Size = new Size(80, 30)
            };
            backButton.Click += (s, e) => ShowMainMenu();
            mainPanel.Controls.Add(backButton);

            var titleLabel = new Label
            {
                Text = "Income",
                Location = new Point(20, 20),
                AutoSize = true,
                Font = new Font(this.Font, FontStyle.Bold)
            };
            mainPanel.Controls.Add(titleLabel);

            // Income category selection
            var categoryLabel = new Label
            {
                Text = "Income Category:",
                Location = new Point(20, 60),
                AutoSize = true
            };
            mainPanel.Controls.Add(categoryLabel);

            var categoryComboBox = new ComboBox
            {
                Location = new Point(20, 90),
                Size = new Size(200, 30),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            categoryComboBox.Items.AddRange(new[] { "Active Income", "Portfolio Income", "Passive Income", "Other" });
            mainPanel.Controls.Add(categoryComboBox);

            var amountLabel = new Label
            {
                Text = "Amount:",
                Location = new Point(20, 130),
                AutoSize = true
            };
            mainPanel.Controls.Add(amountLabel);

            var amountTextBox = new TextBox
            {
                Location = new Point(20, 160),
                Size = new Size(200, 30)
            };
            mainPanel.Controls.Add(amountTextBox);

            // Date selection
            var dateLabel = new Label
            {
                Text = "Date (MM/YYYY):",
                Location = new Point(20, 200),
                AutoSize = true
            };
            mainPanel.Controls.Add(dateLabel);

            var monthComboBox = new ComboBox
            {
                Location = new Point(20, 230),
                Size = new Size(100, 30),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            monthComboBox.Items.AddRange(Enumerable.Range(1, 12).Select(m => m.ToString("00")).ToArray());
            mainPanel.Controls.Add(monthComboBox);

            var yearComboBox = new ComboBox
            {
                Location = new Point(130, 230),
                Size = new Size(100, 30),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            yearComboBox.Items.AddRange(Enumerable.Range(2023, 5).Select(y => y.ToString()).ToArray());
            yearComboBox.SelectedItem = DateTime.Now.Year.ToString();
            mainPanel.Controls.Add(yearComboBox);

            var addButton = new Button
            {
                Text = "Add Income",
                Location = new Point(20, 270),
                Size = new Size(120, 30)
            };
            addButton.Click += (s, e) =>
            {
                if (categoryComboBox.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select an income category", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!decimal.TryParse(amountTextBox.Text, out decimal amount) || amount <= 0)
                {
                    MessageBox.Show("Please enter a valid positive amount", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (monthComboBox.SelectedIndex == -1 || yearComboBox.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select month and year", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                currentAccount?.Incomes.Add(new IncomeRecord
                {
                    Amount = amount,
                    Category = categoryComboBox.SelectedItem.ToString() ?? "Other",
                    Month = int.Parse(monthComboBox.SelectedItem.ToString() ?? "0"),
                    Year = int.Parse(yearComboBox.SelectedItem.ToString() ?? "0")
                });
                SaveAccounts();
                MessageBox.Show("Income added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ShowMainMenu();
            };
            mainPanel.Controls.Add(addButton);
        }

        private void ShowExpenseForm()
        {
            mainPanel.Controls.Clear();

            var backButton = new Button
            {
                Text = "← Back",
                Location = new Point(mainPanel.Width - 100, 20),
                Size = new Size(80, 30)
            };
            backButton.Click += (s, e) => ShowMainMenu();
            mainPanel.Controls.Add(backButton);

            var titleLabel = new Label
            {
                Text = "Expense",
                Location = new Point(20, 20),
                AutoSize = true,
                Font = new Font(this.Font, FontStyle.Bold)
            };
            mainPanel.Controls.Add(titleLabel);

            var categoryLabel = new Label
            {
                Text = "Expense Category:",
                Location = new Point(20, 60),
                AutoSize = true
            };
            mainPanel.Controls.Add(categoryLabel);

            var categoryComboBox = new ComboBox
            {
                Location = new Point(20, 90),
                Size = new Size(200, 30),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            var amountLabel = new Label
            {
                Text = "Amount:",
                Location = new Point(20, 130),
                AutoSize = true
            };
            mainPanel.Controls.Add(amountLabel);

            var amountTextBox = new TextBox
            {
                Location = new Point(20, 160),
                Size = new Size(200, 30)
            };
            mainPanel.Controls.Add(amountTextBox);

            // Expense category selection
            
            categoryComboBox.Items.AddRange(new[] { "Utilities", "Groceries", "Housing", "Transportation", "Health Care", "Lifestyle", "Education" });
            mainPanel.Controls.Add(categoryComboBox);

            // Date selection
            var dateLabel = new Label
            {
                Text = "Date (MM/YYYY):",
                Location = new Point(20, 200),
                AutoSize = true
            };
            mainPanel.Controls.Add(dateLabel);

            var monthComboBox = new ComboBox
            {
                Location = new Point(20, 230),
                Size = new Size(100, 30),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            monthComboBox.Items.AddRange(Enumerable.Range(1, 12).Select(m => m.ToString("00")).ToArray());
            mainPanel.Controls.Add(monthComboBox);

            var yearComboBox = new ComboBox
            {
                Location = new Point(130, 230),
                Size = new Size(100, 30),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            yearComboBox.Items.AddRange(Enumerable.Range(2023, 5).Select(y => y.ToString()).ToArray());
            yearComboBox.SelectedItem = DateTime.Now.Year.ToString();
            mainPanel.Controls.Add(yearComboBox);

            var addButton = new Button
            {
                Text = "Add Expense",
                Location = new Point(20, 270),
                Size = new Size(120, 30)
            };
            addButton.Click += (s, e) =>
            {
                if (!decimal.TryParse(amountTextBox.Text, out decimal amount) || amount <= 0)
                {
                    MessageBox.Show("Please enter a valid positive amount", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (categoryComboBox.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select an expense category", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (monthComboBox.SelectedIndex == -1 || yearComboBox.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select month and year", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                currentAccount?.Expenses.Add(new Expense
                {
                    Amount = amount,
                    Category = categoryComboBox.SelectedItem.ToString() ?? "Other",
                    Month = int.Parse(monthComboBox.SelectedItem.ToString() ?? "0"),
                    Year = int.Parse(yearComboBox.SelectedItem.ToString() ?? "0")
                });
                SaveAccounts();
                MessageBox.Show("Expense added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ShowMainMenu();
            };
            mainPanel.Controls.Add(addButton);
        }

        private void ShowCalculateTotals()
        {
            mainPanel.Controls.Clear();

            var backButton = new Button
            {
                Text = "← Back",
                Location = new Point(mainPanel.Width - 100, 20),
                Size = new Size(80, 30)
            };
            backButton.Click += (s, e) => ShowMainMenu();
            mainPanel.Controls.Add(backButton);

            var titleLabel = new Label
            {
                Text = "Financial Summary",
                Location = new Point(20, 20),
                AutoSize = true,
                Font = new Font(this.Font, FontStyle.Bold)
            };
            mainPanel.Controls.Add(titleLabel);

            // Date selection for filtering
            var filterLabel = new Label
            {
                Text = "Filter by Month/Year:",
                Location = new Point(20, 60),
                AutoSize = true
            };
            mainPanel.Controls.Add(filterLabel);

            var monthComboBox = new ComboBox
            {
                Location = new Point(20, 90),
                Size = new Size(100, 30),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            monthComboBox.Items.Add("All Months");
            monthComboBox.Items.AddRange(Enumerable.Range(1, 12).Select(m => m.ToString("00")).ToArray());
            monthComboBox.SelectedIndex = 0;
            mainPanel.Controls.Add(monthComboBox);

            var yearComboBox = new ComboBox
            {
                Location = new Point(130, 90),
                Size = new Size(100, 30),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            yearComboBox.Items.Add("All Years");
            yearComboBox.Items.AddRange(Enumerable.Range(2023, 5).Select(y => y.ToString()).ToArray());
            yearComboBox.SelectedIndex = 0;
            mainPanel.Controls.Add(yearComboBox);

            var filterButton = new Button
            {
                Text = "Apply Filter",
                Location = new Point(240, 90),
                Size = new Size(100, 30)
            };
            filterButton.Click += (s, e) => UpdateFinancialSummary(monthComboBox, yearComboBox);
            mainPanel.Controls.Add(filterButton);

            UpdateFinancialSummary(monthComboBox, yearComboBox);
        }


        private Account CreateTestAccount()
        {
            var testAccount = new Account
            {
                AccountName = "Test Account",
                Incomes = new List<IncomeRecord>(),
                Expenses = new List<Expense>(),
                Budgets = new List<Budget>()
            };

            var random = new Random();
            string[] incomeCategories = { "Active Income", "Portfolio Income", "Passive Income" };
            string[] expenseCategories = { "Utilities", "Groceries", "Housing", "Transportation", "Health Care", "Lifestyle" };

            for (int year = 2023; year <= 2027; year++)
            {
                for (int month = 1; month <= 12; month++)
                {
                    // Add incomes (2-4 per month)
                    int incomeCount = random.Next(2, 5);
                    for (int i = 0; i < incomeCount; i++)
                    {
                        testAccount.Incomes.Add(new IncomeRecord
                        {
                            Amount = Math.Round((decimal)(1500 + random.NextDouble() * 3000), 2),
                            Category = incomeCategories[random.Next(incomeCategories.Length)],
                            Month = month,
                            Year = year
                        });
                    }

                    // Add expenses (5-10 per month)
                    int expenseCount = random.Next(5, 11);
                    for (int i = 0; i < expenseCount; i++)
                    {
                        string category = expenseCategories[random.Next(expenseCategories.Length)];
                        decimal amount = Math.Round((decimal)(50 + random.NextDouble() * 500), 2);

                        testAccount.Expenses.Add(new Expense
                        {
                            Amount = amount,
                            Category = category,
                            Month = month,
                            Year = year
                        });
                    }

                    // Add budgets (one per category per month)
                    foreach (var category in expenseCategories)
                    {
                        testAccount.Budgets.Add(new Budget
                        {
                            Category = category,
                            Amount = Math.Round((decimal)(200 + random.NextDouble() * 800), 2),
                            Month = month,
                            Year = year
                        });
                    }
                }
            }

            return testAccount;
        }



        private Label? periodLabel; // Add this class field to keep track of the label

        private void UpdateFinancialSummary(ComboBox monthComboBox, ComboBox yearComboBox)
        {
            // Clear previous summary controls except the title and filter controls
            for (int i = mainPanel.Controls.Count - 1; i >= 0; i--)
            {
                var control = mainPanel.Controls[i];
                if (control.Location.Y > 130 &&
                    control != monthComboBox &&
                    control != yearComboBox &&
                    control.GetType() != typeof(Button)) // Keep the filter button
                {
                    mainPanel.Controls.RemoveAt(i);
                }
            }

            bool filterByMonth = monthComboBox.SelectedIndex > 0;
            bool filterByYear = yearComboBox.SelectedIndex > 0;
            int? month = filterByMonth ? int.Parse(monthComboBox.SelectedItem.ToString() ?? "0") : null;
            int? year = filterByYear ? int.Parse(yearComboBox.SelectedItem.ToString() ?? "0") : null;

            var filteredIncomes = currentAccount?.Incomes ?? Enumerable.Empty<IncomeRecord>();
            var filteredExpenses = currentAccount?.Expenses ?? Enumerable.Empty<Expense>();

            if (filterByMonth || filterByYear)
            {
                filteredIncomes = filteredIncomes.Where(i =>
                    (!filterByMonth || i.Month == month) &&
                    (!filterByYear || i.Year == year));

                filteredExpenses = filteredExpenses.Where(e =>
                    (!filterByMonth || e.Month == month) &&
                    (!filterByYear || e.Year == year));
            }

            decimal totalIncome = filteredIncomes.Sum(i => i.Amount);
            decimal totalExpenses = filteredExpenses.Sum(e => e.Amount);
            decimal netIncome = totalIncome - totalExpenses;

            // Update the period label text
            string periodText = "All Time";
            if (filterByMonth && !filterByYear)
            {
                periodText = $"All {month:00} months";
            }
            else if (!filterByMonth && filterByYear)
            {
                periodText = $"Year {year}";
            }
            else if (filterByMonth && filterByYear)
            {
                periodText = $"{month:00}/{year}";
            }

            // Create or update the period label
            if (periodLabel == null)
            {
                periodLabel = new Label
                {
                    Text = $"Showing: {periodText}",
                    Location = new Point(20, 130),
                    AutoSize = true,
                    Font = new Font(this.Font, FontStyle.Bold),
                    Name = "periodLabel" // Give it a name so we can find it later
                };
                mainPanel.Controls.Add(periodLabel);
            }
            else
            {
                periodLabel.Text = $"Showing: {periodText}";
            }

            var incomeLabel = new Label
            {
                Text = $"Total Income: {totalIncome:C}",
                Location = new Point(20, 160),
                AutoSize = true
            };
            mainPanel.Controls.Add(incomeLabel);

            var expensesLabel = new Label
            {
                Text = "Expenses by Category:",
                Location = new Point(20, 190),
                AutoSize = true
            };
            mainPanel.Controls.Add(expensesLabel);

            // Group expenses by category
            var expensesByCategory = filteredExpenses
                .GroupBy(e => e.Category)
                .Select(g => new { Category = g.Key, Total = g.Sum(e => e.Amount) })
                .OrderByDescending(g => g.Total);

            int yPos = 220;
            foreach (var category in expensesByCategory)
            {
                var categoryLabel = new Label
                {
                    Text = $"{category.Category}: {category.Total:C}",
                    Location = new Point(20, yPos),
                    AutoSize = true
                };
                mainPanel.Controls.Add(categoryLabel);
                yPos += 30;
            }

            var totalExpensesLabel = new Label
            {
                Text = $"Total Expenses: {totalExpenses:C}",
                Location = new Point(20, yPos),
                AutoSize = true
            };
            mainPanel.Controls.Add(totalExpensesLabel);
            yPos += 30;

            var netIncomeLabel = new Label
            {
                Text = $"Net Income: {netIncome:C}",
                Location = new Point(20, yPos),
                AutoSize = true,
                Font = new Font(this.Font, FontStyle.Bold)
            };
            mainPanel.Controls.Add(netIncomeLabel);
        }

        private void ShowSetBudgetsForm()
        {
            mainPanel.Controls.Clear();

            var backButton = new Button
            {
                Text = "← Back",
                Location = new Point(mainPanel.Width - 100, 20),
                Size = new Size(80, 30)
            };
            backButton.Click += (s, e) => ShowMainMenu();
            mainPanel.Controls.Add(backButton);

            var titleLabel = new Label
            {
                Text = "Set Budgets",
                Location = new Point(20, 20),
                AutoSize = true,
                Font = new Font(this.Font, FontStyle.Bold)
            };
            mainPanel.Controls.Add(titleLabel);

            var amountLabel = new Label
            {
                Text = "Budget Amount:",
                Location = new Point(20, 60),
                AutoSize = true
            };
            mainPanel.Controls.Add(amountLabel);

            var amountTextBox = new TextBox
            {
                Location = new Point(20, 90),
                Size = new Size(200, 30)
            };
            mainPanel.Controls.Add(amountTextBox);

            var categoryLabel = new Label
            {
                Text = "Category:",
                Location = new Point(20, 130),
                AutoSize = true
            };
            mainPanel.Controls.Add(categoryLabel);

            var categoryComboBox = new ComboBox
            {
                Location = new Point(20, 160),
                Size = new Size(200, 30),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            categoryComboBox.Items.AddRange(new[] { "Utilities", "Groceries", "Housing", "Transportation", "Health Care", "Lifestyle", "Education" });
            mainPanel.Controls.Add(categoryComboBox);

            // Month and year for budget
            var periodLabel = new Label
            {
                Text = "Budget Period:",
                Location = new Point(20, 200),
                AutoSize = true
            };
            mainPanel.Controls.Add(periodLabel);

            var monthComboBox = new ComboBox
            {
                Location = new Point(20, 230),
                Size = new Size(100, 30),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            monthComboBox.Items.AddRange(Enumerable.Range(1, 12).Select(m => m.ToString("00")).ToArray());
            mainPanel.Controls.Add(monthComboBox);

            var yearComboBox = new ComboBox
            {
                Location = new Point(130, 230),
                Size = new Size(100, 30),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            yearComboBox.Items.AddRange(Enumerable.Range(2023, 5).Select(y => y.ToString()).ToArray());
            yearComboBox.SelectedItem = DateTime.Now.Year.ToString();
            mainPanel.Controls.Add(yearComboBox);

            var setButton = new Button
            {
                Text = "Set Budget",
                Location = new Point(20, 270),
                Size = new Size(120, 30)
            };
            setButton.Click += (s, e) =>
            {
                if (!decimal.TryParse(amountTextBox.Text, out decimal amount) || amount <= 0)
                {
                    MessageBox.Show("Please enter a valid positive amount", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (categoryComboBox.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select a category", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (monthComboBox.SelectedIndex == -1 || yearComboBox.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select month and year", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var budget = new Budget
                {
                    Category = categoryComboBox.SelectedItem.ToString() ?? "Other",
                    Amount = amount,
                    Month = int.Parse(monthComboBox.SelectedItem.ToString() ?? "0"),
                    Year = int.Parse(yearComboBox.SelectedItem.ToString() ?? "0")
                };

                // Remove existing budget for same category and period
                currentAccount?.Budgets.RemoveAll(b =>
                    b.Category == budget.Category &&
                    b.Month == budget.Month &&
                    b.Year == budget.Year);

                currentAccount?.Budgets.Add(budget);
                SaveAccounts();

                MessageBox.Show($"Budget for {budget.Category} set to {amount:C} for {budget.Month:00}/{budget.Year}",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ShowMainMenu();
            };
            mainPanel.Controls.Add(setButton);
        }

        private void ShowMonthlyOverview()
        {
            mainPanel.Controls.Clear();

            var backButton = new Button
            {
                Text = "← Back",
                Location = new Point(mainPanel.Width - 100, 20),
                Size = new Size(80, 30)
            };
            backButton.Click += (s, e) => ShowMainMenu();
            mainPanel.Controls.Add(backButton);

            var titleLabel = new Label
            {
                Text = "Monthly Overview",
                Location = new Point(20, 20),
                AutoSize = true,
                Font = new Font(this.Font, FontStyle.Bold)
            };
            mainPanel.Controls.Add(titleLabel);

            // Month and year selection
            var monthLabel = new Label
            {
                Text = "Select Month:",
                Location = new Point(20, 60),
                AutoSize = true
            };
            mainPanel.Controls.Add(monthLabel);

            var monthComboBox = new ComboBox
            {
                Location = new Point(20, 90),
                Size = new Size(100, 30),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            monthComboBox.Items.AddRange(Enumerable.Range(1, 12).Select(m => m.ToString("00")).ToArray());
            monthComboBox.SelectedIndex = DateTime.Now.Month - 1;
            mainPanel.Controls.Add(monthComboBox);

            var yearLabel = new Label
            {
                Text = "Select Year:",
                Location = new Point(130, 60),
                AutoSize = true
            };
            mainPanel.Controls.Add(yearLabel);

            var yearComboBox = new ComboBox
            {
                Location = new Point(130, 90),
                Size = new Size(100, 30),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            yearComboBox.Items.AddRange(Enumerable.Range(2023, 5).Select(y => y.ToString()).ToArray());
            yearComboBox.SelectedItem = DateTime.Now.Year.ToString();
            mainPanel.Controls.Add(yearComboBox);

            var showButton = new Button
            {
                Text = "Show Overview",
                Location = new Point(240, 90),
                Size = new Size(120, 30)
            };
            showButton.Click += (s, e) => UpdateMonthlyOverview(
                int.Parse(monthComboBox.SelectedItem.ToString() ?? "0"),
                int.Parse(yearComboBox.SelectedItem.ToString() ?? "0"));
            mainPanel.Controls.Add(showButton);

            // Show current month by default
            UpdateMonthlyOverview(DateTime.Now.Month, DateTime.Now.Year);
        }

        private void UpdateMonthlyOverview(int month, int year)
        {
            // Clear previous overview controls
            for (int i = mainPanel.Controls.Count - 1; i >= 0; i--)
            {
                if (mainPanel.Controls[i].Location.Y > 130)
                {
                    mainPanel.Controls.RemoveAt(i);
                }
            }

            var monthlyIncomes = currentAccount?.Incomes
                .Where(i => i.Month == month && i.Year == year)
                .OrderBy(i => i.Category) ?? Enumerable.Empty<IncomeRecord>();

            var monthlyExpenses = currentAccount?.Expenses
                .Where(e => e.Month == month && e.Year == year)
                .OrderBy(e => e.Category) ?? Enumerable.Empty<Expense>();

            var monthlyBudgets = currentAccount?.Budgets
                .Where(b => b.Month == month && b.Year == year)
                .OrderBy(b => b.Category) ?? Enumerable.Empty<Budget>();

            decimal totalIncome = monthlyIncomes.Sum(i => i.Amount);
            decimal totalExpenses = monthlyExpenses.Sum(e => e.Amount);
            decimal totalBudget = monthlyBudgets.Sum(b => b.Amount);

            // Create summary labels
            var summaryLabel = new Label
            {
                Text = $"Summary for {month:00}/{year}",
                Location = new Point(20, 130),
                AutoSize = true,
                Font = new Font(this.Font, FontStyle.Bold)
            };
            mainPanel.Controls.Add(summaryLabel);

            var incomeLabel = new Label
            {
                Text = $"Total Income: {totalIncome:C}",
                Location = new Point(20, 160),
                AutoSize = true
            };
            mainPanel.Controls.Add(incomeLabel);

            var expensesLabel = new Label
            {
                Text = $"Total Expenses: {totalExpenses:C}",
                Location = new Point(20, 190),
                AutoSize = true
            };
            mainPanel.Controls.Add(expensesLabel);

            var budgetLabel = new Label
            {
                Text = $"Total Budget: {totalBudget:C}",
                Location = new Point(20, 220),
                AutoSize = true
            };
            mainPanel.Controls.Add(budgetLabel);

            var netLabel = new Label
            {
                Text = $"Net Income: {(totalIncome - totalExpenses):C}",
                Location = new Point(20, 250),
                AutoSize = true,
                Font = new Font(this.Font, FontStyle.Bold)
            };
            mainPanel.Controls.Add(netLabel);

            // Create tab control for detailed view
            var tabControl = new TabControl
            {
                Location = new Point(20, 290),
                Size = new Size(700, 250)
            };
            mainPanel.Controls.Add(tabControl);

            // Incomes tab
            var incomesTab = new TabPage("Incomes");
            tabControl.TabPages.Add(incomesTab);

            var incomesGrid = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false, // Removes extra blank row
                RowTemplate = { Height = 22 } // Taller rows (default is 20)
            };
            incomesGrid.Columns.Add("Category", "Category");
            incomesGrid.Columns.Add("Amount", "Amount");
            incomesTab.Controls.Add(incomesGrid);

            foreach (var income in monthlyIncomes)
            {
                incomesGrid.Rows.Add(income.Category, $"{income.Amount:C}");
            }

            // Add total row to incomes
            incomesGrid.Rows.Add("TOTAL", $"{totalIncome:C}");
            incomesGrid.Rows[incomesGrid.Rows.Count - 1].DefaultCellStyle.Font =
                new Font(incomesGrid.Font, FontStyle.Bold);

            // Expenses tab
            var expensesTab = new TabPage("Expenses");
            tabControl.TabPages.Add(expensesTab);

            var expensesGrid = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                RowTemplate = { Height = 22 }
            };
            expensesGrid.Columns.Add("Category", "Category");
            expensesGrid.Columns.Add("Amount", "Amount");
            expensesTab.Controls.Add(expensesGrid);

            foreach (var expense in monthlyExpenses)
            {
                expensesGrid.Rows.Add(expense.Category, $"{expense.Amount:C}");
            }

            // Add total row to expenses
            expensesGrid.Rows.Add("TOTAL", $"{totalExpenses:C}");
            expensesGrid.Rows[expensesGrid.Rows.Count - 1].DefaultCellStyle.Font =
                new Font(expensesGrid.Font, FontStyle.Bold);

            // Budgets tab
            var budgetsTab = new TabPage("Budgets");
            tabControl.TabPages.Add(budgetsTab);

            var budgetsGrid = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                RowTemplate = { Height = 22 }
            };
            budgetsGrid.Columns.Add("Category", "Category");
            budgetsGrid.Columns.Add("Budget", "Budget");
            budgetsGrid.Columns.Add("Expenses", "Expenses");
            budgetsGrid.Columns.Add("Budget vs Expenses", "Budget vs Expenses");
            budgetsTab.Controls.Add(budgetsGrid);

            // Group expenses by category for comparison
            var expensesByCategory = monthlyExpenses
                .GroupBy(e => e.Category)
                .ToDictionary(g => g.Key, g => g.Sum(e => e.Amount));

            foreach (var budget in monthlyBudgets)
            {
                decimal categoryExpenses = expensesByCategory.ContainsKey(budget.Category) ?
                    expensesByCategory[budget.Category] : 0;
                decimal percentage = budget.Amount > 0 ?
                    (categoryExpenses / budget.Amount) * 100 : 0;
                string status = percentage > 100 ? "OVER" : "UNDER";

                budgetsGrid.Rows.Add(
                    budget.Category,
                    $"{budget.Amount:C}",
                    $"{categoryExpenses:C}",
                    $"{percentage:F2}% ({status})"
                );
            }

            // Add total row to budgets
            decimal totalBudgetExpenses = expensesByCategory.Sum(e => e.Value);
            decimal totalPercentage = totalBudget > 0 ?
                (totalBudgetExpenses / totalBudget) * 100 : 0;
            string totalStatus = totalPercentage > 100 ? "OVER" : "UNDER";

            budgetsGrid.Rows.Add(
                "TOTAL",
                $"{totalBudget:C}",
                $"{totalBudgetExpenses:C}",
                $"{totalPercentage:F2}% ({totalStatus})"
            );
            budgetsGrid.Rows[budgetsGrid.Rows.Count - 1].DefaultCellStyle.Font =
                new Font(budgetsGrid.Font, FontStyle.Bold);
        }

        private DataGridView yearlyDataGridView; // Add this class field

        private void ShowYearlyOverview()
        {
            mainPanel.Controls.Clear();

            var backButton = new Button
            {
                Text = "← Back",
                Location = new Point(mainPanel.Width - 100, 20),
                Size = new Size(80, 30)
            };
            backButton.Click += (s, e) => ShowMainMenu();
            mainPanel.Controls.Add(backButton);

            var titleLabel = new Label
            {
                Text = "Yearly Overview",
                Location = new Point(20, 20),
                AutoSize = true,
                Font = new Font(this.Font, FontStyle.Bold)
            };
            mainPanel.Controls.Add(titleLabel);

            // Year selection
            var yearLabel = new Label
            {
                Text = "Select Year:",
                Location = new Point(20, 60),
                AutoSize = true
            };
            mainPanel.Controls.Add(yearLabel);

            var yearComboBox = new ComboBox
            {
                Location = new Point(20, 90),
                Size = new Size(100, 30),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            yearComboBox.Items.AddRange(Enumerable.Range(2023, 5).Select(y => y.ToString()).ToArray());
            yearComboBox.SelectedItem = DateTime.Now.Year.ToString();
            mainPanel.Controls.Add(yearComboBox);

            // Initialize the DataGridView only once
            yearlyDataGridView = new DataGridView
            {
                Location = new Point(20, 130),
                Size = new Size(700, 311),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ScrollBars = ScrollBars.Vertical
            };
            mainPanel.Controls.Add(yearlyDataGridView);

            // Add columns (only once)
            yearlyDataGridView.Columns.Clear();
            yearlyDataGridView.Columns.Add("Month", "Month");
            yearlyDataGridView.Columns.Add("Income", "Income");
            yearlyDataGridView.Columns.Add("Expenses", "Expenses");
            yearlyDataGridView.Columns.Add("Budget", "Budget");
            yearlyDataGridView.Columns.Add("Difference", "Difference (Income - Expenses)");
            yearlyDataGridView.Columns.Add("BudgetDiff", "Budget - Expenses");

            // Total label
            var totalLabel = new Label
            {
                Location = new Point(20, 450),
                AutoSize = true,
                Font = new Font(this.Font, FontStyle.Bold),
                Name = "totalLabel"
            };
            mainPanel.Controls.Add(totalLabel);

            var showButton = new Button
            {
                Text = "Show Overview",
                Location = new Point(130, 90),
                Size = new Size(120, 30)
            };
            showButton.Click += (s, e) =>
            {
                if (yearComboBox.SelectedIndex != -1)
                {
                    UpdateYearlyOverview(int.Parse(yearComboBox.SelectedItem.ToString()));
                }
            };
            mainPanel.Controls.Add(showButton);

            // Show initial data
            if (yearComboBox.SelectedIndex != -1)
            {
                UpdateYearlyOverview(int.Parse(yearComboBox.SelectedItem.ToString()));
            }
        }

        private void UpdateYearlyOverview(int selectedYear)
        {
            // Clear previous data
            yearlyDataGridView.Rows.Clear();

            // Set DataGridView properties
            yearlyDataGridView.AllowUserToAddRows = false;
            yearlyDataGridView.RowTemplate.Height = 22;

            // Filter data by selected year
            var yearIncomes = currentAccount?.Incomes
                .Where(i => i.Year == selectedYear)
                .ToList() ?? new List<IncomeRecord>();

            var yearExpenses = currentAccount?.Expenses
                .Where(e => e.Year == selectedYear)
                .ToList() ?? new List<Expense>();

            var yearBudgets = currentAccount?.Budgets
                .Where(b => b.Year == selectedYear)
                .ToList() ?? new List<Budget>();

            // Monthly summaries and column totals
            var monthlySummaries = new List<(int Month, decimal Income, decimal Expenses, decimal Budget)>();
            decimal totalIncomeSum = 0;
            decimal totalExpensesSum = 0;
            decimal totalBudgetSum = 0;
            decimal totalDifferenceSum = 0;
            decimal totalBudgetDiffSum = 0;

            for (int month = 1; month <= 12; month++)
            {
                decimal monthIncome = yearIncomes
                    .Where(i => i.Month == month)
                    .Sum(i => i.Amount);

                decimal monthExpenses = yearExpenses
                    .Where(e => e.Month == month)
                    .Sum(e => e.Amount);

                decimal monthBudget = yearBudgets
                    .Where(b => b.Month == month)
                    .Sum(b => b.Amount);

                decimal monthDifference = monthIncome - monthExpenses;
                decimal monthBudgetDiff = monthBudget - monthExpenses;

                monthlySummaries.Add((month, monthIncome, monthExpenses, monthBudget));

                // Accumulate totals
                totalIncomeSum += monthIncome;
                totalExpensesSum += monthExpenses;
                totalBudgetSum += monthBudget;
                totalDifferenceSum += monthDifference;
                totalBudgetDiffSum += monthBudgetDiff;
            }

            // Add rows for each month
            foreach (var month in monthlySummaries)
            {
                yearlyDataGridView.Rows.Add(
                    $"{month.Month:00}/{selectedYear}",
                    $"{month.Income:C}",
                    $"{month.Expenses:C}",
                    $"{month.Budget:C}",
                    $"{(month.Income - month.Expenses):C}",
                    $"{(month.Budget - month.Expenses):C}"
                );
            }

            // Add TOTAL row
            int totalRowIndex = yearlyDataGridView.Rows.Add(
                "TOTAL",
                $"{totalIncomeSum:C}",
                $"{totalExpensesSum:C}",
                $"{totalBudgetSum:C}",
                $"{totalDifferenceSum:C}",
                $"{totalBudgetDiffSum:C}"
            );

            // Style the TOTAL row
            DataGridViewRow totalRow = yearlyDataGridView.Rows[totalRowIndex];
            totalRow.DefaultCellStyle.Font = new Font(yearlyDataGridView.Font, FontStyle.Bold);

            // Update totals label
            var totalLabel = mainPanel.Controls.Find("totalLabel", true).FirstOrDefault() as Label;
            if (totalLabel != null)
            {
                totalLabel.Text = $"Year {selectedYear} Totals - Income: {totalIncomeSum:C}, Expenses: {totalExpensesSum:C}, " +
                                 $"Budget: {totalBudgetSum:C}, Net: {totalDifferenceSum:C}";
            }
        }
    }

    public class Account
    {
        public string AccountName { get; set; } = string.Empty;
        public List<IncomeRecord> Incomes { get; set; } = new List<IncomeRecord>();
        public List<Expense> Expenses { get; set; } = new List<Expense>();
        public List<Budget> Budgets { get; set; } = new List<Budget>();
    }

    public class IncomeRecord
    {
        public decimal Amount { get; set; }
        public string Category { get; set; } = string.Empty;
        public int Month { get; set; }
        public int Year { get; set; }
    }

    public class Expense
    {
        public decimal Amount { get; set; }
        public string Category { get; set; } = string.Empty;
        public int Month { get; set; }
        public int Year { get; set; }
    }

    public class Budget
    {
        public string Category { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
    }
}