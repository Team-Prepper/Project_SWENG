using EHTool;

public class GameManager : MonoSingleton<GameManager> {
    
    public IGameMaster GameMaster { get; private set; }

    public void SetGameMaster(IGameMaster gm) { GameMaster = gm; }

}