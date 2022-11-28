using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Serialization;

public class btnRightClick : Button
{
	[FormerlySerializedAs("onClick")]
	[SerializeField]
	private ButtonClickedEvent m_OnLClick = new ButtonClickedEvent();   // Event for leftclickhandling
	[FormerlySerializedAs("onClick")]
	[SerializeField]
	private ButtonClickedEvent m_OnRClick = new ButtonClickedEvent();   // Event for rightclickhandling

	
	public ButtonClickedEvent onLClick
	{
		get { return m_OnLClick; }
		set { m_OnLClick = value; }
	}
	public ButtonClickedEvent onRClick
	{
		get { return m_OnRClick; }
		set { m_OnRClick = value; }
	}

	private void PressL()
	{
		if (!IsActive() || !IsInteractable())
			return;
		UISystemProfilerApi.AddMarker("Button.onClick", this);
		m_OnLClick.Invoke();
	}
	private void PressR()
	{
		if (!IsActive() || !IsInteractable())
			return;
		UISystemProfilerApi.AddMarker("Button.onRClick", this);
		m_OnRClick.Invoke();
	}

	public override void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button != PointerEventData.InputButton.Left && eventData.button != PointerEventData.InputButton.Right)
		{
			return;
		}
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			PressL();
		}
		else
		{
			PressR();
		}
	}

	public override void OnSubmit(BaseEventData eventData)
	{
		base.OnSubmit(eventData);
		PressL();

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



