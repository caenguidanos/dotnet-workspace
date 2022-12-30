package Service

import "context"

type IUserCreatorService interface {
	AddNewUser(ctx context.Context, email string, role string) error
}
