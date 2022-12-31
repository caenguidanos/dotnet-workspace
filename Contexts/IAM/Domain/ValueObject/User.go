package ValueObject

import (
	"errors"
)

type UserId struct {
	Value string
}

func NewUserId(value string, id *UserId) error {
	if len(value) == 0 {
		return errors.New("invalid length")
	}

	id.Value = value
	return nil
}

type UserEmail struct {
	Value string
}

func NewUserEmail(value string, email *UserEmail) error {
	if len(value) == 0 {
		return errors.New("invalid length")
	}

	email.Value = value
	return nil
}

type UserRole struct {
	Value string
}

func NewUserRole(value string, role *UserRole) error {
	if len(value) == 0 {
		return errors.New("invalid length")
	}

	role.Value = value
	return nil
}
