CREATE TABLE income_statement_deduction
(
    id             BIGINT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
    statement_id   BIGINT UNSIGNED,
    deduction_name VARCHAR(30),
    amount         DECIMAL,
    FOREIGN KEY (statement_id) REFERENCES income_statement (id)
);

/*
ROLLBACK
DROP TABLE income_statment_deduction
DELETE FROM changelog where name ='V20230510205950__CREATE_INCOME_STATEMENT_DEDUCTION_TABLE.sql'
*/