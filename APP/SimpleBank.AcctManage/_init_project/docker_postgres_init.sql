
	
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

CREATE SCHEMA "SB-operational" AUTHORIZATION postgres;
COMMENT ON SCHEMA "SB-operational" IS 'All data related to account management operations.';

CREATE TABLE "SB-operational"."Users"(
	User_id            uuid PRIMARY KEY DEFAULT uuid_generate_v4 (),
	Created_at            TIMESTAMP NOT NULL DEFAULT (CURRENT_TIMESTAMP),
	Username                   TEXT NOT NULL,
	Email              VARCHAR(320) NOT NULL,
	Fullname                   TEXT NOT NULL,
	"Password"                BYTEA NOT NULL,
	Salt                      BYTEA NOT NULL,
	Password_changed_at TIMESTAMP NOT NULL DEFAULT (CURRENT_TIMESTAMP),
CONSTRAINT Check_MinLength_Users_FullName CHECK (LENGTH(FullName) >= 16),
CONSTRAINT Check_MinLength_Users_Username CHECK (LENGTH(Username) >= 8),
CONSTRAINT UNIQUE_Users_Email    UNIQUE (Email),
CONSTRAINT UNIQUE_Users_Username UNIQUE (Username)
);
    
CREATE TABLE "SB-operational"."Accounts"(
	Account_id uuid PRIMARY KEY DEFAULT uuid_generate_v4 (),
	Created_at       TIMESTAMP NOT NULL DEFAULT (CURRENT_TIMESTAMP),
	User_id               uuid NOT NULL,
	Balance            NUMERIC NOT NULL,
	Currency           CHAR(3) NOT NULL DEFAULT ('EUR'),
CONSTRAINT Check_Accounts_Balance_Positive CHECK (Balance > 0),
CONSTRAINT Account_User_fkey FOREIGN KEY (User_id)
    REFERENCES "SB-operational"."Users" (User_id) MATCH SIMPLE
    ON UPDATE NO ACTION ON DELETE NO ACTION,
CONSTRAINT Check_Accounts_Currency_Length CHECK (LENGTH(Currency) = 3)
);
    
CREATE TABLE "SB-operational"."Movements"(
	Movement_id       uuid PRIMARY KEY DEFAULT uuid_generate_v4 (),
	Created_at           TIMESTAMP NOT NULL DEFAULT (CURRENT_TIMESTAMP),
	Account_id                uuid NOT NULL,
	Amount                 NUMERIC NOT NULL,
CONSTRAINT Movements_Accounts_fkey FOREIGN KEY (Account_id)
    REFERENCES "SB-operational"."Accounts" (Account_id) MATCH SIMPLE
    ON UPDATE NO ACTION ON DELETE NO ACTION
);  
 
 

CREATE SCHEMA "SB-auth" AUTHORIZATION postgres;
COMMENT ON SCHEMA "SB-auth" IS 'All data related to account management authentication.';

CREATE TABLE "SB-auth"."UserTokens"(
	User_token_id           uuid PRIMARY KEY DEFAULT uuid_generate_v4 (),
	Created_at                 TIMESTAMP NOT NULL DEFAULT (CURRENT_TIMESTAMP),
	User_id                         uuid NOT NULL, 
	Access_token                    TEXT NOT NULL,
	Access_token_expires_at    TIMESTAMP NOT NULL,
	Refresh_token                   TEXT NOT NULL,
	Refresh_token_expires_at   TIMESTAMP NOT NULL
);



CREATE SCHEMA "SB-historic" AUTHORIZATION postgres;
COMMENT ON SCHEMA "SB-historic" IS 'All data related to account management history.';

CREATE TABLE "SB-historic"."Transfers"(
    Transfer_id       uuid PRIMARY KEY DEFAULT uuid_generate_v4 (),
	Created_at           TIMESTAMP NOT NULL DEFAULT (CURRENT_TIMESTAMP),
    Amount                 NUMERIC NOT NULL,
    From_account_id           uuid NOT NULL,
    To_account_id             uuid NOT NULL,
CONSTRAINT Check_Transfers_Amount_Positive            CHECK (Amount > 0),
CONSTRAINT Check_Transfers_FromAndToAccounts_NotEqual CHECK (From_account_id <> To_account_id)
);  


