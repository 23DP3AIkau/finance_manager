# Personal Finance Manager - User Guide

## Introduction

The Personal Finance Manager is a Windows desktop application designed to help users track their income, expenses, and budgets. This comprehensive tool provides financial insights through various reports and overviews, helping users make informed decisions about their personal finances.


**Technologies**

- **Language:** C#
- **UI:** Windows Forms .NET9.0
- **Data saving:** JSON
- **Additionally:** Git

## Bundled Release:

1) Download the [release](https://github.com/23DP3AIkau/finance_manager/releases/latest) or from [website](<https://23dp3aikau.github.io/finance_manager_website/>).
2) Extract **all** files.
3) Run FinanceManager.exe

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
   - Seven main function buttons:<br>
     1\. Income - Add income records<br>
     2\. Expense - Add expense records<br>
     3\. Set Budgets - Define monthly budgets<br>
     4\. Calculate Totals - View financial summaries<br>
     5\. Monthly Overview - Detailed monthly reports<br>
     6\. Yearly Overview - Annual financial summary<br>
     7\. Exit Program - Close the application

3. **Income/Expense/Budgets Entry Forms**
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
- From the Account Selection screen, click "Create new Account", enter a name for your account and finally click Create Account, a new account will appear in account list

#### Selecting an Account
- Choose an account in Account Selection screen and then click on "Select Account" and you will be taken to the main menu for that account.

#### Deleting an Account
- Choose an account in Account Selection screen and then click on "Delete Account" and the account will be deleted after you confirm it.

#### Recording Income
- Click on "Income" button and then you can choose an income category, then you will have to enter amount in $ and select the month and year, finally click "Add Income" to confirm.

#### Recording Expenses
- Click on "Expense" button and then you can choose an expense category, then you will have to enter amount in $ and select the month and year, finally click "Add Expense" to confirm.

### Setting Budgets
- Click on "Set Budget" button and then you can choose an expense category, then you will have to enter amount in $ and select the month and year, finally click "Set Budget" to confirm.

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

## Data Storage

All account data is saved automatically in a JSON file located at:
`FinanceManager\data\accounts.json`

The application handles all file operations automatically - no manual saving is required.
