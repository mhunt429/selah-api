ALTER TABLE user_bank_account ADD COLUMN institution_id uuid REFERENCES user_institution(id);
