# Casino-Bank Integration Project

## Overview

This .NET solution comprises two projects: **Casino** and **Bank**. The Casino project is a simulated online casino application, and the Bank project acts as a verification service for handling deposits and withdrawals. The communication between the Casino and Bank is established through HttpClient requests.

## Project Structure

- **Casino**: The main casino application.
- **Bank**: The verification service for handling deposit and withdrawal requests.

## Casino Project

### Features

1. **User Management:**
   - Users can register with the casino.
   - Users can log in to access their accounts.

2. **Account Operations:**
   - Users can deposit funds into their casino accounts.
   - Users can withdraw funds from their casino accounts.
   - Users can check their account balance.
   - Users can view their transaction history.

3. **Database:**
   - The Casino project includes a T-SQL database to store user information, account details, and transaction history.

### Setup and Configuration

1. **Database Configuration:**
   - The Casino project uses a T-SQL database. Ensure that the database connection string is configured in the `appsettings.json` file.

2. **HttpClient Configuration:**
   - The Casino project communicates with the Bank project using HttpClient requests. Ensure that the Bank API URL is correctly set in the `appsettings.json` file.

3. **Run the Application:**
   - Build and run the Casino project to start the casino application.

## Bank Project

### Features

1. **Verification Service:**
   - The Bank project acts as a verification service for handling deposit and withdrawal requests from the Casino.

### Setup and Configuration

1. **HttpClient Configuration:**
   - The Bank project exposes an API endpoint that the Casino project communicates with. Ensure that the API is correctly configured, and the URL is set in the `appsettings.json` file.

2. **Run the Application:**
   - Build and run the Bank project to start the verification service.

## Communication Between Casino and Bank

- The Casino project sends HTTP requests to the Bank project for deposit and withdrawal verification.
- Ensure that the HttpClient configuration in both projects is aligned.

## Example Request Flow

1. **User Deposits Funds:**
   - Casino sends a deposit request to the Bank for verification.
   - Bank verifies the request and responds to the Casino with new request.

2. **User Withdraws Funds:**
   - Casino sends a withdrawal request to the Bank for verification.
   - Bank verifies the request and responds to the Casino.

## Additional Notes

- Both projects should be running simultaneously for seamless communication.
- Check the logs and error handling for troubleshooting communication issues.

## Dependencies

- .NET Core
- Entity Framework Core
- HttpClient
- Dapper
