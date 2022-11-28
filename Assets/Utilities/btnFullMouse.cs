using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Serialization;

//[AddComponentMenu("MIR/Buttons/Button - Full mousesupport", 1)]
public class btnFullMouse : Selectable, IPointerClickHandler, ISubmitHandler
{
	[Serializable]
	/// <summary>
	/// Function definition for a button click event.
	/// </summary>
	public class ButtonClickedEvent : UnityEvent { }

	[FormerlySerializedAs("onLeftClick")]
	[SerializeField]
	private ButtonClickedEvent m_OnLeftClick = new ButtonClickedEvent();

	[FormerlySerializedAs("onRightClick")]
	[SerializeField]
	private ButtonClickedEvent m_OnRightClick = new ButtonClickedEvent();

	[FormerlySerializedAs("onMiddleClick")]
	[SerializeField]
	private ButtonClickedEvent m_OnMiddleClick = new ButtonClickedEvent();

	/// <summary>
	/// Event triggered by left-clicks
	/// </summary>
	public ButtonClickedEvent onLeftClick
	{
		get { return m_OnLeftClick; }
		set { m_OnLeftClick = value; }
	}

	/// <summary>
	/// Event triggered by right-clicks
	/// </summary>
	public ButtonClickedEvent onRightClick
	{
		get { return m_OnRightClick; }
		set { m_OnRightClick = value; }
	}

	/// <summary>
	/// Event triggered by middle-clicks
	/// </summary>
	public ButtonClickedEvent onMiddleClick
	{
		get { return m_OnMiddleClick; }
		set { m_OnMiddleClick = value; }
	}

	/// <summary>
	/// Function called when left clicking button
	/// </summary>
	private void PressLeft()
	{
		if (!IsActive() || !IsInteractable())
			return;
		UISystemProfilerApi.AddMarker("Button.onClick", this);
		m_OnLeftClick.Invoke();
	}

	/// <summary>
	/// Function called when right clicking button
	/// </summary>
	private void PressRight()
	{
		if (!IsActive() || !IsInteractable())
			return;
		UISystemProfilerApi.AddMarker("Button.onRClick", this);
		m_OnRightClick.Invoke();
	}

	/// <summary>
	/// Function called when middle clicking button
	/// </summary>
	private void PressMiddle()
	{
		if (!IsActive() || !IsInteractable())
			return;
		UISystemProfilerApi.AddMarker("Button.onMClick", this);
		m_OnMiddleClick.Invoke();
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		switch (eventData.button)
		{
			case PointerEventData.InputButton.Left:
				{
					PressLeft();
				}
				break;
			case PointerEventData.InputButton.Right:
				{
					PressRight();
				}
				break;
			case PointerEventData.InputButton.Middle:
				{
					PressMiddle();
				}
				break;
			default:
				{
					PressLeft();
				}
				break;
		}
	}

	public void OnSubmit(BaseEventData eventData)
	{
		PressLeft();

		// if we get set disabled during the press
		// don't run the coroutine.
		if (!IsActive() || !IsInteractable())
			return;

		DoStateTransition(SelectionState.Pressed, false);
		StartCoroutine(OnFinishSubmit());
	}


	private IEnumerator OnFinishSubmit()
	{
		var fadeTime = colors.fadeDuration;
		var elapsedTime = 0f;

		while (elapsedTime < fadeTime)
		{
			elapsedTime += Time.unscaledDeltaTime;
			yield return null;
		}

		DoStateTransition(currentSelectionState, false);
	}
}
