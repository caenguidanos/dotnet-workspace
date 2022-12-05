check:
	dotnet clean
	dotnet restore
	dotnet format
	dotnet build
	dotnet test

webapi:
	dotnet clean
	dotnet restore
	dotnet publish Apps/MySaaS/Backend/Api \
		-c Release \
		-o ./build/Apps/MySaas/Backend/Api \
		--no-restore
	./build/Apps/MySaas/Backend/Api/Api

webapi-watch:
	dotnet clean
	dotnet restore
	dotnet watch --project Apps/MySaaS/Backend/Api

infra:
	docker-compose up --force-reacreate