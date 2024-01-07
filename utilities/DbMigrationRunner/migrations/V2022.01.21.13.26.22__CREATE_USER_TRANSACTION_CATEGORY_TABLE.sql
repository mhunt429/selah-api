CREATE TABLE user_transaction_category
(
    id            BIGINT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
    user_id       BIGINT UNSIGNED,
    name VARCHAR(100),
    FOREIGN KEY (user_id) REFERENCES app_user (id)
);
