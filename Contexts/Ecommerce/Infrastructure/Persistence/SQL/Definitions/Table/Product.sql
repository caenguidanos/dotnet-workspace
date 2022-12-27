-- TYPE
CREATE DOMAIN product_title AS text;
CREATE DOMAIN product_description AS text;
CREATE DOMAIN product_price AS integer CHECK (value > 0);
CREATE TYPE product_status AS ENUM ('draft', 'published');

-- TABLE
CREATE TABLE product
(
    title       product_title UNIQUE NOT NULL,
    description product_description  NOT NULL,
    price       product_price        NOT NULL,
    status      product_status       NOT NULL,
    PRIMARY KEY (id)
) INHERITS (base);

-- INDEX
CREATE INDEX product_by_price ON product (price);
CREATE INDEX product_by_status ON product (status);

-- TRIGGERS
CREATE TRIGGER set_timestamp
    BEFORE UPDATE
    ON product
    FOR EACH ROW
    WHEN (OLD.* IS DISTINCT FROM NEW.*)
EXECUTE PROCEDURE update_timestamp();