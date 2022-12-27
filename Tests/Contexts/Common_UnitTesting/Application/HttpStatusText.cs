namespace Common_UnitTesting.Application;

[Category("HTTP")]
public sealed class HttpStatusTextUnitTest
{
    [Test]
    public void StatusText_I()
    {
        Assert.Multiple(() =>
        {
            Assert.That(HttpStatusText.From(HttpStatusCode.OK), Is.EqualTo("OK"));
            
            Assert.That(HttpStatusText.From(HttpStatusCode.Accepted), Is.EqualTo("Accepted"));
            
            Assert.That(HttpStatusText.From(HttpStatusCode.BadRequest), Is.EqualTo("BadRequest"));
        });
    }

    [Test]
    public void StatusText_II()
    {
        Assert.That(HttpStatusText.From(9877), Is.EqualTo(""));
    }
}