using Photon.Pun;
using CameraSystem;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class PhotonCameraController :
    MonoBehaviourPun, ICameraController
{
    [SerializeField] private PhotonView _pv;
    private bool _sync = false;

    private void Start()
    {
        _pv = _pv != null ? _pv : GetComponent<PhotonView>();
    }

    public void SetSync(bool sync)
    {
        _sync = sync;
    }

    public void CamSetting(string key)
    {
        if (!_sync)
        {
            _CamSetting(key);
            return;
        }
        _pv.RPC(nameof(PunCamSetting), RpcTarget.Others, key);
    }

    [PunRPC]
    public void PunCamSetting(string key)
    {
        _CamSetting(key);
    }

    private void _CamSetting(string key)
    {
        CameraManager.Instance.
            CameraSetting(transform, key);

    }
}