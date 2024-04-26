using CharacterSystem;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemyController : EnemyController
{
    public override void DieAct()
    {
        Debug.Log("YOU WIN");
        GameManager.Instance.GameMaster.GameEnd(true);
    }
}
