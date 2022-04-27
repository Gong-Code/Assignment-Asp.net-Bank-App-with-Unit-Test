using System;
using System.Collections.Generic;
using BankStartWeb.Data;
using BankStartWeb.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BankWebTest.Services
{
    [TestClass]
    public class AccountServiceTests
    {
        private readonly ApplicationDbContext _context;
        private readonly AccountService _sut;

        public AccountServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "Test")
                .Options;
            _context = new ApplicationDbContext(options);
            _sut = new AccountService(_context);
        }

        [TestMethod]
        public void When_deposit_Amount_is_negative_amount_should_return_ok()
        {
            // Arrange
            _context.Accounts.Add(new Account
            {
                Balance = 1000,
                AccountType = "Personal",
                Created = DateTime.Now,
                Transactions = new List<Transaction>()
            });

            _context.Customers.Add(new Customer
            {
                Givenname = "Arnold",
                Surname = "Swats",
                Streetaddress = "Proteinroad 21b",
                Zipcode = "765 32",
                City = "Uppsala",
                Country = "Sweden",
                CountryCode = "SE",
                NationalId = "nationalTest",
                TelephoneCountryCode = 45,
                Telephone = "5646456234",
                EmailAddress = "arnold.swats@gmail.com",
                Birthday = DateTime.Now,
                Accounts = new List<Account>()

            });

            // Act
            _context.SaveChanges();

            var result = _sut.MakeDeposit(1, - 1000);

            // Assert
            Assert.AreEqual(IAccountService.ErrorCode.AmountIsNegative, result);

        }

        [TestMethod]
        public void When_withdrawal_is_to_low_amount_should_return_ok()
        {
            // Arrange
            _context.Accounts.Add(new Account
            {
                Balance = 1000,
                AccountType = "Personal",
                Created = DateTime.Now,
                Transactions = new List<Transaction>()
            });

            _context.Customers.Add(new Customer
            {
                Givenname = "Arnold",
                Surname = "Swats",
                Streetaddress = "Proteinroad 21b",
                Zipcode = "765 32",
                City = "Uppsala",
                Country = "Sweden",
                CountryCode = "SE",
                NationalId = "nationalTest",
                TelephoneCountryCode = 45,
                Telephone = "5646456234",
                EmailAddress = "arnold.swats@gmail.com",
                Birthday = DateTime.Now,
                Accounts = new List<Account>()

            });

            // Act
            _context.SaveChanges();

            var result = _sut.MakeWithdrawal(1, 2000);

            // Assert
            Assert.AreEqual(IAccountService.ErrorCode.BalanceIsToLow, result);

        }

        [TestMethod]
        public void When_transfer_is_insufficient_amount_should_return_ok()
        {
            // Arrange
            _context.Accounts.Add(new Account
            {
                Balance = 1000,
                AccountType = "Personal",
                Created = DateTime.Now,
                Transactions = new List<Transaction>()
            });

            _context.Accounts.Add(new Account
            {
                Balance = 1000,
                AccountType = "Checking",
                Created = DateTime.Now,
                Transactions = new List<Transaction>()
            });

            _context.Customers.Add(new Customer
            {
                Givenname = "Arnold",
                Surname = "Swats",
                Streetaddress = "Proteinroad 21b",
                Zipcode = "765 32",
                City = "Uppsala",
                Country = "Sweden",
                CountryCode = "SE",
                NationalId = "nationalTest",
                TelephoneCountryCode = 45,
                Telephone = "5646456234",
                EmailAddress = "arnold.swats@gmail.com",
                Birthday = DateTime.Now,
                Accounts = new List<Account>()

            });

            // Act
            _context.SaveChanges();

            var result = _sut.Transfer(1, 2, 2000);

            // Assert
            Assert.AreEqual(IAccountService.ErrorCode.InSufficientFunds, result);

        }

        [TestMethod]
        public void When_transfer_account_amount_is_negative_should_return_ok()
        {
            // Arrange
            _context.Accounts.Add(new Account
            {
                Balance = 5000,
                AccountType = "Personal",
                Created = DateTime.Now,
                Transactions = new List<Transaction>()
            });

            _context.Accounts.Add(new Account
            {
                Balance = 1000,
                AccountType = "Checking",
                Created = DateTime.Now,
                Transactions = new List<Transaction>()
            });

            _context.Customers.Add(new Customer
            {
                Givenname = "Arnold",
                Surname = "Swats",
                Streetaddress = "Proteinroad 21b",
                Zipcode = "765 32",
                City = "Uppsala",
                Country = "Sweden",
                CountryCode = "SE",
                NationalId = "nationalTest",
                TelephoneCountryCode = 45,
                Telephone = "5646456234",
                EmailAddress = "arnold.swats@gmail.com",
                Birthday = DateTime.Now,
                Accounts = new List<Account>()

            });

            // Act
            _context.SaveChanges();

            var result = _sut.Transfer(1, 2, -5000);

            // Assert

            Assert.AreEqual(IAccountService.ErrorCode.AmountIsNegative, result);

        }


    }
}
