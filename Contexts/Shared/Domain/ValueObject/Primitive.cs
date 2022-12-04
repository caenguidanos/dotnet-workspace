// <copyright file="Primitive.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Shared.Domain.ValueObject;

public abstract class ValueObject<TPrimitive>
{
    private readonly TPrimitive value;

    public ValueObject(TPrimitive value)
    {
        this.value = this.Validate(value);
    }

    public abstract TPrimitive Validate(TPrimitive value);

    public TPrimitive GetValue() => this.value;
}
