using UnityEngine;

public class GameMasterStarter : MonoBehaviour {

    private void Start()
    {
        GameManager.Instance.GameMaster.StartGame();
    }
}