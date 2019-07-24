using Framework;
public class TestApp : Framework.GameApp
{
    public static bool inited = false;

    public override void Awake()
    {
        base.Awake();
        var config = SetConfig();
        base.InitFrameWork(config);
        LoadConfig();
        ConfigeScenes();
        ConfigUIInfos();
        SceneMgr.Instance.LoadScene(LoginScene.Name);
    }

    private void LoadConfig()
    {
        LocalData.Instance.AddTable<monsterData>("csv/monster.csv");
        LocalData.Instance.AddTable<MonsterTriggerData>("csv/monstertrigger.csv");
    }

    private Framework.AppConfig SetConfig()
    {
        Framework.AppConfig config = new Framework.AppConfig();
        config.loadMode = ResourceLoadMode.ASSETDATA;
        return config;
    }

    private void ConfigeScenes()
    {
        SceneMgr.Instance.RegisterSceneInfo(new LoginScene());
        SceneMgr.Instance.RegisterSceneInfo(new MainScene());
    }

    private void ConfigUIInfos()
    {
        UIMgr.Instance.registerInfo(WindowsID.Login, new UIInfo("UI/prefab/Login/LoginView.prefab",true));
        UIMgr.Instance.registerInfo(WindowsID.Main, new UIInfo("UI/prefab/main/MainView.prefab"));
    }
}
