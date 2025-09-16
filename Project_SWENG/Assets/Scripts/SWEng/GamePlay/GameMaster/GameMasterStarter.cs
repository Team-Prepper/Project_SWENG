using UnityEngine;

namespace SWEng.GamePlay
{
    public class GameMasterStarter : MonoBehaviour
    {
        private void Start()
        {
            GameManager.Instance.Master.StartLoad();
        }
    }
}