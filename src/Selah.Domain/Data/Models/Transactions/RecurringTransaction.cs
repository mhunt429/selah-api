using System;
using System.Collections.Generic;

namespace Selah.Domain.Data.Models.Transactions
{
    public class RecurringTransaction
    {
        public DateTime UpcomingDate { get; set; }

        public DateTime LastPaidDate { get; set; }

        public string Location { get; set; }

        public Frequency Frequency { get; set; }
        public decimal Amount { get; set; }

        public Guid CategoryId { get; set; }

        /// <summary>
        ///If true, send notication 1 week, 3 days, 2 days and 1 day based on preference 
        /// </summary>
        public bool SendReminderNotification { get; set; }

        public NoticationPreference NoticationPreferences { get; set; }
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

    public enum NoticationPreference
    {
        None = 0,
        Email = 1,
        Text = 2,
        PushNotification = 3
    }
}
