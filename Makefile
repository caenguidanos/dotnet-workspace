check:
	dotnet clean
	dotnet restore
	dotnet format
	dotnet build
	dotnet test

infra-dev:
	docker-compose up

webapi-dev:
	dotnet watch --project ./Apps/MySaaS/Backend/Api/Api.csproj

test:
	dotnet test