CREATE EXTENSION "uuid-ossp";

CREATE TABLE base
(
    id             uuid                 DEFAULT uuid_generate_v4(),
    __created_at__ timestamptz NOT NULL DEFAULT NOW(),
    __updated_at__ timestamptz NOT NULL DEFAULT NOW()
);

CREATE FUNCTION update_timestamp()
    RETURNS TRIGGER AS
$$
BEGIN
    NEW.updated_at = NOW();
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;