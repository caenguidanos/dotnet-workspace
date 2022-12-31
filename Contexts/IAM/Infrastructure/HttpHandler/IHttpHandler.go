package HttpHandler

import (
	"github.com/labstack/echo/v4"
)

type IHttpHandler interface {
	Handle(c echo.Context) error
}
