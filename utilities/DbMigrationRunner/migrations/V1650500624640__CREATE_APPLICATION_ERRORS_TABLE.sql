CREATE TABLE application_error (
	id uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
	request_id uuid,
	application_user_id BIGINT,
	request_ts TIMESTAMP,
	error_status INT,
	error_message TEXT,
	stack_trace TEXT
);