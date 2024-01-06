CREATE TABLE user_institution
(
    id                     BIGINT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
    user_id                BIGINT UNSIGNED,
    institution_id         TEXT,
    institution_name       TEXT,
    encrypted_access_token TEXT,
    FOREIGN KEY (user_id) REFERENCES app_user (id)
);

/*
--ROLLBACK;
--DELETE FROM changelog where script = 'V2021.11.16.15.58.27__CREATE_INSTITUTION_TABLE.sql';
--DROP TABLE user_institution;


 */