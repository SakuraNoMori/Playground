using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class btnUtil : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
	[SerializeField] private Sprite _neutral;
	[SerializeField] private Sprite _hover;
	[SerializeField] private Sprite _down;

	private Image _img;

	public void OnPointerDown(PointerEventData eventData)
	{
		_img.sprite = _down;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		_img.sprite = _hover;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		_img.sprite = _neutral;
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		_img.sprite = _neutral;
	}

	private void Awake()
	{
		_img = this.gameObject.GetComponent<Image>();
		_img.sprite = _neutral;
	}
	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}
}
