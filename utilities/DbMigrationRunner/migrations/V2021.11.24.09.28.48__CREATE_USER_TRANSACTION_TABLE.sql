CREATE TABLE user_transaction
(
    id                       BIGINT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
    account_id               BIGINT UNSIGNED,

    user_id                  BIGINT REFERENCES app_user (id),
    transaction_amount       DECIMAL,
    transaction_date         TIMESTAMP,
    merchant_name            TEXT,
    transaction_name         TEXT,
    pending                  boolean,
    payment_method           TEXT,
    external_transaction_id  TEXT,
    recurring_transaction_id BIGINT UNSIGNED,
    FOREIGN KEY (recurring_transaction_id) REFERENCES user_transaction (id),
    FOREIGN KEY (account_id) REFERENCES user_bank_account (id)
);

/*
 --ROLLBACK
 --DELETE FROM flyway_schema_history WHERE script = 'V2021.11.24.09.28.48__CREATE_USER_TRANSACTION_TABLE.sql';
 --DROP TABLE user_transaction
 */