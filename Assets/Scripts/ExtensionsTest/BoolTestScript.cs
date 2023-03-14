using UnityEngine;
using UnityEngine.UI;

public class BoolTestScript : MonoBehaviour
{
    // Update is called once per frame
    public void Update()
    {
		Image.gameObject.SetActive(Toggler);
    }

	public void Action()
	{
		Toggler.Toggle();
	}

	public bool Toggler;
	public Image Image;
}
