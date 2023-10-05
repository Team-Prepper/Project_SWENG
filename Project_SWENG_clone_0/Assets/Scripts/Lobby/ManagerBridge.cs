using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerBridge : MonoBehaviour
{
	public void BackToMenu()
	{
		Debug.Log("BackToMenu"); // GM-> interfaceManager -> mainmenuScene
 	}

	public void QuitGame()
	{
		GameManager.QuitGame();
	}
}
