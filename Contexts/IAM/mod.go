package IAM

import (
	"github.com/labstack/echo/v4"

	"dotnet-workspace/Contexts/IAM/Application/Service"
	"dotnet-workspace/Contexts/IAM/Infrastructure/HttpHandler"
	"dotnet-workspace/Contexts/IAM/Infrastructure/Repository"
)

func RegisterModule(server *echo.Echo) {
	userRepository := Repository.NewUserRepository()

	userCreatorService := Service.NewCreatorService()
	userCreatorService.AddSingleton(&userRepository)

	createUserHttpHandler := HttpHandler.NewCreateUserHttpHandler()
	createUserHttpHandler.AddSingleton(&userCreatorService)

	router := server.Group("/user")
	router.GET("/", createUserHttpHandler.Handle)
}
