package Entity

import "dotnet-workspace/Contexts/IAM/Domain/ValueObject"

type User struct {
	Id    ValueObject.UserId
	Email ValueObject.UserEmail
	Role  ValueObject.UserRole
}

type UserPrimitives struct {
	Id    string `json:"id"`
	Email string `json:"email"`
	Role  string `json:"role"`
}

func NewUser(id string, email string, role string) (User, error) {
	var newUser User

	var newUserId ValueObject.UserId
	var newUserEmail ValueObject.UserEmail
	var newUserRole ValueObject.UserRole

	if err := ValueObject.NewUserId(id, &newUserId); err != nil {
		return newUser, err
	}

	if err := ValueObject.NewUserEmail(email, &newUserEmail); err != nil {
		return newUser, err
	}

	if err := ValueObject.NewUserRole(role, &newUserRole); err != nil {
		return newUser, err
	}

	newUser = User{
		Id:    newUserId,
		Email: newUserEmail,
		Role:  newUserRole,
	}

	return newUser, nil
}

func (u *User) ToPrimitives() UserPrimitives {
	return UserPrimitives{
		Id:    u.Id.Value,
		Email: u.Email.Value,
		Role:  u.Role.Value,
	}
}
