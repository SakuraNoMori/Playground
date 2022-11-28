using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class btnTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		btnRightClick b = this.GetComponent<btnRightClick>();
		b.onLClick.AddListener(onLClick);
		b.onRClick.AddListener(onRClick);
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

}
