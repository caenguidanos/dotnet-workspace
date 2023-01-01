package HttpHandler

import (
	"net/http"

	"dotnet-workspace/Contexts/IAM/Domain/Service"

	"github.com/labstack/echo/v4"
)

type HttpHandler struct {
	IHttpHandler
	userCreatorService *Service.IUserCreatorService
}

func NewCreateUserHttpHandler() HttpHandler {
	return HttpHandler{}
}

func (h *HttpHandler) AddSingleton(userCreatorService *Service.IUserCreatorService) {
	h.userCreatorService = userCreatorService
}

func (h *HttpHandler) Handle(c echo.Context) error {
	if err := (*h.userCreatorService).AddNewUser(c.Request().Context(), "email@gmail.com", "admin"); err != nil {
		return c.String(http.StatusBadRequest, http.StatusText(http.StatusBadRequest))
	}

	return c.String(http.StatusOK, http.StatusText(http.StatusOK))
}
