using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudGUI : MonoBehaviour
{
    public Animator loadingCloudAnim;
    [SerializeField] private GameObject[] loadingCloud;

    private void Awake()
    {

    }

    public void StartCloud(object sender, EventArgs e)
    {
        Invoke("StartCloudOff", 0.1f);
    }

    public void StartCloudOff()
    {
        loadingCloudAnim.SetTrigger("active");
        Invoke("SetActiveFalse", 0.9f);
    }

    private void SetActiveFalse()
    {
        loadingCloud[0].SetActive(false);
        loadingCloud[1].SetActive(false);
    }
}
