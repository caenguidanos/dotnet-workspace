-- Configuration
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

CREATE OR REPLACE FUNCTION update_timestamp()
RETURNS TRIGGER AS $$
BEGIN
  NEW.updated_at = NOW();
  RETURN NEW;
END;
$$ LANGUAGE plpgsql;

-- Domain::Product
CREATE TABLE IF NOT EXISTS product (
	id uuid DEFAULT uuid_generate_v4(),
	title VARCHAR(256) NOT NULL,
	description VARCHAR(600) NOT NULL,
	price INT NOT NULL,
	status INT NOT NULL,
	created_at TIMESTAMPTZ NOT NULL DEFAULT NOW(),
  	updated_at TIMESTAMPTZ NOT NULL DEFAULT NOW(),
	PRIMARY KEY (id)
);

CREATE OR REPLACE TRIGGER set_timestamp_to_product BEFORE UPDATE
ON product
FOR EACH ROW
WHEN (OLD.* IS DISTINCT FROM NEW.*)
EXECUTE PROCEDURE update_timestamp();