using EHTool;

public class GameManager : MonoSingleton<GameManager> {

    public INetwork Network { get; private set; }
    public IGameMaster GameMaster { get; private set; }

    public void SetGameMaster(IGameMaster gm) { GameMaster = gm; }

    protected override void OnCreate()
    {
        base.OnCreate();

        Network = gameObject.AddComponent<PhotonNet>();
    }

}