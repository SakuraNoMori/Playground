

public static class BoolExtensions
{
	/// <summary>
	/// Toggles the value of the bool
	/// </summary>
	public static void Toggle(ref this bool b)
	{
		b = !b;
	}
}
