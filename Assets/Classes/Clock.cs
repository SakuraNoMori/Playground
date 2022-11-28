public class Clock
{
	public class Time
	{
		public float seconds;
		public long minutes;
		public long hours;
		public Time(float s = 0f, long m = 0, long h = 0)
		{
			this.seconds = s < 0 ? 0 : s;
			this.minutes = m < 0 ? 0 : m;
			this.hours = h < 0 ? 0 : h;
		}
	}

	// Current time
	private float _secondsCurrent;
	public float Seconds { get => _secondsCurrent; }
	private long _minutesCurrent;
	public long Minutes { get => _minutesCurrent; }
	private long _hoursCurrent;
	public long Hours { get => _hoursCurrent; }
	public Time CurrentTime { get => new Time(_secondsCurrent, _minutesCurrent, _hoursCurrent); }

	// Maximum amount of seconds/minutes/hours
	private readonly float _secondsMax;           //seconds per minute
	public float SecondsMax { get => _secondsMax; }
	private readonly long _minutesMax;           //minutes per hour
	public long MinutesMax { get => _minutesMax; }
	private readonly long _hoursMax;             //hours per day
	public long HoursMax { get => _hoursMax; }
	public Time MaxTime { get => new Time(_secondsMax, _minutesMax, _hoursMax); }

	// String for formatting time
	private string _format;


	/// <summary>
	/// Creates default clock with 24 hours per day, 60 minutes per hour and 60 seconds per minute
	/// </summary>
	public Clock()
	: this(0, 0, 0, 60, 60, 24)
	{ }

	/// <summary>
	/// Creates clock with custom start time and hours/minutes/seconds per day.<br/>
	/// Start values are set respecting the maximum values
	/// </summary>
	/// <param name="seconds">Start value for seconds</param>
	/// <param name="minutes">Start value for minutes</param>
	/// <param name="hours">Start value for hours</param>
	/// <param name="secondsMax">Maximum values of seconds</param>
	/// <param name="minutesMax">Maximum values of minutes</param>
	/// <param name="hoursMax">Maximum values of hours</param>
	public Clock(float seconds, long minutes, long hours, float secondsMax = 60, long minutesMax = 60, long hoursMax = 24)
	{
		_secondsMax = secondsMax <= 0 ? 1 : secondsMax;
		_minutesMax = minutesMax <= 0 ? 1 : minutesMax;
		_hoursMax = hoursMax <= 0 ? 1 : hoursMax;
		_secondsCurrent = seconds < 0 ? 0 : seconds % _secondsMax;
		_minutesCurrent = minutes % _minutesMax;
		_hoursCurrent = hours % _hoursMax;
		_calcMaxDigits();
	}

	/// <summary>
	/// Creates clock with custom start time and hours/minutes/seconds per day.<br/>
	/// Start values are set respecting the maximum values
	/// </summary>
	/// <param name="current">Start values for time</param>
	/// <param name="max">Maximum values for time</param>
	public Clock(Time current, Time max)
	: this(current.seconds, current.minutes, current.hours, max.seconds, max.minutes, max.hours)
	{ }

	/// <summary>
	/// Used for setting up start time of a clock. For progressing the clock use pass_time()
	/// </summary>
	/// <param name="seconds">Start value for seconds</param>
	/// <param name="minutes">Start value for minutes</param>
	/// <param name="hours">Start value for hours</param>
	public void setTime(float seconds, long minutes, long hours)
	{
		_secondsCurrent = seconds % _secondsMax;
		_minutesCurrent = minutes % _minutesMax;
		_hoursCurrent = hours % _hoursMax;
	}

	/// <summary>
	/// Used to progress the clock, handles calculation of new time
	/// </summary>
	/// <param name="seconds">Number of passed seconds</param>
	/// <param name="minutes">Number of passed minutes</param>
	/// <param name="hours">Number of passed hours</param>
	/// <returns>Number of passed days</returns>
	public long passTime(long seconds = 1, long minutes = 0, long hours = 0)
	{
		//adds passed time to current time
		_secondsCurrent += seconds>=0?seconds:0;
		_minutesCurrent += minutes >= 0 ? minutes : 0;
		_hoursCurrent += hours >= 0 ? hours : 0;

		return _resolveOverflow();
	}

	/// <summary>
	/// Returns current time in format h:m:s with necessary numbers of leading zeros to give a consistent clockdisplay
	/// </summary>
	/// <returns>String representing current time in format h:m:s</returns>
	public string printTime()
	{
		return string.Format(_format, _hoursCurrent, _minutesCurrent, (long)_secondsCurrent);
	}

	/// <summary>
	/// Resolves overflows of seconds/minutes/hours
	/// </summary>
	/// <returns>Number of days that have passed</returns>
	private long _resolveOverflow()
	{
		long overflows = 0;
		if (_secondsCurrent >= _secondsMax) // Check for overflow of seconds
		{
			overflows = (long)(_secondsCurrent / _secondsMax);
			_secondsCurrent -= overflows * _secondsMax;
		}
		if (overflows > 0)
		{
			_minutesCurrent += overflows;
			overflows = 0;
		}
		if (_minutesCurrent >= _minutesMax)    // Check for overflow of minutes
		{
			overflows = _minutesCurrent / _minutesMax;
			_minutesCurrent -= overflows * _minutesMax;
		}
		if (overflows > 0)
		{
			_hoursCurrent += overflows;
			overflows = 0;
		}
		if (_hoursCurrent >= _hoursMax)    // Check for overflow of minutes
		{
			overflows = _hoursCurrent / _hoursMax;
			_hoursCurrent -= overflows * _hoursMax;
		}
		return overflows;
	}

	/// <summary>
	/// Calculates the number of digits for the maximum values
	/// </summary>
	private void _calcMaxDigits()
	{
		//number of digits needed for nice print
		long _maxDigitsSeconds;
		long _maxDigitsMinutes;
		long _maxDigitsHours;

		long compare = 1;
		long digit = 0;
		do
		{
			compare *= 10;
			digit++;
		}
		while (compare < _secondsMax);
		_maxDigitsSeconds = digit;

		compare = 1;
		digit = 0;
		do
		{
			compare *= 10;
			digit++;
		}
		while (compare < _minutesMax);
		_maxDigitsMinutes = digit;

		compare = 1;
		digit = 0;
		do
		{
			compare *= 10;
			digit++;
		}
		while (compare < _hoursMax);
		_maxDigitsHours = digit;
		_format = "{0:D" + _maxDigitsHours.ToString() + "}:{1:D" + _maxDigitsMinutes.ToString() + "}:{2:D" + _maxDigitsSeconds.ToString() + "}";
	}
}
