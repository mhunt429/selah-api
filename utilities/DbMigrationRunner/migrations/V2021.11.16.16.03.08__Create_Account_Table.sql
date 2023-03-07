CREATE TABLE user_bank_account(
	id BIGSERIAL PRIMARY KEY,
	external_account_id TEXT,
	account_mask TEXT,
	account_name TEXT,
	available_balance DECIMAL,
	current_balance DECIMAL,
	user_id uuid references app_user(id),
	subtype TEXT
);


--ROLLBACK;
--DELETE FROM schema_version where script = 'V2021.06.06.18.56.43__Create_Account_Table.sql';
--DROP INDEX account_user_id_idx
--DROP TABLE account;
