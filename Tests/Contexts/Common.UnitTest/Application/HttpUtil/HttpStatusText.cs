namespace Common.UnitTest.Application.HttpUtil;

using System.Net;

using Common.Application.HttpUtil;
using Common.Fixture.Application.Tests;

[Category(TestCategory.Unit)]
public sealed class HttpStatusTextTest
{
    [Test]
    public void GivenStatusTextGenerator_WhenPassDifferentValidStatusValues_ThenParseToString()
    {
        Assert.That(HttpStatusText.From(HttpStatusCode.OK), Is.EqualTo("OK"));
        Assert.That(HttpStatusText.From(HttpStatusCode.Accepted), Is.EqualTo("Accepted"));
        Assert.That(HttpStatusText.From(HttpStatusCode.BadRequest), Is.EqualTo("BadRequest"));
    }

    [Test]
    public void GivenStatusTextGenerator_WhenPassInvalidStatusValues_ThenReturnEmptyString()
    {
        Assert.That(HttpStatusText.From(9877), Is.EqualTo(""));
    }
}
