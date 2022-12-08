-- Store::Product
CREATE TABLE IF NOT EXISTS ecommerce.product (
	title text NOT NULL,
	description text NOT NULL,
	price integer NOT NULL,
	status integer NOT NULL,
	PRIMARY KEY (id)
) INHERITS (ecommerce.timebase);

-- Store::Product::Indexes
CREATE INDEX IF NOT EXISTS product_by_index ON ecommerce.product (id);

-- Store::Event
CREATE TABLE IF NOT EXISTS ecommerce.event (
	name text NOT NULL,
	owner uuid NOT NULL,
	PRIMARY KEY (id)
) INHERITS (ecommerce.timebase);

-- Store::Event::Indexes
CREATE INDEX IF NOT EXISTS event_by_index ON ecommerce.event (id);
CREATE INDEX IF NOT EXISTS event_by_name ON ecommerce.event (name);
CREATE INDEX IF NOT EXISTS event_by_owner ON ecommerce.event (owner);
CREATE INDEX IF NOT EXISTS event_by_created_at ON ecommerce.event (created_at);