namespace Common_UnitTesting.Domain;

public sealed class SchemaUnitTest
{
    [Test]
    public void GivenEmptyEntity_WhenEditTimeStampAttributes_ThenReflectChanges()
    {
        var entity = new EmptyEntity();

        Assert.Multiple(() =>
        {
            Assert.That(entity.CreatedAd, Is.EqualTo(default(DateTime)));
            Assert.That(entity.UpdatedAt, Is.EqualTo(default(DateTime)));
        });

        var createdAt = DateTime.Now;
        var updatedAt = DateTime.Now;

        entity.AddTimeStamp(createdAt, updatedAt);

        var locale = new CultureInfo("en-US");

        Assert.Multiple(() =>
        {
            Assert.That(entity.CreatedAd.ToString(locale), Is.EqualTo(createdAt.ToString(locale)));
            Assert.That(entity.UpdatedAt.ToString(locale), Is.EqualTo(updatedAt.ToString(locale)));
        });
    }

    private sealed record EmptyEntity : Schema<EmptyEntity>;
}