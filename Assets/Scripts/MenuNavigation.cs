//Original script taken from:
// http://unity.grogansoft.com/simple-script-for-gamepad-ui-navigation/
// Edited specifically for use by Jon B. "Cheezyrock" Honeycutt

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuNavigation : MonoBehaviour
{
	private Button previousButton;
	[SerializeField] private float scaleAmount = 1.4f;
	[SerializeField] private GameObject defaultButton = null;

	void Start()
	{
		if (defaultButton != null)
		{
			EventSystem.current.SetSelectedGameObject(defaultButton);
		}
	}
	void Update()
	{
		var selectedObj = EventSystem.current.currentSelectedGameObject;

		if (selectedObj == null) return;
		var selectedAsButton = selectedObj.GetComponent<Button>();
		if(selectedAsButton != null && selectedAsButton != previousButton)
		{
			if(selectedAsButton.transform.name != "PauseButton")
				HighlightButton(selectedAsButton);
		}

		if (previousButton != null && previousButton != selectedAsButton)
		{
			UnHighlightButton(previousButton);
		}
		previousButton = selectedAsButton;
	}
	void OnDisable()
	{
		if (previousButton != null) UnHighlightButton(previousButton);
	}

	void HighlightButton(Button btn)
	{
		btn.transform.localScale = new Vector3(scaleAmount, scaleAmount, scaleAmount);
	}

	void UnHighlightButton(Button btn)
	{
		btn.transform.localScale = new Vector3 (1, 1, 1);
	}
}