// <copyright file="Settings.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Ecommerce.Store.Infrastructure.Environment;

public class ConfigurationSettings
{
    private readonly IConfiguration configuration;

    public ConfigurationSettings(IConfiguration configuration)
    {
        this.configuration = configuration;

        this.PostgresConnection = this.configuration["Database:Postgres.Ecommerce.Store.Connection"] ?? throw new InvalidDataException();
    }

    required public string PostgresConnection { get; set; }
}
