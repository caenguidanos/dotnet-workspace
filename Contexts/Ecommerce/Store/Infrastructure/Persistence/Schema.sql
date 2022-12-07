-- Database
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

CREATE OR REPLACE FUNCTION update_timestamp()
RETURNS TRIGGER AS $$
BEGIN
  NEW.updated_at = NOW();
  RETURN NEW;
END;
$$ LANGUAGE plpgsql;

-- Store::Product
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

CREATE INDEX product_by_index ON product (id);

CREATE OR REPLACE TRIGGER set_timestamp_to_product BEFORE UPDATE
ON product
FOR EACH ROW
WHEN (OLD.* IS DISTINCT FROM NEW.*)
EXECUTE PROCEDURE update_timestamp();

-- Store::Event
CREATE TABLE IF NOT EXISTS event (
	id uuid DEFAULT uuid_generate_v4(),
	name VARCHAR(256) NOT NULL,
	owner uuid NOT NULL,
	created_at TIMESTAMPTZ NOT NULL DEFAULT NOW(),
  	updated_at TIMESTAMPTZ NOT NULL DEFAULT NOW(),
	PRIMARY KEY (id)
);

CREATE INDEX event_by_index ON event (id);
CREATE INDEX event_by_name ON event (name);
CREATE INDEX event_by_owner ON event (owner);
CREATE INDEX event_by_created_at ON event (created_at);

CREATE OR REPLACE TRIGGER set_timestamp_to_event BEFORE UPDATE
ON event
FOR EACH ROW
WHEN (OLD.* IS DISTINCT FROM NEW.*)
EXECUTE PROCEDURE update_timestamp();