using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class btnRandom : MonoBehaviour
{

	[SerializeField] private TextMeshProUGUI randomInt;
	[SerializeField] private TextMeshProUGUI randomIntMax;
	[SerializeField] private TextMeshProUGUI randomSkew;

	private int i;
	[SerializeField] private int max1 = 50;
	private int imax;
	[SerializeField] private int min1 = 10;
	[SerializeField] private int max2 = 20;
	private float _skew;
	[SerializeField] private float min2 = 5;
	[SerializeField] private float max3 = 15;
	[SerializeField] private float peak = 7;
	// Start is called before the first frame update
	void Start()
	{
		this.GetComponent<Button>().onClick.AddListener(_newRandoms);
		_newRandoms();
	}

	// Update is called once per frame
	void Update()
	{
		randomInt.text = "Random int [0,"+max1+"]: " + i;
		randomIntMax.text = "Random int ["+min1+","+max2+"]: " + imax;
		randomSkew.text = "Random Skew ["+min2+","+max3+"]: " + _skew;
	}

	private void _newRandoms()
	{
		i = RNG.Instance.getInt(max1);
		imax = RNG.Instance.getInt(min1, max2);
		_skew = RNG.Instance.getNormalSkewed(min2, max3, peak);
	}
}
