CREATE TABLE recurring_transaction(
    id uuid PRIMARY KEY,
    user_id uuid REFERENCES app_user(id) ON DELETE CASCADE,
    frequency TEXT,
    notification_preference TEXT,
    upcoming_date TIMESTAMP,
    last_paid_date TIMESTAMP,
    location_name TEXT,
    send_reminder_notication boolean,
    category_id uuid REFERENCES user_transaction_category(id) ON DELETE CASCADE
);