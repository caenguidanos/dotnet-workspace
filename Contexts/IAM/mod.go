package IAM

import (
	"github.com/labstack/echo/v4"

	"dotnet-workspace/Contexts/IAM/Application/Service"
	"dotnet-workspace/Contexts/IAM/Infrastructure/HttpHandler"
	"dotnet-workspace/Contexts/IAM/Infrastructure/Repository"
)

func MapHttpHandlers(server *echo.Echo) {
	var userRepository = Repository.NewUserRepository()

	var userCreatorService = Service.NewCreatorService(userRepository)

	var createUserHttpHandler = HttpHandler.NewCreateUserHttpHandler(userCreatorService)

	router := server.Group("/user")
	router.GET("/", createUserHttpHandler.Handle)
}
