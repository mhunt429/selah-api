using System;
using System.Collections.Generic;

namespace Selah.Domain.Data.Models
{
    //View Model for exposed endpoints
    public record HttpResponseViewModel<T>
    {
        public long? ApplicationUserId { get; set; }
        public int StatusCode { get; set; }
        public List<T> Data { get; set; } = new List<T>();
        public IEnumerable<ValidationError> Errors {get;set; }
    }
}   