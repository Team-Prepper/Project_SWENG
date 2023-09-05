using UnityEngine;
using System.Collections.Generic;

public class ClickToMove : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;

    [SerializeField] private Unit player;
    
    private RaycastHit hit;
    [SerializeField] GameObject[] impactVfx;
    
    public float moveDistance;
    public LayerMask groundMask;
    
    public void HandleClick(Vector3 mousePosition)
    {
        Debug.Log("input step 2");
        List<Vector3> movePath = new List<Vector3>();
        Vector3 result;
        if (FindPos(mousePosition, out result))
        {
            movePath.Add(result);
            Debug.Log("FieldPos : " + result);
            player.MoveThroughPath(movePath);
        }
    }
    
    private bool FindPos(Vector3 mousePosition, out Vector3 result)
    {
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundMask))
        {
            if ((hit.point - player.transform.position).magnitude <= player.movementPoints)
            {
                result = hit.point;
                VFX(0,hit.point + hit.normal * 0.01f,1.5f);
                return true;
            }
            else
            {
                VFX(1,hit.point + hit.normal * 0.01f,1.5f);
            }
        }
        result = Vector3.zero;
        return false;
    }
    
    void VFX(int type, Vector3 pos, float impactVfxLifetime)
    {
        if (impactVfx.Length > 0)
        {
            GameObject impactVfxInstance = Instantiate(impactVfx[type]);
            impactVfxInstance.transform.position = pos;
            if (impactVfxLifetime > 0)
            {
                Destroy(impactVfxInstance, impactVfxLifetime);
            }
        }
    }
}