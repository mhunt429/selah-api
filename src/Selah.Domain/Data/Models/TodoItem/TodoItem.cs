using System;

namespace Selah.Domain.Data.Models.TodoItem
{
    public class TodoItem
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public bool Recurring { get; set; }

        public DateTime? LastCompleted { get; set; }

        public Frequency Frequency { get; set; }

        public DateTime? Deadline { get; set; }
    }

    public enum Frequency
    {
        OneTime = 0,
        Weekly = 1,
        BiWeekly = 2,
        Monthly = 3,
        Annually = 4,
        Other = 5
    }
}
