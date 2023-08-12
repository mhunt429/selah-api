namespace Selah.Domain.Data.Models
{
    public record ErrorMessage
    {
        public string Key { get; set; }
        public string Message { get; set; }
    }
}
