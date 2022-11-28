
using System.Collections.Generic;

public class Calendar
{

	private readonly int _lengthWeek;    // Days per week
	public int LengthWeek { get => _lengthWeek; }

	// Current date
	private int _day;
	/// <summary>
	/// Returns the current day
	/// </summary>
	public int Day { get => _day; }
	private int _weekday;
	/// <summary>
	/// Returns the current weekday
	/// </summary>
	public int Weekday { get => _weekday; }
	private int _month;
	/// <summary>
	/// Returns the current month
	/// </summary>
	public int Month { get => _month; }
	private int _year;
	/// <summary>
	/// Returns the current year
	/// </summary>
	public int Year { get => _year; }

	// List containing he number of days in each month and at index 0 the number of months in a year
	private readonly List<int> _lengthMonth = new List<int>();
	/// <summary>
	/// Returns the length of the current month
	/// </summary>
	public int LengthMonth { get => getLengthMonth(_month); }
	private int _lengthYear;
	/// <summary>
	/// Returns the number of days in a non-leap-year
	/// </summary>
	public int LengthYearDays { get => _lengthYear; }
	/// <summary>
	/// Returns the number of months in a year
	/// </summary>
	public int LengthYearMonths { get => _lengthMonth[0]; }

	private bool _leapday;  // 0 = leap-yearmechanic off/ manualmode (leap-days occur only after being manually triggered by "set_trigger()")
							// 1 = leap-yearmechanic in automode (leapday after a fixed timespan, time determined by "leap_cycle")
	private bool _leapTrigger;  // Flag that shows that the current year is a leap-year
	private int _leapMonth; // Month in which the leapday occurs
	private int _leapCycle; // Number of years between two leap-days, only necessary for automode
	private int _leapStart; // Year of the first leapday to occur, only necessary for automode

	// String for formatting date
	private string _format; // TODO define string

	/// <summary>
	/// Creates a normal year with 12 months, 365 days and a leapday every 4 years in the second month
	/// </summary>
	public Calendar()
	{
		_lengthWeek = 7;        //7 days per week
		_lengthMonth.Capacity = 13;

		_lengthMonth.Add(12);   //12 months per year

		//number of days in a month
		_lengthMonth.Add(31);
		_lengthMonth.Add(28);
		_lengthMonth.Add(31);
		_lengthMonth.Add(30);
		_lengthMonth.Add(31);
		_lengthMonth.Add(30);
		_lengthMonth.Add(31);
		_lengthMonth.Add(31);
		_lengthMonth.Add(30);
		_lengthMonth.Add(31);
		_lengthMonth.Add(30);
		_lengthMonth.Add(31);

		//calculation of days in a non-leap-year
		_calcYearLength();

		//calendar starts at first day of first month of first year
		setDate(1, 1, 1, 0);

		//first leap-year is zero, with leap years occuring every 4 years. leap day occurs in second month
		_setupLeapCycle(true, 2, 4, 0);
	}

	/// <summary>
	/// Creates a calendar where each month has the same number of days
	/// </summary>
	/// <param name="Months">Number of months in a year</param>
	/// <param name="Monthdays">Number of days in a month</param>
	/// <param name="Weekdays">Number of days in a week</param>
	/// <param name="Leapday">True: Leap-days are automatically handled<br/>False: Leap-days need to be triggered by external code with setTrigger()</param>
	/// <param name="Leapmonth">The month where the leapday occurs</param>
	/// <param name="Leapcycle">Number of years between leap-years</param>
	/// <param name="Startyear">The first year with a leap-day</param>
	public Calendar(int Months, int Monthdays = 30, int Weekdays = 7, bool Leapday = false, int Leapmonth = 1, int Leapcycle = 1, int Startyear = 0)
	{

		_lengthWeek = Weekdays;                             //setting length of a week

		_lengthMonth.Capacity = Months + 1;                   //increasing size of vector to house all monthlengths + number of months in a year
		_lengthMonth.Add(Months);                        //writing length of year (=size of vector) to position 0

		for (int i = 1; i <= _lengthMonth[0]; i++)      //filling vector with the same number at every position to avoid empty elements
		{                                                           //monthlengths have to be changed manually if necessary
			_lengthMonth.Add(Monthdays);
		}

		//calculating number of days in a non-leap-year
		_calcYearLength();

		//setting date to the first day in first month in year 1
		setDate(1, 1, 1, 0);

		_setupLeapCycle(Leapday, Leapmonth, Leapcycle, Startyear);

	}

	/// <summary>
	/// Creates a calendar based on the parameters
	/// </summary>
	/// <param name="Monthdays">List that contains the number of days in each month</param>
	/// <param name="Weekdays">Number of days in a week</param>
	/// <param name="Leapday">True: Leap-days are automatically handled<br/>False: Leap-days need to be triggered by external code with setTrigger()</param>
	/// <param name="Leapmonth">The month where the leapday occurs</param>
	/// <param name="Leapcycle">Number of years between leap-years</param>
	/// <param name="Startyear">The first year with a leapday</param>
	public Calendar(List<int> Monthdays, int Weekdays = 7, bool Leapday = false, int Leapmonth = 1, int Leapcycle = 1, int Startyear = 0)
	{
		_lengthWeek = Weekdays;                             //setting length of a week
		int yearlength = Monthdays.Count;

		_lengthMonth.Capacity = yearlength + 1;                        //resizing vector to fit length of every month
		_lengthMonth.Add(yearlength);                            //entering number of months per year

		for (int i = 1; i <= _lengthMonth[0]; i++)      //filling vector with the content of the array
		{
			_lengthMonth.Add(Monthdays[i - 1]);
		}

		//calculating number of days in a non-leap-year
		_calcYearLength();

		//setting start date
		setDate(1, 1, 1, 0);

		_setupLeapCycle(Leapday, Leapmonth, Leapcycle, Startyear);
	}

	/// <summary>
	/// Checks if the current year is a leap-year
	/// Only in automode
	/// </summary>
	private void _leapCheck()
	{
		if (_leapday == false)      //leap_day is not in automated mode
			return;
		//leap_day is in automated mode
		//leap-days occur only, if current year is higher or equal to the startyear
		if (_year > _leapStart)
		{
			if (((_year - _leapStart) % _leapCycle == 0) && _leapday)
				setTrigger();
			else
				return;
		}
		else if (_year == _leapStart)
		{
			setTrigger();
		}
	}

	/// <summary>
	/// Returns the number of days in a month
	/// </summary>
	/// <param name="month">Index to specify the month</param>
	/// <returns>The number of days of the specified month</returns>
	public int getLengthMonth(int month)
	{
		if (month <= 0 || month > _lengthMonth[0])
			return -1;
		else
		{
			return _lengthMonth[month];
		}
	}

	/// <summary>
	/// Sets the trigger for a leap-year.
	/// Can be used to trigger a leap-year manually.
	/// </summary>
	public void setTrigger()
	{
		_leapTrigger = true;
	}

	/// <summary>
	/// Progresses the calendar and handles calculation of date
	/// </summary>
	/// <param name="days">Number of passed days</param>
	/// <param name="months">Number of passed months</param>
	/// <param name="years">Number of passed years</param>
	public void passTime(int days = 1, int months = 0, int years = 0)
	{
		// Increases _day and _weekday by the number of passed  days
		_day += days>=0?days:0;
		_weekday += days>=0?days:0;

		// As long as _day is bigger than the number of days in the month, the length of the month is subtracted and _month is incremented
		while ((_day > _lengthMonth[_month] && (_leapTrigger == false || _month != _leapMonth)) ||          //month without a leapday
			   ((_leapTrigger == true) && (_month == _leapMonth) && (_day > (_lengthMonth[_month] + 1))))   //month with occuring leapday
		{
			//subtracting number of days in current month from number of passed days
			if (_leapTrigger == true && _month == _leapMonth)
			{
				_day -= _lengthMonth[_month] + 1;          //+1 because of occuring leapday
				_leapTrigger = false;                                           //resetting trigger for leap-days
			}
			else if (_leapTrigger == false || _month != _leapMonth)
				_day -= _lengthMonth[_month];

			_month++;

			if (_month > _lengthMonth[0])     //if end of the year is reached, _month is resetted to 1, _year is incremented and checked for leap-year
			{
				_month = 1;
				_year++;
				_leapCheck();
			}
		}

		//passing months
		while (months > 0)
		{
			_weekday += _lengthMonth[_month];      //adding passed days to _weekday to keep track of correct weekday
			_month++;
			months--;

			if (_month > _lengthMonth[0])     //if end of the year is reached, _month is resetted to 1 and _year is incremented
			{
				_month = 1;
				_year++;
				_leapCheck();
			}
		}

		//passing years
		while (years > 0)
		{
			if (_leapTrigger == false)
			{
				_weekday += _lengthYear;         //adding passed days to _weekday to keep track of correct weekday
			}
			else if (_leapTrigger == true)
			{
				_weekday += _lengthYear + 1;     //adding passed days to _weekday to keep track of correct weekday; +1 because of leap-days
				_leapTrigger = false;
			}
			_year++;
			years--;
		}

		// Calculating current weekday
		while (_weekday > _lengthWeek)
		{
			_weekday %= _lengthWeek;
		}

		return;
	}

	/// <summary>
	/// Sets up the necessary parameters for handling of leap-years
	/// </summary>
	/// <param name="Leapday">True: Leap-days are automatically handled<br/>False: Leap-days need to be triggered by external code with setTrigger()</param>
	/// <param name="Leapmonth">The month where the leapday occurs</param>
	/// <param name="Leapcycle">Number of years between leap-years</param>
	/// <param name="Startyear">The first year with a leapday</param>
	private void _setupLeapCycle(bool Leapday = false, int Leapmonth = 1, int Leapcycle = 1, int Startyear = 0)
	{
		_leapday = Leapday;
		_leapMonth = Leapmonth;
		_leapCycle = Leapcycle;
		_leapStart = Startyear;
		_leapCheck();
	}

	/// <summary>
	/// Calculates the number of days in a year
	/// </summary>
	private void _calcYearLength()
	{
		_lengthYear = 0;
		for (int i = 1; i <= _lengthMonth[0]; i++)
		{
			_lengthYear += _lengthMonth[i];
		}
	}

	/// <summary>
	/// Set the current date, should be only used for setting up the calendar
	/// </summary>
	/// <param name="Day">Current day</param>
	/// <param name="Weekday">Current weekday</param>
	/// <param name="Month">Current month</param>
	/// <param name="Year">Current year</param>
	public void setDate(int Day, int Weekday, int Month, int Year)
	{
		_day = Day;
		_weekday = Weekday;
		_month = Month;
		_year = Year;
	}

	/// <summary>
	/// Sets the length of a specific month, only for setting up the calendar.
	/// </summary>
	/// <param name="Month">Index to specify the month, starts from 1.<br/>If invalid no changes are made.</param>
	/// <param name="Length">Number of days the specified month should have</param>
	public void setLengthMonth(int Month, int Length)
	{
		if (Month < 1 || Month > LengthYearMonths)
		{
			return;
		}

		_lengthMonth[Month] = Length;
		_calcYearLength();
	}

	/// <summary>
	/// Returns the current date
	/// </summary>
	/// <returns>Returns the current date as string</returns>
	public string printDate()
	{
		// TODO beautify output?
		// Prints date in dd//mm//yy
		return "Day " + _day + "/" + ((_leapTrigger && (_month == _leapMonth)) ? (_lengthMonth[_month] + 1) : _lengthMonth[_month])
			+ "  Month " + _month + "/" + LengthYearMonths + "  Year " + _year;
		//return string.Format(_format, _day, _month, _year);
	}




}
