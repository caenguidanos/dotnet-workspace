namespace Common.UnitTest.Domain;

using System.Globalization;

using Common.Domain;

public sealed class SchemaTestUnitTest
{
    [Test]
    public void GivenEmptyEntity_WhenEditTimeStampAttributes_ThenReflectChanges()
    {
        var entity = new EmptyEntity();

        Assert.That(entity.created_at, Is.EqualTo(default(DateTime)));
        Assert.That(entity.updated_at, Is.EqualTo(default(DateTime)));

        var createdAt = DateTime.Now;
        var updatedAt = DateTime.Now;

        entity.AddTimeStamp(createdAt, updatedAt);

        var locale = new CultureInfo("en-US");

        Assert.That(entity.created_at.ToString(locale), Is.EqualTo(createdAt.ToString(locale)));
        Assert.That(entity.updated_at.ToString(locale), Is.EqualTo(updatedAt.ToString(locale)));
    }

    private sealed record EmptyEntity : Schema<EmptyEntity, EmptyEntity>
    {
    }
}
