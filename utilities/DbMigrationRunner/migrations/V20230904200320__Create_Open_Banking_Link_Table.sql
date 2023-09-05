CREATE TABLE open_banking_link(
    request_id uuid PRIMARY KEY, --ASP.NET Core request ID
    user_id BIGSERIAL REFERENCES app_user(id) ON DELETE CASCADE NOT NULL,
    generated_timestamp TIMESTAMP,
    encrypted_access_token TEXT
);