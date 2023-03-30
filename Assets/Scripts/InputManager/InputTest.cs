using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class InputTest : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if(InputManager.IsKeyDown(InputManager.Accept))
		{
			Debug.Log("PRESSED Accept");
		}
		if(InputManager.IsKeyDown(InputManager.Cancel))
		{
			Debug.Log("PRESSED Cancel");
		}
		if(InputManager.IsKeyDown(InputManager.Jump))
		{
			Debug.Log("PRESSED Jump");
		}
		if(InputManager.IsKeyDown(InputManager.Attack))
		{
			Debug.Log("PRESSED Attack");
		}

		if(InputManager.IsKeyUp(InputManager.Accept))
		{
			Debug.Log("RELEASED Accept");
		}
		if(InputManager.IsKeyUp(InputManager.Cancel))
		{
			Debug.Log("RELEASED Cancel");
		}
		if(InputManager.IsKeyUp(InputManager.Jump))
		{
			Debug.Log("RELEASED Jump");
		}
		if(InputManager.IsKeyUp(InputManager.Attack))
		{
			Debug.Log("RELEASED Attack");
		}

		if(InputManager.IsKeyHeld(InputManager.Accept))
		{
			Debug.Log("HELD Accept");
		}
		if(InputManager.IsKeyHeld(InputManager.Cancel))
		{
			Debug.Log("HELD Cancel");
		}
		if(InputManager.IsKeyHeld(InputManager.Jump))
		{
			Debug.Log("HELD Jump");
		}
		if(InputManager.IsKeyHeld(InputManager.Attack))
		{
			Debug.Log("HELD Attack");
		}

		//Debug.Log(InputManager.GetAxisValue(InputManager.MoveHorizontal));
		//Debug.Log(InputManager.GetAxisValue(InputManager.MoveVertical));
    }
}
