using Selah.Domain.Data.Models.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selah.Application.UnitTests.Transactions.TestHelpers
{
    public static class TransactionCategoryTestHelpers
    {
        public static List<UserTransactionCategory> CreateCategories(bool createEmpty = false)
        {
            if(createEmpty) return new List<UserTransactionCategory>();

            var userId = Guid.NewGuid();
            var list = new List<UserTransactionCategory>
            {
                new UserTransactionCategory
                {
                    Id = 1,
                    UserId = userId,
                    CategoryName = "Test 1"
                },

                new UserTransactionCategory
                {
                    Id = 2,
                    UserId = userId,
                    CategoryName = "Test 2"
                },

                new UserTransactionCategory
                {
                    Id = 3,
                    UserId = userId,
                    CategoryName = "Test 3"
                },

                new UserTransactionCategory
                {
                    Id = 4,
                    UserId = userId,
                    CategoryName = "Test 4"
                }
            };
            return list;
        }

        public static List<UserTransactionCategory> CreateInvalidCategories()
        {
            var userId = Guid.NewGuid();
            var list = new List<UserTransactionCategory>
            {
                new UserTransactionCategory
                {
                    Id = -1,
                    UserId = userId,
                    CategoryName = "Test 1"
                },

                new UserTransactionCategory
                {
                    Id = -2,
                    UserId = userId,
                    CategoryName = "Test 1"
                }
            };
            return list;
        }

        public static List<LineItem> CreateLineItems()
        {
            var userId = Guid.NewGuid();
            var list = new List<LineItem>
            {
                new LineItem
                {
                    ItemizedAmount = 1,
                    TransactionCategoryId = 1
                },
                 new LineItem
                {
                    ItemizedAmount = 1,
                    TransactionCategoryId = 2
                },

                   new LineItem
                {
                    ItemizedAmount = 1,
                    TransactionCategoryId = 3
                },

                     new LineItem
                {
                    ItemizedAmount = 1,
                    TransactionCategoryId = 4
                },
            };
            return list;
        }
    }
}
