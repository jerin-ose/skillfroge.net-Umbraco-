namespace Skillfrog.Web.Tests;

/// <summary>
/// Smoke test used to keep CI green by default.
/// During the session, temporarily change Assert.True(true) to Assert.True(false)
/// on a feature branch to demonstrate a failing pipeline.
/// </summary>
public class PipelineSmokeTests
{
    [Fact]
    public void Solution_IsReadyForCiDemo()
    {
        Assert.True(true);
    }
}
