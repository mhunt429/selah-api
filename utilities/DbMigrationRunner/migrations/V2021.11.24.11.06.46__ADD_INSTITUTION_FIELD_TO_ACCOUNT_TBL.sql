ALTER TABLE user_bank_account
    ADD COLUMN institution_id BIGINT UNSIGNED,
ADD CONSTRAINT fk_institution_id
    FOREIGN KEY (institution_id)
    REFERENCES user_institution (id);
