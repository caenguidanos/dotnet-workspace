package Repository

import (
	"context"

	"dotnet-workspace/Contexts/IAM/Domain/Entity"
)

type IUserRepository interface {
	Save(ctx context.Context, user *Entity.User) error
}
