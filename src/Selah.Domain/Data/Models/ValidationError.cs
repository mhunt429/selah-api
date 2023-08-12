namespace Selah.Domain.Data.Models
{
    public  class ValidationError
    {
        public string PropertyName { get; set; }
        public string ErrorMessage { get; set; }
        public string AttemtedValue { get; set; }
    }
}
