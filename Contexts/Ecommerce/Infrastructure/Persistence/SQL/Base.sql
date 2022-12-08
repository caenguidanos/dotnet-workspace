-- Schema
CREATE SCHEMA IF NOT EXISTS ecommerce;

-- Extensions
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

CREATE TABLE IF NOT EXISTS ecommerce.base (
	id uuid DEFAULT uuid_generate_v4()
);

CREATE TABLE IF NOT EXISTS ecommerce.timebase (
	created_at timestamptz NOT NULL DEFAULT NOW(),
  	updated_at timestamptz NOT NULL DEFAULT NOW()
) INHERITS (ecommerce.base);

CREATE OR REPLACE FUNCTION ecommerce.update_timestamp()
RETURNS TRIGGER AS $$
BEGIN
  NEW.updated_at = NOW();
  RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE TRIGGER set_timestamp BEFORE UPDATE
ON ecommerce.timebase
FOR EACH ROW
WHEN (OLD.* IS DISTINCT FROM NEW.*)
EXECUTE PROCEDURE ecommerce.update_timestamp();