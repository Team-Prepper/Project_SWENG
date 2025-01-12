using System.Collections;
using UnityEngine;
using EHTool;

public class CameraManager : MonoSingleton<CameraManager>
{
    [SerializeField] private CameraGroup[] _groups;
    int _nowGroupIdx;

    [SerializeField] private Transform _target;

    public bool IsPlayerMove { get; set; }
    public bool IsWide { get; private set; }

    public void SetCamTarget(Transform target)
    {
        int nextGroupIdx = (1 + _nowGroupIdx) % _groups.Length;
        _groups[nextGroupIdx].SetCamTarget(target);
        _groups[_nowGroupIdx].OffTheGroup();
        _nowGroupIdx = nextGroupIdx;
        ConvertToCharacterCam();
        StartCoroutine(ResetMode());
    }

    IEnumerator ResetMode()
    {
        yield return new WaitForSeconds(1f);
    }

    private Vector3 MoveCamWithMouse()
    {
        Vector3 mouseCamMove = new Vector3();
        Vector3 worldPos = Camera.main.ScreenToViewportPoint(PlayerInputManager.Instance.mousePos);
        if (worldPos.x < 0.01f)
        {
            if(PlayerOutOfRange() % 2 != 0)
                mouseCamMove.x = -1;
        }
        if (worldPos.x > 0.99f)
        {
            if (PlayerOutOfRange() % 3 != 0)
                mouseCamMove.x = 1; 
        }
        if (worldPos.y < 0.01f)
        {
            if (PlayerOutOfRange() % 7 != 0)
                mouseCamMove.z = -1;
        }
        if (worldPos.y > 0.99f)
        {
            if (PlayerOutOfRange() % 5 != 0)
                mouseCamMove.z = 1;
        }
        return mouseCamMove;
    }

    private int PlayerOutOfRange()
    {
        int outValue = 1;
        float px = _target.transform.position.x;
        float pz = _target.transform.position.z;
        //float tx = characterCam.gameObject.transform.position.x;
        //float tz = characterCam.gameObject.transform.position.z;

        float xOffset = px;
        float zOffset = pz;

        if (xOffset > 12)
            outValue *= 2;
        if (xOffset < -12)
            outValue *= 3;

        if (zOffset > 17)
            outValue *= 7;
        if (zOffset < -1)
            outValue *= 5;

        return outValue;
    }// Camera script

    void LateUpdate()
    {
        if (_target == null) return;

        // Player는 싱글톤이기에 전역적으로 접근할 수 있습니다.
        Vector3 direction = (_target.position - transform.position).normalized;

        RaycastHit[] hits = Physics.RaycastAll(transform.position, direction, Mathf.Infinity,
                            1 << LayerMask.NameToLayer("EnvironmentObject"));

        for (int i = 0; i < hits.Length; i++)
        {
            TransparentObject[] obj = hits[i].transform.GetComponentsInChildren<TransparentObject>();

            for (int j = 0; j < obj.Length; j++)
            {
                obj[j]?.BecomeTransparent();
            }
        }
    }

    public void ConverTo(Transform target, string key)
    {
        SetCamTarget(target);

        switch (key)
        {
            case "Character":
                ConvertToCharacterCam();
                break;
            case "Battle":
                ConvertToBattleCam();
                break;
            case "Wide":
                ConvertToWideCam();
                break;
            default:
                ConvertToCharacterCam();
                break;
        }

    }

    public void ConvertToCharacterCam()
    {
        IsWide = false;
        _groups[_nowGroupIdx].ConvertToCharacterCam();
    }

    public void ConvertToBattleCam()
    {
        IsWide = false;
        _groups[_nowGroupIdx].ConvertToBattleCam();
    }

    public void ConvertToWideCam()
    {
        IsWide = true;
        _groups[_nowGroupIdx].ConvertToWideCam();
    }
}
