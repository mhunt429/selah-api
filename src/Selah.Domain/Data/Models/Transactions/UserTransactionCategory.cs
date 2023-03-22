using System;
using System.ComponentModel.DataAnnotations;

namespace Selah.Domain.Data.Models.Transactions
{
  public class UserTransactionCategory
  {
    public long Id { get; set; }
    public Guid UserId { get; set; }
    public string CategoryName { get; set; }
  }

  public class UserTransactionCategoryCreate
  {
    [Required]
    public Guid UserId { get; set; }
    
    [Required]
    public string CategoryName { get; set; }
  }

  public class UserTransactionCategoryUpdate
  {
    public string CategoryName { get; set; }
  }
}