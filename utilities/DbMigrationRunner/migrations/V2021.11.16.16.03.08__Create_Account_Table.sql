CREATE TABLE user_bank_account
(
    id                BIGINT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
    user_id           BIGINT UNSIGNED,
    account_mask      TEXT,
    account_name      TEXT,
    available_balance DECIMAL,
    current_balance   DECIMAL,
    subtype           TEXT,
    FOREIGN KEY (user_id) REFERENCES app_user (id)
);

/*

--ROLLBACK;
--DELETE FROM schema_version where script = 'V2021.06.06.18.56.43__Create_Account_Table.sql';
--DROP INDEX account_user_id_idx
--DROP TABLE account;
*/