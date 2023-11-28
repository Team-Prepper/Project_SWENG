using Character;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemyController : EnemyController
{
    public override void DieAct()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("YOU WIN");
            GameManager.Instance.GameEnd(true);
        }
    }
}
