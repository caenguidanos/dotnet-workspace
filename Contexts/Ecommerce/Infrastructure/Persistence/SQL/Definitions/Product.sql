CREATE TABLE IF NOT EXISTS public.product (
	title text NOT NULL,
	description text NOT NULL,
	price integer NOT NULL,
	status integer NOT NULL,
	PRIMARY KEY (id)
) INHERITS (public.basetime);

CREATE INDEX IF NOT EXISTS product_by_id ON public.product (id);