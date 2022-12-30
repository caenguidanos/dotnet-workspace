package Repository

import (
	"context"

	"dotnet-workspace/Contexts/IAM/Domain/Entity"
	"dotnet-workspace/Contexts/IAM/Domain/Repository"
)

type repository struct{}

func NewUserRepository() Repository.IUserRepository {
	return &repository{}
}

func (r repository) Save(_ context.Context, _ *Entity.User) error {
	return nil
}
