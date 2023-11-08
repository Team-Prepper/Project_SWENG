using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

public class SelectionManager : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;

    public LayerMask selectionMask;

    private void Awake()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    public void HandleClick(Vector3 mousePosition)
    {
        GameObject result;
        if (FindTarget(mousePosition, out result))
        {
            if (UnitSelected(result))
            {
                Debug.Log("Click Player");
                //OnUnitSelected?.Invoke(result);
                
                //UIManager.OpenGUI<GUI_ActionSelect>("ActionSelect").Set(result);
            }
            
        }
    }

    private bool UnitSelected(GameObject result)
    {
        return result.GetComponent<DicePoint>() != null;
    }

    private bool FindTarget(Vector3 mousePosition, out GameObject result)
    {
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(mousePosition);
        if (Physics.Raycast(ray, out hit, 100, selectionMask))
        {
            result = hit.collider.gameObject;
            return true;
        }
        result = null;
        return false;
    }
}
