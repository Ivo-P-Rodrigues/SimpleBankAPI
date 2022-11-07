
	
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

CREATE SCHEMA "SB-operational" AUTHORIZATION postgres;
COMMENT ON SCHEMA "SB-operational" IS 'All data related to account management operations.';

CREATE TABLE "SB-operational"."Users"(
	User_id            uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
	Created_at          TIMESTAMPTZ NOT NULL DEFAULT (clock_timestamp()),
	Username            VARCHAR(12) NOT NULL,
	Email              VARCHAR(320) NOT NULL,
	Fullname           VARCHAR(100) NOT NULL,
	"Password"                BYTEA NOT NULL,
	Salt                      BYTEA NOT NULL,
	Password_changed_at TIMESTAMPTZ NOT NULL DEFAULT (clock_timestamp()),
CONSTRAINT Check_Length_Users_FullName CHECK (LENGTH(FullName) >= 16 AND LENGTH(FullName) <= 100),
CONSTRAINT Check_Length_Users_Username CHECK (LENGTH(Username) >= 5  AND LENGTH(Username) <= 12),
CONSTRAINT UNIQUE_Users_Email    UNIQUE (Email),
CONSTRAINT UNIQUE_Users_Username UNIQUE (Username)
);
    
CREATE TABLE "SB-operational"."Accounts"(
	Account_id    uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
	Created_at     TIMESTAMPTZ NOT NULL DEFAULT (clock_timestamp()),
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
	Created_at         TIMESTAMPTZ NOT NULL DEFAULT (clock_timestamp()),
	Account_id                uuid NOT NULL,
	Amount                 NUMERIC NOT NULL,
CONSTRAINT Movements_Accounts_fkey FOREIGN KEY (Account_id)
    REFERENCES "SB-operational"."Accounts" (Account_id) MATCH SIMPLE
    ON UPDATE NO ACTION ON DELETE NO ACTION
);  

CREATE TABLE "SB-operational"."AccountDocs"(
	Account_doc_id    uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
	Account_id                uuid NOT NULL,
	Created_at         TIMESTAMPTZ NOT NULL DEFAULT (clock_timestamp()),
	Name               VARCHAR(40) NOT NULL,
	Doc_type           VARCHAR(20) NOT NULL,
	Content                  BYTEA NOT NULL,
CONSTRAINT Check_Length_AccountDocs_Name CHECK (LENGTH(Name) >= 5 AND LENGTH(Name) <= 40),
CONSTRAINT AccountDocs_Accounts_fkey FOREIGN KEY (Account_id)
    REFERENCES "SB-operational"."Accounts" (Account_id) MATCH SIMPLE
    ON UPDATE NO ACTION ON DELETE NO ACTION
);  
 


CREATE SCHEMA "SB-auth" AUTHORIZATION postgres;
COMMENT ON SCHEMA "SB-auth" IS 'All data related to account management authentication.';

CREATE TABLE "SB-auth"."UserTokens"(
	User_token_id           uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
	Created_at               TIMESTAMPTZ NOT NULL DEFAULT (clock_timestamp()),
	User_id                         uuid NOT NULL, 
	Access_token                    TEXT NOT NULL,
	Access_token_expires_at  TIMESTAMPTZ NOT NULL,
	Refresh_token                   TEXT NOT NULL,
	Refresh_token_expires_at TIMESTAMPTZ NOT NULL
);



CREATE SCHEMA "SB-historic" AUTHORIZATION postgres;
COMMENT ON SCHEMA "SB-historic" IS 'All data related to account management history.';

CREATE TABLE "SB-historic"."Transfers"(
    Transfer_id       uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
	Created_at         TIMESTAMPTZ NOT NULL DEFAULT (clock_timestamp()),
    Amount                 NUMERIC NOT NULL,
    From_account_id           uuid NOT NULL,
    To_account_id             uuid NOT NULL,
CONSTRAINT Check_Transfers_Amount_Positive            CHECK (Amount > 0),
CONSTRAINT Check_Transfers_FromAndToAccounts_NotEqual CHECK (From_account_id <> To_account_id)
);  


