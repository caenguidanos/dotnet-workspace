check:
	dotnet clean
	dotnet restore
	dotnet format analyzers
	dotnet build
	dotnet test

infra:
	docker-compose up