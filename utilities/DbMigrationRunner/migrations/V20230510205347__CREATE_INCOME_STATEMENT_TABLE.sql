CREATE TABLE income_statement
(
    id                   BIGINT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
    user_id              BIGINT UNSIGNED,
    statement_start_date TIMESTAMP,
    statement_end_date   TIMESTAMP,
    total_pay            DECIMAL,
    FOREIGN KEY (user_id) REFERENCES app_user (id) ON DELETE CASCADE
);
/*
ROLLBACK 
DROP INDEX income_statement_user on income_statement;
DROP TABLE income_statement;
DELETE FROM changelog WHERE name = 'V20230510205347__CREATE_INCOME_STATEMENT_TABLE.sql';
*/