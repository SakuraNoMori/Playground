using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class btnCompare : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		Button b=this.GetComponent<Button>();
		b.onClick.AddListener(onLClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void onLClick()
	{
		Debug.Log(this.name+" was pressed with mouseL");
	}
}
