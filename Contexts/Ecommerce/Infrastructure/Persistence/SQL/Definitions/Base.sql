CREATE EXTENSION "uuid-ossp";

CREATE TABLE public.base (
	id uuid DEFAULT uuid_generate_v4()
);

CREATE TABLE public.basetime (
	created_at timestamptz NOT NULL DEFAULT NOW(),
  	updated_at timestamptz NOT NULL DEFAULT NOW()
) INHERITS (public.base);

CREATE FUNCTION public.update_timestamp()
RETURNS TRIGGER AS $$
BEGIN
  NEW.updated_at = NOW();
  RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER set_timestamp BEFORE UPDATE
ON public.basetime
FOR EACH ROW
WHEN (OLD.* IS DISTINCT FROM NEW.*)
EXECUTE PROCEDURE public.update_timestamp();