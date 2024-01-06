CREATE TABLE recurring_transaction
(
    id                       BIGINT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
    user_id                  BIGINT UNSIGNED,
    frequency                TEXT,
    notification_preference  TEXT,
    upcoming_date            TIMESTAMP,
    last_paid_date           TIMESTAMP,
    location_name            TEXT,
    send_reminder_notification BOOLEAN,
    category_id              BIGINT UNSIGNED,
    FOREIGN KEY (user_id) REFERENCES app_user (id) ON DELETE CASCADE,
    FOREIGN KEY (category_id) REFERENCES user_transaction_category (id) ON DELETE CASCADE
);
