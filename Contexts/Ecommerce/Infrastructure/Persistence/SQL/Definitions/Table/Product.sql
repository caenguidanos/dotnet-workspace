CREATE TYPE product_status AS ENUM ('closed', 'published');

CREATE TABLE product
(
    title       text UNIQUE    NOT NULL,
    description text           NOT NULL,
    price       integer        NOT NULL,
    status      product_status NOT NULL,
    PRIMARY KEY (id)
) INHERITS (base);

-- CONSTRAINTS
ALTER TABLE product
    ADD CONSTRAINT check_title_length
        CHECK (
                    length(title) >= 5 AND
                    length(title) <= 256
            );

ALTER TABLE product
    ADD CONSTRAINT check_description_length
        CHECK (
                    length(description) >= 5 AND
                    length(description) <= 600
            );

ALTER TABLE product
    ADD CONSTRAINT check_price_range
        CHECK (
                    price >= 100 AND
                    price <= 100000000
            );

-- INDEX
CREATE INDEX product_by_id ON product (id);
CREATE INDEX product_by_status ON product (status);

-- TRIGGERS
CREATE TRIGGER set_timestamp
    BEFORE UPDATE
    ON product
    FOR EACH ROW
    WHEN (OLD.* IS DISTINCT FROM NEW.*)
EXECUTE PROCEDURE update_timestamp();