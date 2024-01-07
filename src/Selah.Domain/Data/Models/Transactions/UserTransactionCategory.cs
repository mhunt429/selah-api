using System.ComponentModel.DataAnnotations;

namespace Selah.Domain.Data.Models.Transactions
{
  public class UserTransactionCategory
  {
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; }
  }

  public class UserTransactionCategoryCreate
  {
    [Required]
    public int UserId { get; set; }
    
    [Required]
    public string Name { get; set; }
  }

  public class UserTransactionCategoryUpdate
  {
    public string Name { get; set; }
  }
}