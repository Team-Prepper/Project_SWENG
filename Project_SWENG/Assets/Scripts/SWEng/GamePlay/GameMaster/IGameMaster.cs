using UnityEngine;

namespace SWEng.GamePlay {

    public interface IGameMaster
    {
        public enum Phase
        {
            Ready,
            Play,
            Boss,
            End
        }

        public ICharacter InstantiateCharacter(
            Vector3 position, Quaternion rotation);

        public GameObject InstantiateItem(Vector3 position);

        public bool StartGame();
        public void StartLoad();
        
        public void DisposeGame();
        public void GameEnd(bool victory);
    }
}