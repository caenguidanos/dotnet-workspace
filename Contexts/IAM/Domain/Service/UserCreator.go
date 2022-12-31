package Service

import (
	"context"

	"dotnet-workspace/Contexts/IAM/Domain/Repository"
)

type IUserCreatorService interface {
	AddSingleton(userRepository *Repository.IUserRepository)

	AddNewUser(ctx context.Context, email string, role string) error
}
