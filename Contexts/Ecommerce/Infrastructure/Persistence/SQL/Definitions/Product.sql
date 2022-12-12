CREATE TABLE product (
	title text UNIQUE NOT NULL,
	description text NOT NULL,
	price integer NOT NULL,
	status integer NOT NULL,
	PRIMARY KEY (id),
	CHECK (
		length(title) >= 5 AND
		length(title) <= 256
	),
	CHECK (
		length(description) >= 5 AND
		length(description) <= 600
	),
	CHECK (
		price >= 100 AND
		price <= 100000000
	),
	CHECK (
		status = 0 OR -- CLOSED
		status = 1    -- PUBLISHED
	)
) INHERITS (base);

CREATE INDEX product_by_id ON product (id);

CREATE TRIGGER set_timestamp BEFORE UPDATE
ON product
FOR EACH ROW
WHEN (OLD.* IS DISTINCT FROM NEW.*)
EXECUTE PROCEDURE update_timestamp();