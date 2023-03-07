CREATE TABLE user_authorized_app
(
	id uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
	user_id uuid references app_user(id),
	application_name TEXT,
	encrypted_access_token TEXT,
	encrypted_refresh_token TEXT,
	access_token_expiration_ts TIMESTAMP,
	refresh_token_expiration_ts TIMESTAMP,
	created_ts TIMESTAMP,
	updated_ts TIMESTAMP 
);