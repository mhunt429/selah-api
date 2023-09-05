using MediatR;
using Selah.Domain.Data.Models.Transactions;

namespace Selah.Application.Queries;

public class GetTransactionCategoryDetailQuery: IRequest<TransactionCategoryDetail>
{
    public string UserId { get; set; }
    
    public string CategoryId { get; set; }
    
    public long StartDate { get; set; }
    
    public long EndDate { get; set; }
}