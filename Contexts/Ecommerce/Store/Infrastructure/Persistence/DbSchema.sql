-- Schema
CREATE SCHEMA IF NOT EXISTS ecommerce_store;

-- Extensions
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

-- Common::Table
CREATE TABLE IF NOT EXISTS ecommerce_store.base (
	id uuid DEFAULT uuid_generate_v4()
);
CREATE TABLE IF NOT EXISTS ecommerce_store.timebase (
	created_at timestamptz NOT NULL DEFAULT NOW(),
  	updated_at timestamptz NOT NULL DEFAULT NOW()
) INHERITS (ecommerce_store.base);

-- Common::Function
CREATE OR REPLACE FUNCTION ecommerce_store.update_timestamp()
RETURNS TRIGGER AS $$
BEGIN
  NEW.updated_at = NOW();
  RETURN NEW;
END;
$$ LANGUAGE plpgsql;

-- Store::Product
CREATE TABLE IF NOT EXISTS ecommerce_store.product (
	title text NOT NULL,
	description text NOT NULL,
	price integer NOT NULL,
	status integer NOT NULL,
	PRIMARY KEY (id)
) INHERITS (ecommerce_store.timebase);

-- Store::Product::Indexes
CREATE INDEX IF NOT EXISTS product_by_index ON ecommerce_store.product (id);

-- Store::Product::Triggers
CREATE OR REPLACE TRIGGER set_timestamp_to_product BEFORE UPDATE
ON ecommerce_store.product
FOR EACH ROW
WHEN (OLD.* IS DISTINCT FROM NEW.*)
EXECUTE PROCEDURE ecommerce_store.update_timestamp();

-- Store::Event
CREATE TABLE IF NOT EXISTS ecommerce_store.event (
	name text NOT NULL,
	owner uuid NOT NULL,
	PRIMARY KEY (id)
) INHERITS (ecommerce_store.timebase);

-- Store::Event::Indexes
CREATE INDEX IF NOT EXISTS event_by_index ON ecommerce_store.event (id);
CREATE INDEX IF NOT EXISTS event_by_name ON ecommerce_store.event (name);
CREATE INDEX IF NOT EXISTS event_by_owner ON ecommerce_store.event (owner);
CREATE INDEX IF NOT EXISTS event_by_created_at ON ecommerce_store.event (created_at);

-- Store::Event::Triggers
CREATE OR REPLACE TRIGGER set_timestamp_to_event BEFORE UPDATE
ON ecommerce_store.event
FOR EACH ROW
WHEN (OLD.* IS DISTINCT FROM NEW.*)
EXECUTE PROCEDURE ecommerce_store.update_timestamp();