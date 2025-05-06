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

            var createButton = new Button
            {
                Text = "Create New Account",
                Location = new Point(160, 260),
                Size = new Size(160, 30)
            };
            createButton.Click += (s, e) => ShowCreateAccountForm();
            mainPanel.Controls.Add(createButton);
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
                Size = new Size(300, 300),
                BorderStyle = BorderStyle.FixedSingle
            };
            mainPanel.Controls.Add(menuPanel);

            var buttons = new[]
            {
                new Button { Text = "1. Income", Tag = "1" },
                new Button { Text = "2. Expense", Tag = "2" },
                new Button { Text = "3. Calculate Totals", Tag = "3" },
                new Button { Text = "4. Set Budgets", Tag = "4" },
                new Button { Text = "5. Show Total Overview", Tag = "5" },
                new Button { Text = "6. Exit Program", Tag = "6" }
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
                    ShowTotalOverview();
                    break;
                case "6":
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
            yearComboBox.Items.AddRange(Enumerable.Range(DateTime.Now.Year - 10, 20).Select(y => y.ToString()).ToArray());
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

            var itemLabel = new Label
            {
                Text = "Item Name:",
                Location = new Point(20, 60),
                AutoSize = true
            };
            mainPanel.Controls.Add(itemLabel);

            var itemTextBox = new TextBox
            {
                Location = new Point(20, 90),
                Size = new Size(200, 30)
            };
            mainPanel.Controls.Add(itemTextBox);

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
            var categoryLabel = new Label
            {
                Text = "Expense Category:",
                Location = new Point(20, 200),
                AutoSize = true
            };
            mainPanel.Controls.Add(categoryLabel);

            var categoryComboBox = new ComboBox
            {
                Location = new Point(20, 230),
                Size = new Size(200, 30),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            categoryComboBox.Items.AddRange(new[] { "Utilities", "Groceries", "Housing", "Transportation", "Health Care", "Lifestyle", "Education" });
            mainPanel.Controls.Add(categoryComboBox);

            // Date selection
            var dateLabel = new Label
            {
                Text = "Date (MM/YYYY):",
                Location = new Point(20, 270),
                AutoSize = true
            };
            mainPanel.Controls.Add(dateLabel);

            var monthComboBox = new ComboBox
            {
                Location = new Point(20, 300),
                Size = new Size(100, 30),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            monthComboBox.Items.AddRange(Enumerable.Range(1, 12).Select(m => m.ToString("00")).ToArray());
            mainPanel.Controls.Add(monthComboBox);

            var yearComboBox = new ComboBox
            {
                Location = new Point(130, 300),
                Size = new Size(100, 30),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            yearComboBox.Items.AddRange(Enumerable.Range(DateTime.Now.Year - 10, 20).Select(y => y.ToString()).ToArray());
            yearComboBox.SelectedItem = DateTime.Now.Year.ToString();
            mainPanel.Controls.Add(yearComboBox);

            var addButton = new Button
            {
                Text = "Add Expense",
                Location = new Point(20, 340),
                Size = new Size(120, 30)
            };
            addButton.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(itemTextBox.Text))
                {
                    MessageBox.Show("Please enter an item name", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

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
                    ItemName = itemTextBox.Text,
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
            yearComboBox.Items.AddRange(Enumerable.Range(DateTime.Now.Year - 10, 20).Select(y => y.ToString()).ToArray());
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

        private void UpdateFinancialSummary(ComboBox monthComboBox, ComboBox yearComboBox)
        {
            // Clear previous summary controls
            for (int i = mainPanel.Controls.Count - 1; i >= 0; i--)
            {
                if (mainPanel.Controls[i].Location.Y > 130)
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

            var incomeLabel = new Label
            {
                Text = $"Total Income: {totalIncome:C}",
                Location = new Point(20, 130),
                AutoSize = true
            };
            mainPanel.Controls.Add(incomeLabel);

            var expensesLabel = new Label
            {
                Text = "Expenses by Category:",
                Location = new Point(20, 160),
                AutoSize = true
            };
            mainPanel.Controls.Add(expensesLabel);

            // Group expenses by category
            var expensesByCategory = filteredExpenses
                .GroupBy(e => e.Category)
                .Select(g => new { Category = g.Key, Total = g.Sum(e => e.Amount) })
                .OrderByDescending(g => g.Total);

            int yPos = 190;
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
            yearComboBox.Items.AddRange(Enumerable.Range(DateTime.Now.Year - 10, 20).Select(y => y.ToString()).ToArray());
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

        private void ShowTotalOverview()
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
                Text = "Total Overview",
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
            yearComboBox.Items.AddRange(Enumerable.Range(DateTime.Now.Year - 10, 20).Select(y => y.ToString()).ToArray());
            yearComboBox.SelectedItem = DateTime.Now.Year.ToString();
            mainPanel.Controls.Add(yearComboBox);

            var showButton = new Button
            {
                Text = "Show Overview",
                Location = new Point(130, 90),
                Size = new Size(120, 30)
            };
            showButton.Click += (s, e) => UpdateTotalOverview(yearComboBox);
            mainPanel.Controls.Add(showButton);

            UpdateTotalOverview(yearComboBox);
        }

        private void UpdateTotalOverview(ComboBox yearComboBox)
        {
            // Clear previous overview controls
            for (int i = mainPanel.Controls.Count - 1; i >= 0; i--)
            {
                if (mainPanel.Controls[i].Location.Y > 130)
                {
                    mainPanel.Controls.RemoveAt(i);
                }
            }

            if (yearComboBox.SelectedIndex == -1)
                return;

            int year = int.Parse(yearComboBox.SelectedItem.ToString() ?? "0");

            var yearIncomes = currentAccount?.Incomes.Where(i => i.Year == year) ?? Enumerable.Empty<IncomeRecord>();
            var yearExpenses = currentAccount?.Expenses.Where(e => e.Year == year) ?? Enumerable.Empty<Expense>();
            var yearBudgets = currentAccount?.Budgets.Where(b => b.Year == year) ?? Enumerable.Empty<Budget>();

            // Monthly summaries
            var monthlySummaries = new List<(int Month, decimal Income, decimal Expenses, decimal Budget)>();

            for (int month = 1; month <= 12; month++)
            {
                decimal monthIncome = yearIncomes.Where(i => i.Month == month).Sum(i => i.Amount);
                decimal monthExpenses = yearExpenses.Where(e => e.Month == month).Sum(e => e.Amount);
                decimal monthBudget = yearBudgets.Where(b => b.Month == month).Sum(b => b.Amount);

                monthlySummaries.Add((month, monthIncome, monthExpenses, monthBudget));
            }

            // Create a DataGridView to display the overview
            var dataGridView = new DataGridView
            {
                Location = new Point(20, 130),
                Size = new Size(700, 300),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ScrollBars = ScrollBars.Vertical
            };
            mainPanel.Controls.Add(dataGridView);

            dataGridView.Columns.Add("Month", "Month");
            dataGridView.Columns.Add("Income", "Income");
            dataGridView.Columns.Add("Expenses", "Expenses");
            dataGridView.Columns.Add("Budget", "Budget");
            dataGridView.Columns.Add("Difference", "Difference (Income - Expenses)");
            dataGridView.Columns.Add("BudgetDiff", "Budget - Expenses");

            foreach (var month in monthlySummaries)
            {
                dataGridView.Rows.Add(
                    $"{month.Month:00}/{year}",
                    $"{month.Income:C}",
                    $"{month.Expenses:C}",
                    $"{month.Budget:C}",
                    $"{(month.Income - month.Expenses):C}",
                    $"{(month.Budget - month.Expenses):C}"
                );
            }

            // Add yearly totals
            decimal totalIncome = yearIncomes.Sum(i => i.Amount);
            decimal totalExpenses = yearExpenses.Sum(e => e.Amount);
            decimal totalBudget = yearBudgets.Sum(b => b.Amount);

            var totalLabel = new Label
            {
                Text = $"Year {year} Totals - Income: {totalIncome:C}, Expenses: {totalExpenses:C}, Budget: {totalBudget:C}, Net: {(totalIncome - totalExpenses):C}",
                Location = new Point(20, 450),
                AutoSize = true,
                Font = new Font(this.Font, FontStyle.Bold)
            };
            mainPanel.Controls.Add(totalLabel);
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
        public string ItemName { get; set; } = string.Empty;
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