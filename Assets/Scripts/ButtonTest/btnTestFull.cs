using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class btnTestFull : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{
		btnFullMouse b = this.GetComponent<btnFullMouse>();
		b.onLeftClick.AddListener(onLClick);
		b.onRightClick.AddListener(onRClick);
		b.onMiddleClick.AddListener(onMClick);
	}

	// Update is called once per frame
	void Update()
	{

	}

	private void onLClick()
	{
		Debug.Log(this.name + " was pressed with mouseL");
	}

	private void onRClick()
	{
		Debug.Log(this.name + " was pressed with mouseR");
	}
	
	private void onMClick()
	{
		Debug.Log(this.name + " was pressed with mouseM");
	}

	private void sub()
	{
		Debug.Log(this.name + " was pressed with submit");
	}

}
