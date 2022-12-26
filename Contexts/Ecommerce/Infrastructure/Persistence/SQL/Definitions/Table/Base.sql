CREATE TABLE base
(
    id             uuid        NOT NULL,
    __created_at__ timestamptz NOT NULL DEFAULT NOW(),
    __updated_at__ timestamptz NOT NULL DEFAULT NOW()
);

CREATE FUNCTION update_timestamp()
    RETURNS TRIGGER AS
$$
BEGIN
    NEW.__updated_at__ = NOW();
    RETURN NEW;
END;
$$
    LANGUAGE plpgsql;