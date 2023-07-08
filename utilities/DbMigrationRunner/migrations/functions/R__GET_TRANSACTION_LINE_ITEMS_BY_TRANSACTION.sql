CREATE or REPLACE function get_transaction_line_items_by_transaction (
  transaction_id_param int
) 
	returns table (
		id int,
		transaction_id int,
		transaction_category_id int,
		itemized_amount decimal,
		account_id int,
		user_id int,
		transaction_amount decimal,
		transaction_date timestamp with time zone ,
		merchant_name TEXT,
		transaction_name TEXT,
		pending boolean
	) 
	language plpgsql
as $$
begin
	return query 
		select
			tli.id AS id,
			tli.transaction_id AS transaction_id,
			tli.transaction_category_id AS transaction_category_id,
			tli.itemized_amount AS itemized_amount,
			trx.account_id AS account_id,
			trx.user_id AS user_id,
			trx.transaction_amount AS transaction_amount,
			trx.transaction_date AS transaction_date,
			trx.merchant_name AS merchant_name,
			trx.transaction_name AS transaction_name,
			trx.pending AS pending
			FROM transaction_line_item tli
			INNER JOIN user_transaction trx on trx.id = tli.transaction_id
		where
			trx.id = transaction_id_param;
end;$$