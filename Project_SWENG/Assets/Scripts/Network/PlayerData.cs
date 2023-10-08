using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helpers.Collections;

public class PlayerData : MonoBehaviour
{
	PlayerObject pObj;

	public Animator anim;
	public Renderer[] modelParts;
    public Transform uiPoint;

	public float animationBlending = 5f;


	private void Awake()
	{
		pObj = GetComponent<PlayerObject>();
		
	}




	
}
