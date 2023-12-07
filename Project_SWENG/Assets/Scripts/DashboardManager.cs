using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DashboardManager : MonoBehaviour
{
    private PhotonView _photonView;
    public List<GameObject> totalDamageDashboard = new List<GameObject>(3);
    public List<Text> playerName = new List<Text>(3);
    public List<TextMeshProUGUI> damageText = new List<TextMeshProUGUI>(3);
    [SerializeField] private TextMeshProUGUI vicOrLoseText;
    [SerializeField] private Color winColor;
    [SerializeField] private Color loseColor;
    [SerializeField] private GameObject dashboard;
    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();
    }

    private void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            totalDamageDashboard[i].SetActive(false);
        }
    }

    public void ShowDashboardHandler(bool victory)
    {
        if (victory)
        {
            vicOrLoseText.color = winColor;
            vicOrLoseText.text = "VICTORY";
        }
        else
        {
            vicOrLoseText.color = loseColor;
            vicOrLoseText.text = "LOSE";
        }
        
        int index = 0;
        foreach (var player in PhotonNetwork.CurrentRoom.Players)
        {
            _photonView.RPC("ShowDashboard",RpcTarget.All,
                index,
                player.Value.NickName,
                GameManager.Instance.playerDmgDashboard[player.Value.NickName].ToString());
            
            index++;
        }
    }

    [PunRPC]
    private void ShowDashboard(int index, string playerNickName, string playerDmg)
    {
        dashboard.SetActive(true);
        totalDamageDashboard[index].SetActive(true);
        playerName[index].text = playerNickName;
        damageText[index].text = playerDmg;
    }
}
