namespace Common_UnitTesting.Application;

[Category("JSON")]
public sealed class JsonUnitTest
{
    /// <summary>
    ///     Given a json string minified as default,
    ///         when create indented json string and minify on runtime,
    ///             then return the same value.
    /// </summary>
    [Test]
    public void Minify_I()
    {
        const string minifiedAtDefault = """"{"a":7,"b":{"c":true,"d":null},"e":89,"f":[7,null,false,56,{"uu":"uu"}]}"""";

        var minifiedAtRuntime = Json.MinifyString("""
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

        Assert.That(minifiedAtDefault, Is.EqualTo(minifiedAtRuntime));
    }
}