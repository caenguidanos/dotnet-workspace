CREATE TABLE IF NOT EXISTS public.event (
	name text NOT NULL,
	owner uuid NOT NULL,
	PRIMARY KEY (id)
) INHERITS (public.basetime);

CREATE INDEX IF NOT EXISTS event_by_id ON public.event (id);
CREATE INDEX IF NOT EXISTS event_by_name ON public.event (name);
CREATE INDEX IF NOT EXISTS event_by_owner ON public.event (owner);
CREATE INDEX IF NOT EXISTS event_by_created_at ON public.event (created_at);