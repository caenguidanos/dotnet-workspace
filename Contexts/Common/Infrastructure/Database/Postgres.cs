namespace Common.Infrastructure.Database;

public struct PostgresErrorCode
{
    public const string UniqueViolation = "23505";
    public const string CheckViolation = "23514";
}
