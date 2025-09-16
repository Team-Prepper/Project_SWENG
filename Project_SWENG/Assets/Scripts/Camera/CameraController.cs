using UnityEngine;

namespace CameraSystem
{

    public class CameraController :
        MonoBehaviour, ICameraController
    {
        
        public void CamSetting(string key)
        {
            CameraManager.Instance.
                CameraSetting(transform, key);
        }

        public void SetSync(bool sync)
        {
            return;
        }
    }

}