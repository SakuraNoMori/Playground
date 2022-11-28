using UnityEngine;
using MathNet.Numerics.Distributions;
using MathNet.Numerics.Random;

public class RNG : MonoBehaviour
{
	// Singleton instance
	private static RNG _instance;
	public static RNG Instance { get => _instance; }

	// instance of generator
	private MersenneTwister _rngMT;

	// Distributions
	private Normal _distNormalSkewing;  // normal distribution used for skewed values with most values in [-1,1]

	private void Awake()
	{
		if (_instance == null)
		{
			_instance = this;
		}
		else
		{
			Destroy(this.gameObject);
		}
		Debug.Log("RNG has started.");
		_rngMT = new MersenneTwister();
		_distNormalSkewing = new Normal(0, 0.4, _rngMT);
	}

	/// <summary>
	/// Returns a random int in the intervall [min,max]
	/// </summary>
	/// <param name="min">Lower bound of random numbers</param>
	/// <param name="max">Upper bound of random numbers</param>
	/// <returns>A random int in the intervall [min,max]</returns>
	public int getInt(int min, int max)
	{
		int lower = min <= max ? min : max;
		int upper = min <= max ? max : min;
		return _rngMT.Next(lower, upper == int.MaxValue ? upper : upper + 1);
	}

	/// <summary>
	/// Returns a random int in the intervall [0,max]
	/// </summary>
	/// <param name="max"></param>
	/// <returns>A random int in the intervall [0,max] or 0 if max is negative or equal to 0</returns>
	public int getInt(int max)
	{
		return max <= 0 ? 0 : getInt(0, max);
	}

	/// <summary>
	/// Returns a random value within or close to [min,max].<br/>
	/// Value may exceed bounds because calculation is based on a simulated skewed normal distribution.
	/// </summary>
	/// <param name="min">Lower bound</param>
	/// <param name="max">Upper bound</param>
	/// <param name="peak">Peak of the skewed distribution</param>
	/// <param name="cutoff">True: Clamps the random value to [min,max]<br/>False: Random value can be slightly outside of [min,max]</param>
	/// <returns>A random value within or close to [min,max]</returns>
	public float getNormalSkewed(float min, float max, float peak, bool cutoff = false)
	{
		// Define lower and upper bound
		float lower = min <= max ? min : max;
		float upper = min <= max ? max : min;
		// Get a normal-distributed random number
		float skew = (float)_distNormalSkewing.Sample();

		// Dampen the part of the curve outside of [-1,1] to reduce deviation
		if (Mathf.Abs(skew) > 1)
		{
			if (cutoff)
			{
				skew = skew < 0 ? -1 : 1;
			}
			else
			{
				skew = (skew - 1f) * 0.2f + 1f;

			}
		}
		// Calculate delta depending of sign of skew
		float delta = skew < 0 ? peak - lower : upper - peak;
		// Scale delta
		delta *= skew;

		// Add delta to peak to get random value mostly in [min,max]
		return peak + delta;
	}
}
