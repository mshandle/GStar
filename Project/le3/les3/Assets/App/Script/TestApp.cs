public class TestApp : Framework.GameApp
{
    public override void Awake()
    {
        base.Awake();
        var config = SetConfig();
        base.InitFrameWork(config);
    }

    private Framework.AppConfig SetConfig()
    {
        Framework.AppConfig config = new Framework.AppConfig();
        return config;
    }
}
