using System.Collections;
using System.Collections.Generic;

using UnityEditor;
using UnityEditor.SceneManagement;

using UnityEngine;
using UnityEngine.SceneManagement;

//[InitializeOnLoad]
public class CopyPasteDetector
{
	static CopyPasteDetector()
	{
		Debug.Log("Detector is online");
		EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyGUIEvent;
		EditorApplication.hierarchyChanged += OnHierarchyChange;
		EditorSceneManager.sceneOpened += OnSceneChange;
		items.Clear();

		if(CheckForTargetScene())
		{
			var ite = Object.FindObjectsOfType<BoxCollider>();

			foreach(var ic in ite)
			{
				items.Add(ic);
			}
		}
	}

	static void OnHierarchyChange()
	{
		if(pasted)
		{
			var a = Object.FindObjectsOfType<BoxCollider>();
			foreach(var b in a)
			{
				if(!items.Contains(b))
				{
					b.gameObject.AddComponent<AudioSource>();
					items.Add(b);
				}
			}
			pasted = false;
		}
	}

	static void OnSceneChange(Scene s, OpenSceneMode mode)
	{
		Debug.Log("Scene change detected");
		if(s.name == TargetScene)
		{
			var ite = Object.FindObjectsOfType<BoxCollider>();

			items.Clear();
			foreach(var i in ite)
			{
				items.Add(i);
			}
		}
	}

	static void OnHierarchyGUIEvent(int id, Rect rect)
	{
		Event e = Event.current;

		if(e != null || !CheckForTargetScene())
		{
			if(e.commandName == "Paste")
			{
				Object o = EditorUtility.InstanceIDToObject(id);
				if(o == null)
				{
					return;
				}

				if(e.commandName == "Paste")
				{
					pasted = true;
				}
			}
		}
	}

	private static bool CheckForTargetScene()
	{
		for(int i = 0; i < SceneManager.sceneCount; i++)
		{
			if(SceneManager.GetSceneAt(i).name == TargetScene)
			{
				return true;

			}
		}

		return false;
	}

	private static string TargetScene = "New Scene";
	private static bool pasted;
	private static List<BoxCollider> items = new();
}
