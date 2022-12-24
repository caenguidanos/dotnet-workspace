namespace Common_UnitTesting.Application;

public sealed class JsonUnitTest
{
    [Test]
    public void GivenStringAsJson_WhenMinify_ThenReturnCoincidence()
    {
        var s0 = """"{"a":7,"b":{"c":true,"d":null},"e":89,"f":[7,null,false,56,{"uu":"uu"}]}"""";
        
        var s1 = Json.MinifyString("""
            {
                "a": 7,
                "b": {
                    "c": true,
                    "d": null
                },
                "e": 89,
                "f": [
                    7,
                    null,
                    false,
                    56,
                    {
                        "uu": "uu"
                    }
                ]
            }
        """);
        
        Assert.That(s0, Is.EqualTo(s1));
    }
}