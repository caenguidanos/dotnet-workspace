check:
	dotnet clean
	dotnet restore
	dotnet format
	dotnet build
	dotnet test

infra-start:
	docker-compose up

infra-down:
	docker-compose down

webapi-start:
	dotnet watch --project ./Apps/MySaaS/Backend/Api/Api.csproj

test-unit:
	dotnet test --filter Name~UnitTest

test-integration:
	dotnet test --filter Name~IntegrationTest

test-acceptance:
	dotnet test --filter Name~AcceptanceTest