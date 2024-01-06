CREATE TABLE user_transaction_category
(
    id            BIGINT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
    user_id       BIGINT UNSIGNED,
    category_name VARCHAR(100),
    symbol        TEXT,
    FOREIGN KEY (user_id) REFERENCES app_user (id)
);
