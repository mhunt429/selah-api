CREATE TABLE app_user (
	id uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
	email TEXT,
	user_name TEXT,
	password TEXT,
	first_name TEXT,
	last_name TEXT,
	date_created TIMESTAMP,
	UNIQUE(email),
	UNIQUE(user_name)
);


