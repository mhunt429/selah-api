CREATE TABLE user_notification_prefence(
id uuid PRIMARY KEY,
user_id uuid REFERENCES app_user(id) ON DELETE CASCADE,
email boolean,
text boolean,
push boolean
);