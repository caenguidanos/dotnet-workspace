package ValueObject

import (
	"errors"
)

type UserId struct {
	Value string
}

func NewUserId(value string, out *UserId) error {
	if err := ValidateUserId(value); err != nil {
		return err
	}
	out.Value = value
	return nil
}

func ValidateUserId(value string) error {
	if len(value) == 0 {
		return errors.New("invalid length")
	}

	return nil
}

type UserEmail struct {
	Value string
}

func NewUserEmail(value string, out *UserEmail) error {
	if err := ValidateUserEmail(value); err != nil {
		return err
	}
	out.Value = value
	return nil
}

func ValidateUserEmail(value string) error {
	if len(value) == 0 {
		return errors.New("invalid length")
	}

	return nil
}

type UserRole struct {
	Value string
}

func NewUserRole(value string, out *UserRole) error {
	if err := ValidateUserRole(value); err != nil {
		return err
	}
	out.Value = value
	return nil
}
func ValidateUserRole(value string) error {
	if len(value) == 0 {
		return errors.New("invalid length")
	}

	return nil
}
