﻿using Selah.Domain.Data.Models.Transactions;

namespace Selah.Application.UnitTests.Transactions.TestHelpers;

public static class TransactionCategoryTestHelpers
{
    public static List<UserTransactionCategory> CreateCategories(bool createEmpty = false)
    {
        if (createEmpty) return new List<UserTransactionCategory>();

        var userId = Guid.NewGuid();
        var list = new List<UserTransactionCategory>
        {
            new()
            {
                Id = 1,
                UserId = 1,
                Name = "Test 1"
            },

            new()
            {
                Id = 2,
                UserId = 1,
                Name = "Test 2"
            },

            new()
            {
                Id = 3,
                UserId = 1,
                Name = "Test 3"
            },

            new()
            {
                Id = 4,
                UserId = 1,
                Name = "Test 4"
            }
        };
        return list;
    }

    public static List<UserTransactionCategory> CreateInvalidCategories()
    {
        var userId = Guid.NewGuid();
        var list = new List<UserTransactionCategory>
        {
            new()
            {
                Id = -1,
                UserId = 1,
                Name = "Test 1"
            },

            new()
            {
                Id = -2,
                UserId = 1,
                Name = "Test 1"
            }
        };
        return list;
    }

    public static List<LineItem> CreateLineItems()
    {
        var userId = Guid.NewGuid();
        var list = new List<LineItem>
        {
            new()
            {
                ItemizedAmount = 25,
                TransactionCategoryId = 1
            },
            new()
            {
                ItemizedAmount = 25,
                TransactionCategoryId = 2
            },

            new()
            {
                ItemizedAmount = 25,
                TransactionCategoryId = 3
            },

            new()
            {
                ItemizedAmount = 25,
                TransactionCategoryId = 4
            }
        };
        return list;
    }
}