using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;

namespace PersonalFinanceManager
{
    public partial class Form1 : Form
    {
        private static string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data\\accounts.json");
        private List<Account> accounts = new List<Account>();
        private Account currentAccount;

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
                Text = $"Current account: {currentAccount.AccountName}",
                Location = new Point(20, 20),
                AutoSize = true,
                Font = new Font(this.Font, FontStyle.Bold)
            };
            mainPanel.Controls.Add(accountLabel);

            var menuPanel = new Panel
            {
                Location = new Point(20, 60),
                Size = new Size(300, 300),
                BorderStyle = BorderStyle.FixedSingle
            };
            mainPanel.Controls.Add(menuPanel);

            var buttons = new[]
            {
                new Button { Text = "1. Add Income", Tag = "1" },
                new Button { Text = "2. Add Expense", Tag = "2" },
                new Button { Text = "3. Calculate Totals", Tag = "3" },
                new Button { Text = "4. Set Budgets", Tag = "4" },
                new Button { Text = "5. Show Total Expenses", Tag = "5" },
                new Button { Text = "6. Exit", Tag = "6" }
            };

            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].Size = new Size(250, 40);
                buttons[i].Location = new Point(25, 20 + i * 45);
                buttons[i].Click += MenuButton_Click;
                menuPanel.Controls.Add(buttons[i]);
            }
        }

        private void MenuButton_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            switch (button.Tag.ToString())
            {
                case "1":
                    ShowAddIncomeForm();
                    break;
                case "2":
                    ShowAddExpenseForm();
                    break;
                case "3":
                    ShowCalculateTotals();
                    break;
                case "4":
                    ShowSetBudgetsForm();
                    break;
                case "5":
                    ShowTotalExpensesForm();
                    break;
                case "6":
                    this.Close();
                    break;
            }
        }

        private void ShowAddIncomeForm()
        {
            mainPanel.Controls.Clear();

            var backButton = CreateBackButton();
            mainPanel.Controls.Add(backButton);

            var titleLabel = new Label
            {
                Text = "Add Income",
                Location = new Point(20, 20),
                AutoSize = true,
                Font = new Font(this.Font, FontStyle.Bold)
            };
            mainPanel.Controls.Add(titleLabel);

            var amountLabel = new Label
            {
                Text = "Enter the income amount:",
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

            var addButton = new Button
            {
                Text = "Add Income",
                Location = new Point(20, 130),
                Size = new Size(120, 30)
            };
            addButton.Click += (s, e) =>
            {
                if (decimal.TryParse(amountTextBox.Text, out decimal income))
                {
                    currentAccount.Incomes.Add(income);
                    SaveAccounts();
                    MessageBox.Show("Income added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ShowMainMenu();
                }
                else
                {
                    MessageBox.Show("Invalid input. Please enter a valid decimal value.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
            mainPanel.Controls.Add(addButton);
        }

        private void ShowAddExpenseForm()
        {
            mainPanel.Controls.Clear();

            var backButton = CreateBackButton();
            mainPanel.Controls.Add(backButton);

            var titleLabel = new Label
            {
                Text = "Add Expense",
                Location = new Point(20, 20),
                AutoSize = true,
                Font = new Font(this.Font, FontStyle.Bold)
            };
            mainPanel.Controls.Add(titleLabel);

            var itemLabel = new Label
            {
                Text = "Enter the item name:",
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
                Text = "Enter the expense amount:",
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

            var addButton = new Button
            {
                Text = "Add Expense",
                Location = new Point(20, 200),
                Size = new Size(120, 30)
            };
            addButton.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(itemTextBox.Text))
                {
                    MessageBox.Show("Item name cannot be empty", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (decimal.TryParse(amountTextBox.Text, out decimal expenseAmount))
                {
                    Expense expense = new Expense { ItemName = itemTextBox.Text, Amount = expenseAmount };
                    currentAccount.Expenses.Add(expense);
                    SaveAccounts();
                    MessageBox.Show("Expense added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ShowMainMenu();
                }
                else
                {
                    MessageBox.Show("Invalid input. Please enter a valid decimal value.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
            mainPanel.Controls.Add(addButton);
        }

        private void ShowCalculateTotals()
        {
            mainPanel.Controls.Clear();

            var backButton = CreateBackButton();
            mainPanel.Controls.Add(backButton);

            var titleLabel = new Label
            {
                Text = "Calculate Totals",
                Location = new Point(20, 20),
                AutoSize = true,
                Font = new Font(this.Font, FontStyle.Bold)
            };
            mainPanel.Controls.Add(titleLabel);

            decimal totalIncome = currentAccount.Incomes.Sum();
            decimal totalExpenses = currentAccount.Expenses.Sum(e => e.Amount);
            decimal netIncome = totalIncome - totalExpenses;

            var incomeLabel = new Label
            {
                Text = $"Total Income: {totalIncome:C}",
                Location = new Point(20, 60),
                AutoSize = true
            };
            mainPanel.Controls.Add(incomeLabel);

            var expensesLabel = new Label
            {
                Text = "Expenses:",
                Location = new Point(20, 90),
                AutoSize = true
            };
            mainPanel.Controls.Add(expensesLabel);

            var expensesList = new ListBox
            {
                Location = new Point(20, 120),
                Size = new Size(400, 150)
            };
            foreach (var expense in currentAccount.Expenses)
            {
                expensesList.Items.Add($"{expense.ItemName}: {expense.Amount:C}");
            }
            mainPanel.Controls.Add(expensesList);

            var totalExpensesLabel = new Label
            {
                Text = $"Total Expenses: {totalExpenses:C}",
                Location = new Point(20, 280),
                AutoSize = true
            };
            mainPanel.Controls.Add(totalExpensesLabel);

            var netIncomeLabel = new Label
            {
                Text = $"Net Income: {netIncome:C}",
                Location = new Point(20, 310),
                AutoSize = true,
                Font = new Font(this.Font, FontStyle.Bold)
            };
            mainPanel.Controls.Add(netIncomeLabel);
        }

        private void ShowSetBudgetsForm()
        {
            mainPanel.Controls.Clear();

            var backButton = CreateBackButton();
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
                Text = "Enter the budget amount:",
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
                Text = "Select a category:",
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
            categoryComboBox.Items.AddRange(new object[] { "Food", "Transportation", "Housing", "Entertainment" });
            mainPanel.Controls.Add(categoryComboBox);

            var setButton = new Button
            {
                Text = "Set Budget",
                Location = new Point(20, 200),
                Size = new Size(120, 30)
            };
            setButton.Click += (s, e) =>
            {
                if (decimal.TryParse(amountTextBox.Text, out decimal budget) && categoryComboBox.SelectedIndex >= 0)
                {
                    string category = categoryComboBox.SelectedItem.ToString();
                    MessageBox.Show($"Budget for {category} set to {budget:C}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ShowMainMenu();
                }
                else
                {
                    MessageBox.Show("Please enter a valid amount and select a category", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
            mainPanel.Controls.Add(setButton);
        }

        private void ShowTotalExpensesForm()
        {
            mainPanel.Controls.Clear();

            var backButton = CreateBackButton();
            mainPanel.Controls.Add(backButton);

            var titleLabel = new Label
            {
                Text = "Show Total Expenses",
                Location = new Point(20, 20),
                AutoSize = true,
                Font = new Font(this.Font, FontStyle.Bold)
            };
            mainPanel.Controls.Add(titleLabel);

            var filterLabel = new Label
            {
                Text = "Filter expenses by:",
                Location = new Point(20, 60),
                AutoSize = true
            };
            mainPanel.Controls.Add(filterLabel);

            var filterComboBox = new ComboBox
            {
                Location = new Point(20, 90),
                Size = new Size(200, 30),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            filterComboBox.Items.AddRange(new object[] { "No filter", "By amount range", "By item name" });
            mainPanel.Controls.Add(filterComboBox);

            var minLabel = new Label
            {
                Text = "Minimum amount:",
                Location = new Point(20, 130),
                AutoSize = true,
                Visible = false
            };
            mainPanel.Controls.Add(minLabel);

            var minTextBox = new TextBox
            {
                Location = new Point(20, 160),
                Size = new Size(100, 30),
                Visible = false
            };
            mainPanel.Controls.Add(minTextBox);

            var maxLabel = new Label
            {
                Text = "Maximum amount:",
                Location = new Point(140, 130),
                AutoSize = true,
                Visible = false
            };
            mainPanel.Controls.Add(maxLabel);

            var maxTextBox = new TextBox
            {
                Location = new Point(140, 160),
                Size = new Size(100, 30),
                Visible = false
            };
            mainPanel.Controls.Add(maxTextBox);

            var nameLabel = new Label
            {
                Text = "Item name:",
                Location = new Point(20, 130),
                AutoSize = true,
                Visible = false
            };
            mainPanel.Controls.Add(nameLabel);

            var nameTextBox = new TextBox
            {
                Location = new Point(20, 160),
                Size = new Size(200, 30),
                Visible = false
            };
            mainPanel.Controls.Add(nameTextBox);

            filterComboBox.SelectedIndexChanged += (s, e) =>
            {
                minLabel.Visible = filterComboBox.SelectedIndex == 1;
                minTextBox.Visible = filterComboBox.SelectedIndex == 1;
                maxLabel.Visible = filterComboBox.SelectedIndex == 1;
                maxTextBox.Visible = filterComboBox.SelectedIndex == 1;
                nameLabel.Visible = filterComboBox.SelectedIndex == 2;
                nameTextBox.Visible = filterComboBox.SelectedIndex == 2;
            };

            var sortLabel = new Label
            {
                Text = "Sort by:",
                Location = new Point(20, 200),
                AutoSize = true
            };
            mainPanel.Controls.Add(sortLabel);

            var sortComboBox = new ComboBox
            {
                Location = new Point(20, 230),
                Size = new Size(200, 30),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            sortComboBox.Items.AddRange(new object[] { "Name", "Amount" });
            mainPanel.Controls.Add(sortComboBox);

            var showButton = new Button
            {
                Text = "Show Expenses",
                Location = new Point(20, 270),
                Size = new Size(120, 30)
            };
            showButton.Click += (s, e) =>
            {
                IEnumerable<Expense> filteredExpenses = currentAccount.Expenses;

                switch (filterComboBox.SelectedIndex)
                {
                    case 1: // Amount range
                        if (decimal.TryParse(minTextBox.Text, out decimal min) && decimal.TryParse(maxTextBox.Text, out decimal max))
                        {
                            filteredExpenses = filteredExpenses.Where(e => e.Amount >= min && e.Amount <= max);
                        }
                        break;
                    case 2: // Item name
                        if (!string.IsNullOrWhiteSpace(nameTextBox.Text))
                        {
                            filteredExpenses = filteredExpenses.Where(e => e.ItemName.ToLower().Contains(nameTextBox.Text.ToLower()));
                        }
                        break;
                }

                switch (sortComboBox.SelectedIndex)
                {
                    case 0:
                        filteredExpenses = filteredExpenses.OrderBy(e => e.ItemName);
                        break;
                    case 1:
                        filteredExpenses = filteredExpenses.OrderBy(e => e.Amount);
                        break;
                }

                var resultListBox = new ListBox
                {
                    Location = new Point(20, 320),
                    Size = new Size(400, 200)
                };
                foreach (var expense in filteredExpenses)
                {
                    resultListBox.Items.Add($"{expense.ItemName}: {expense.Amount:C}");
                }
                mainPanel.Controls.Add(resultListBox);
            };
            mainPanel.Controls.Add(showButton);
        }

        private Button CreateBackButton()
        {
            var button = new Button
            {
                Text = "← Back",
                Location = new Point(20, 20),
                Size = new Size(80, 30)
            };
            button.Click += (s, e) => ShowMainMenu();
            return button;
        }
    }

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
}