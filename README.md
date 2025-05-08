# Personal Finance Manager - User Guide

## Introduction

The Personal Finance Manager is a Windows desktop application designed to help users track their income, expenses, and budgets. This comprehensive tool provides financial insights through various reports and overviews, helping users make informed decisions about their personal finances.


**Technologies**

- **Language:** C#
- **UI:** Windows Forms .NET9.0
- **Data saving:** JSON
- **Papildu:** Git


### Key Features:
- **Multiple Account Management**: Create and manage different financial accounts
- **Income Tracking**: Record different types of income (Active, Portfolio, Passive)
- **Expense Management**: Categorize and track expenses across multiple categories
- **Budget Planning**: Set monthly budgets for different expense categories
- **Financial Reports**: View monthly and yearly financial summaries
- **Data Analysis**: Compare actual spending against budgets
- **Data Persistence**: All data is saved automatically in JSON format

## User Interface Description

### Main Windows and Components

1. **Account Selection Screen**
   - List of existing accounts
   - Buttons to create, select, or delete accounts
   - Option to create a test account with sample data
   - Exit program button

2. **Main Menu**
   - Displays current account name
   - Seven main function buttons:
     1. Income - Add income records
     2. Expense - Add expense records
     3. Set Budgets - Define monthly budgets
     4. Calculate Totals - View financial summaries
     5. Monthly Overview - Detailed monthly reports
     6. Yearly Overview - Annual financial summary
     7. Exit Program - Close the application

3. **Income/Expense Entry Forms**
   - Dropdown to select category
   - Text field for amount
   - Month/Year selection
   - Add/Cancel buttons

4. **Financial Summary Views**
   - Filter options by month/year
   - Total income/expenses/net values
   - Category breakdowns
   - Budget comparisons

5. **Monthly/Yearly Overviews**
   - Tabbed interface showing incomes, expenses, and budgets
   - Detailed tables with totals
   - Delete functionality for individual records

## Function Descriptions

### Account Management

#### Creating a New Account
1. From the Account Selection screen, click "Create New Account"
2. Enter a name for your account
3. Click "Create Account"
4. The new account will appear in your account list

#### Selecting an Account
1. From the Account Selection screen, click on an account in the list
2. Click "Select Account"
3. You will be taken to the Main Menu for that account

#### Deleting an Account
1. From the Account Selection screen, select an account from the list
2. Click "Delete Account"
3. Confirm the deletion when prompted

### Recording Income

1. From the Main Menu, click "Income"
2. Select an income category from the dropdown:
   - Active Income (salary, wages)
   - Portfolio Income (investments)
   - Passive Income (rental, royalties)
   - Other
3. Enter the amount
4. Select the month and year
5. Click "Add Income"

### Recording Expenses

1. From the Main Menu, click "Expense"
2. Select an expense category from the dropdown:
   - Utilities, Groceries, Housing, Transportation, etc.
3. Enter the amount
4. Select the month and year
5. Click "Add Expense"

### Setting Budgets

1. From the Main Menu, click "Set Budgets"
2. Select a category from the dropdown
3. Enter the budget amount
4. Select the month and year
5. Click "Set Budget"

Note: Setting a budget for the same category and period will overwrite any existing budget.

### Viewing Financial Summaries

1. From the Main Menu, click "Calculate Totals"
2. Use the filters to view data for:
   - All time (default)
   - Specific month(s)
   - Specific year(s)
   - Combination of month and year
3. View the summary which includes:
   - Total income
   - Expenses by category
   - Net income

### Monthly Overview

1. From the Main Menu, click "Monthly Overview"
2. Select a month and year (defaults to current month/year)
3. View the detailed report which includes:
   - Summary totals
   - Tabbed views for incomes, expenses, and budgets
   - Budget vs. actual spending comparisons
4. To delete a record:
   - Select a row in any tab
   - Click "Delete Selected"
   - Confirm the deletion

### Yearly Overview

1. From the Main Menu, click "Yearly Overview"
2. Select a year (defaults to current year)
3. View the annual report which includes:
   - Monthly breakdown of income, expenses, and budgets
   - Monthly and annual totals
   - Difference between income and expenses
   - Budget vs. actual spending differences

### Using the Test Account

1. From the Account Selection screen, click "Create Test Account"
2. A pre-populated account with sample data will be created
3. Select this account to explore all features with example data

## Tips for Effective Use

- Create separate accounts for different purposes (e.g., Personal, Business)
- Regularly update your income and expense records
- Set realistic budgets based on your historical spending
- Use the monthly overview to identify spending patterns
- Compare yearly data to track financial progress over time
- Delete test accounts when you're ready to use real data

## Data Storage

All account data is saved automatically in a JSON file located at:
`[Application Folder]\data\accounts.json`

The application handles all file operations automatically - no manual saving is required.