CREATE TABLE app_user
(
    id           BIGINT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
    email        VARCHAR(255) NOT NULL,
    user_name    VARCHAR(255) NOT NULL,
    password     TEXT,
    first_name   VARCHAR(255),
    last_name    VARCHAR(255),
    date_created TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UNIQUE (email),
    UNIQUE (user_name)
);

/*
 --ROLLBACK
--DROP INDEX user_email_idx ON app_user
--DROP TABLE app_user
--DELETE FROM changelog where name = 'V2021.04.08.19.17.42__ Create_User_Table.sql'
 
 */

