using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// 생성 담당
[SelectionBase]
public class NetworkCreator : MonoSingleton<NetworkCreator>
{

    [PunRPC]
    public void CreateHex(int createId) { 
        

    }

    public void CreatItem(int itemId) { 
        
    }

}
