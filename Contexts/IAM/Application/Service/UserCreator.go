package Service

import (
	"context"

	"dotnet-workspace/Contexts/IAM/Domain/Entity"
	"dotnet-workspace/Contexts/IAM/Domain/Repository"
	"dotnet-workspace/Contexts/IAM/Domain/Service"

	"github.com/google/uuid"
)

type service struct {
	repository Repository.IUserRepository
}

func NewCreatorService(repository Repository.IUserRepository) Service.IUserCreatorService {
	return &service{repository}
}

func (s *service) AddNewUser(ctx context.Context, email string, role string) error {
	user, err := Entity.NewUser(uuid.NewString(), email, role)
	if err != nil {
		return err
	}

	if err := s.repository.Save(ctx, &user); err != nil {
		return err
	}

	return nil
}
