using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CalendarTest : MonoBehaviour
{

	[SerializeField] TextMeshProUGUI _time;
	[SerializeField] TextMeshProUGUI _date;
	[SerializeField] TextMeshProUGUI _moondate;

	Clock c;
	Moon m;
	Calendar y;

	float delta = 0;
	[SerializeField] float multi = 1;
	long d = 0;
	long d2 = 0;
	private void Awake()
	{
		c = new Clock(100,100,100);
		m = new Moon(300,25);
		y = new Calendar(14,9,13,true,7,7,0);
	}

	// Start is called before the first frame update
	void Start()
	{
		d = 0;
	}

	// Update is called once per frame
	void Update()
	{
		delta += Time.deltaTime * multi;
		if(delta<0||delta>=int.MaxValue)
		{ delta = 0f; }

		if (delta >= 1)
		{
			d = c.passTime((long)delta);
			delta -= (long)delta;
		}
		if (d > 0)
		{
			m._passTime(d);
			y.passTime();
			d2 += d;
			d = 0;
		}
		if(d2>=17)
		{
			if(!m.ActiveCustomPhase)
			{
			m._overwriteMoon("Quaggan");
			}
			d2 = 0;
		}

		updateText();
	}


	private void updateText()
	{
		_time.text = c.printTime();
		_date.text = y.printDate();
		;
		_moondate.text = m.MoonDay + "/" + m.LengthMoonCycle + " | " + m.printMoonphase();
	}
}
