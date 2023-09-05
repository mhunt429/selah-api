using System;

namespace Selah.Domain.Data.Models.Transactions;
/// <summary>
/// Transaction GetById result
/// </summary>
public class TransactionCategoryDetail
{
    public string Id { get; set; }
    
    public string Name { get; set; }
    
    public int Transactions { get; set; }
    
    public decimal Total { get; set; }
    
    public DateTime StartDate { get; set; }
    
    public DateTime EndTime { get; set; }
}

/// <summary>
/// Canonical mapping to database
/// </summary>
public class TransactionCategoryDetailSql
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public int Transactions { get; set; }
    
    public decimal Total { get; set; }
}
