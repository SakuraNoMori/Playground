using System.Collections;
using System.Collections.Generic;

using UnityEngine;

/// <summary>
/// Usage
/// 1. Add Manager to a GameObject
/// 2. Setup Scriptexecution-Order in Unity so that Manager runs before other user-scripts
/// 3. Define Keycodes for actions and strings for axes as public const
/// 4. Add them to their respective List
/// 5. Use the static functions to check for keypresses, axis-values
/// </summary>
public class InputManager : MonoBehaviour
{
	private void Awake()
	{
		// Singleton
		if(_instance != null && _instance != this)
		{
			Destroy(this);
		}
		else
		{
			_instance = this;
		}
	}

	private void Start()
	{
		// Registering all defined key and axis		
		_registeredKeys.Add(Jump);
		_registeredKeys.Add(Attack);
		_registeredKeys.Add(Accept);
		_registeredKeys.Add(Cancel);

		_registeredAxes.Add(MoveHorizontal);
		_registeredAxes.Add(MoveVertical);
	}

	/// <summary>
	/// Setup unityproject so that Update of InputManager is called first
	/// </summary>
	private void Update()
	{
		_KeyDowns.Clear();
		_KeyHolds.Clear();
		_KeyUps.Clear();
		_Axes.Clear();

		foreach(KeyCode key in _registeredKeys)
		{
			if(Input.GetKeyDown(key))
			{
				_KeyDowns.Add(key);
			}
			if(Input.GetKeyUp(key))
			{
				_KeyUps.Add(key);
			}
			if(Input.GetKey(key))
			{
				_KeyHolds.Add(key);
			}
		}

		foreach(string axis in _registeredAxes)
		{
			if(!_Axes.ContainsKey(axis))
			{
				_Axes.Add(axis, Input.GetAxis(axis));
			}
		}
	}

	/// <summary>
	/// Ckecks whether a key was pressed this frame
	/// </summary>
	/// <param name="key">Key to check</param>
	/// <returns>True if key was pressed, otherwise false</returns>
	public static bool IsKeyDown(KeyCode key)
	{
		return _KeyDowns.Contains(key);
	}

	/// <summary>
	/// Ckecks whether a key was released this frame
	/// </summary>
	/// <param name="key">Key to check</param>
	/// <returns>True if key was released, otherwise false</returns>
	public static bool IsKeyUp(KeyCode key)
	{
		return _KeyUps.Contains(key);
	}

	/// <summary>
	/// Ckecks whether a key was held down this frame
	/// </summary>
	/// <param name="key">Key to check</param>
	/// <returns>True if key was held down, otherwise false</returns>
	public static bool IsKeyHeld(KeyCode key)
	{
		return _KeyHolds.Contains(key);
	}

	/// <summary>
	/// Get the value of an axis
	/// </summary>
	/// <param name="axis">Name of the axis to read from</param>
	/// <returns>The value of the axis or zero if axis couldn't be found</returns>
	public static float GetAxisValue(string axis)
	{
		if(_Axes.TryGetValue(axis, out float value))
		{
			return value;
		}

		return 0f;
	}

	/// <summary>
	/// Define all keys and axis that should be queried.
	/// Add them in Start/Awake to their respective List
	/// </summary>
	#region
	public const KeyCode Jump = KeyCode.Space;
	public const KeyCode Attack = KeyCode.X;
	public const KeyCode Accept = KeyCode.Return;
	public const KeyCode Cancel = KeyCode.Escape;
	public const string MoveHorizontal = "Horizontal";
	public const string MoveVertical = "Vertical";
	#endregion

	private readonly List<KeyCode> _registeredKeys = new();
	private readonly List<string> _registeredAxes = new();

	private static readonly HashSet<KeyCode> _KeyDowns = new();
	private static readonly HashSet<KeyCode> _KeyHolds = new();
	private static readonly HashSet<KeyCode> _KeyUps = new();
	private static readonly Dictionary<string, float> _Axes = new();

	private static InputManager _instance;
	public static InputManager Instance { get => _instance; }
}
