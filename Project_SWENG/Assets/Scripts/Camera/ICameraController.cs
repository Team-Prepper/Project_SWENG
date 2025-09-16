using UnityEngine;

namespace CameraSystem
{

    public interface ICameraController
    {
        public void SetSync(bool sync);
        public void CamSetting(string key);

    }

}