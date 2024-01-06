CREATE TABLE transaction_line_item
(
    id                      BIGINT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
    transaction_id          BIGINT UNSIGNED,
    transaction_category_id BIGINT UNSIGNED,
    itemized_amount         DECIMAL,
    is_app_category         BOOLEAN,
    FOREIGN KEY (transaction_id) REFERENCES user_transaction (id),
    FOREIGN KEY (transaction_category_id) REFERENCES user_transaction_category (id)
);
