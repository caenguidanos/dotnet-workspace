check:
	dotnet clean
	dotnet restore
	dotnet format
	dotnet build
	dotnet test

infra:
	docker-compose up