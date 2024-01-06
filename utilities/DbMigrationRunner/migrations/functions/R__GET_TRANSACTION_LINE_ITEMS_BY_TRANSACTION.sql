CREATE PROCEDURE get_transaction_line_items_by_transaction(
    IN transaction_id_param INT
)
BEGIN
SELECT tli.id                      AS id,
       tli.transaction_id          AS transaction_id,
       tli.transaction_category_id AS transaction_category_id,
       tli.itemized_amount         AS itemized_amount,
       trx.account_id              AS account_id,
       trx.user_id                 AS user_id,
       trx.transaction_amount      AS transaction_amount,
       trx.transaction_date        AS transaction_date,
       trx.merchant_name           AS merchant_name,
       trx.transaction_name        AS transaction_name,
       trx.pending                 AS pending
FROM transaction_line_item tli
         INNER JOIN user_transaction trx ON trx.id = tli.transaction_id
WHERE trx.id = transaction_id_param;
END

