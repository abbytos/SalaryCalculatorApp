# Salary Calculator App

## Overview

The Salary Calculator App is a console application designed to calculate and display a detailed salary breakdown for users based on their gross salary package and pay frequency. It computes various deductions such as income tax, Medicare levy, and budget repair levy, and presents the net income and pay packet amount.

## Features

- **Calculate Salary Breakdown:** Computes superannuation, taxable income, income tax, Medicare levy, budget repair levy, net income, and pay packet amount.
- **Flexible Pay Frequency:** Supports weekly, fortnightly, and monthly pay frequencies.
- **Error Handling:** Includes comprehensive error handling for invalid inputs and unexpected errors.

## Installation

1. Clone the repository:

    ```bash
    git clone https://github.com/yourusername/salary-calculator-app.git
    ```

2. Navigate to the project directory:

    ```bash
    cd salary-calculator-app
    ```

3. Build the project using your preferred .NET tool (e.g., Visual Studio, Visual Studio Code, or the .NET CLI):

    ```bash
    dotnet build
    ```

4. Run the application:

    ```bash
    dotnet run
    ```

## Usage

1. **Enter Gross Salary Package:** Input your gross salary package when prompted.
2. **Select Pay Frequency:** Choose your pay frequency (W for weekly, F for fortnightly, M for monthly) when prompted.
3. **View Salary Breakdown:** The application will display the calculated salary breakdown, including gross package, superannuation, taxable income, deductions, net income, and pay packet amount.

## Components

- **Interfaces:**
  - `IDeductionStrategy`: Defines the contract for deduction strategies.
  - `ISalaryCalculator`: Defines the contract for salary calculation.

- **Strategies:**
  - `IncomeTaxStrategy`: Calculates income tax based on taxable income.
  - `MedicareLevyStrategy`: Calculates Medicare levy based on taxable income.
  - `BudgetRepairLevyStrategy`: Calculates budget repair levy based on taxable income.

- **Services:**
  - `SalaryCalculator`: Implements the `ISalaryCalculator` interface to compute the salary breakdown.

- **Models:**
  - `SalaryBreakdown`: Represents the detailed salary breakdown.
  - `PayFrequency`: Enum for pay frequency options.

## Contact

For any questions or inquiries, please contact Albena Roshelova - [aroshelova.tech@gmail.com](mailto:aroshelova.tech@gmail.com).

