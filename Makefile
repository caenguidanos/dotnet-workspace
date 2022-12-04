ifneq (,$(wildcard ./.env))
    include .env
    export
endif

clean:
	dotnet clean

dev:
	dotnet watch --project api

start:
	dotnet run --project api