CREATE TABLE todo_item(
    id BIGSERIAL PRIMARY KEY,
    user_id uuid REFERENCES app_user(id) ON DELETE CASCADE,
    recurring BOOLEAN,
    last_completed TIMESTAMP,
    frequency VARCHAR(16),
    deadline TIMESTAMP
);

/*
ROLLBACK
DROP INDEX todo_user_index;
DROP TABLE todo_item
*/