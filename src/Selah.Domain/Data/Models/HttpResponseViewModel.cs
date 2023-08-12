using System.Collections.Generic;

namespace Selah.Domain.Data.Models
{
    //View Model for exposed endpoints
    public record HttpResponseViewModel<T>
    {
        public int StatusCode { get; set; }
        public List<T> Data { get; set; }
        public IEnumerable<ValidationError> Errors {get;set; }
    }
}   