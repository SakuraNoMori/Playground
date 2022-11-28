
public class Moon
{
	// List of moonphases
	public enum Moonphases { eFullMoon, eWaningMoon, eWaningHalfmoon, eNewMoon, eWaxingMoon, eWaxingHalfmoon }

	private Moonphases _phase;  // Current moonphase
	public Moonphases Phase{ get => _phase; }
	private readonly long _lengthMooncycle;   // Duration of a full Mooncycle from one Full Moon to the next
	public long LengthMoonCycle { get => _lengthMooncycle; }
	private long _dayMoon;   // Day within the Mooncycle
	public long MoonDay { get => _dayMoon; }
	private string _nameCustomPhase;    // Name of custom moonphase, overwrites the next full moon
	private bool _pendingCustomPhase;   // Indicates that next full moon is a custom moon and has to be overwritten
	private bool _activeCustomPhase; // Indicates that custom moon is active
	public bool ActiveCustomPhase { get => _activeCustomPhase; }

	private float _phaseProgress => _dayMoon / (float)_lengthMooncycle;

	/// <summary>
	/// Constructs a moon with a custom Mooncycle
	/// </summary>
	/// <param name="Duration">Length of mooncycle, defaults to 28</param>
	/// <param name="Day">Current moonday, defaults to 1</param>
	public Moon(long Duration = 28, long Day = 1)
	{
		_lengthMooncycle = Duration <= 0 ? 28 : Duration;   //sets duration

		// Clamp _dayMoon between 0 and Duration
		if (Day < 1)
		{
			_dayMoon = 1;
		}
		else if (Day > Duration)
		{
			_dayMoon = Day % Duration;
		}
		else
		{
			_dayMoon = Day;
		}

		_setMoonphase();    //updates moonphase to show correct phase
	}

	/// <summary>
	/// Used to progress the moontime 
	/// </summary>
	/// <param name="days">Number of passed days</param>
	public void _passTime(long days)
	{
		_dayMoon += days;                  //increasing Time_Moon by the number of passed days

		// Mooncycle restarts as often as needed
		if (_dayMoon > _lengthMooncycle)
		{
			_dayMoon %= _lengthMooncycle;

			// Check whether custom moonphase was skipped
			if(_phaseProgress > 0.125)
			{
				_pendingCustomPhase = false;
			}
		}

		_setMoonphase();    //updating Moonphase
	}

	/// <summary>
	/// Changes moonphase according to Time_Moon, called everytime add_Time_Moon() is used
	/// </summary>
	private void _setMoonphase()
	{
		float Phase = _phaseProgress;    // Variable to determine the current moonphase; factors are based on the 8 moonphases: (1/8)*x
		if (Phase >= 0 && Phase <= 0.125)
		{
			_phase = Moonphases.eFullMoon;
			if (_pendingCustomPhase)
			{
				// Activate custom moonphase
				_activeCustomPhase = true;
				_pendingCustomPhase = false;
			}
		}
		else if (Phase > 0.125 && Phase <= 0.25)
		{
			_phase = Moonphases.eWaningMoon;
		}
		else if (Phase > 0.25 && Phase <= 0.375)
		{
			_phase = Moonphases.eWaningHalfmoon;
		}
		else if (Phase > 0.375 && Phase <= 0.5)
		{
			_phase = Moonphases.eWaningMoon;
		}
		else if (Phase > 0.5 && Phase <= 0.625)
		{
			_phase = Moonphases.eNewMoon;
		}
		else if (Phase > 0.625 && Phase <= 0.75)
		{
			_phase = Moonphases.eWaxingMoon;
		}
		else if (Phase > 0.75 && Phase <= 0.875)
		{
			_phase = Moonphases.eWaxingHalfmoon;
		}
		else if (Phase > 0.875 && Phase <= 1)
		{
			_phase = Moonphases.eWaxingMoon;
		}

		// Reset _activeCustomPhase
		if (_activeCustomPhase && _phase != Moonphases.eFullMoon)
		{
			_activeCustomPhase = false;
		}
	}

	/// <summary>
	/// Setup custom full moon
	/// </summary>
	/// <param name="Name">Name of the custom fullmoon</param>
	public void _overwriteMoon(string Name)
	{
		_nameCustomPhase = Name;     //setting name of custom moon
		_pendingCustomPhase = true;       //indicates that next full moon has to be overwritten
		_activeCustomPhase = false;           //indicates that custom moon was not already used
	}

	/// <summary>
	/// Returns the current moonphase as string
	/// </summary>
	/// <returns>String containing the current moonphase</returns>
	public string printMoonphase()
	{
		if (_activeCustomPhase)
		{
			return _nameCustomPhase;
		}
		switch (_phase)
		{
			case Moonphases.eFullMoon:
				{
					return "Full Moon";
				}
			case Moonphases.eWaningMoon:
				{
					return "Waning Moon";
				}
			case Moonphases.eWaningHalfmoon:
				{
					return "Half Moon";
				}
			case Moonphases.eNewMoon:
				{
					return "New Moon";
				}
			case Moonphases.eWaxingMoon:
				{
					return "Waxing Moon";
				}
			case Moonphases.eWaxingHalfmoon:
				{
					return "Half Moon";
				}
			default:
				{
					return _phase.ToString() + " is not a valid moonphase.";
				}
		}
	}
}
