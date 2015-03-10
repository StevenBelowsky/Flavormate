using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuManager : MonoBehaviour {


	private static List<Transform> menus = new List<Transform>();

	public static string currentMenu;

	void Start () 
	{
		foreach(Transform child in transform)
		{
			if(child.name.Contains("Menu"))
			{
				menus.Add(child);
			}
		}

		ChangeMenu("IdleMenu");
	}


	public static void ChangeMenu(string menuName)
	{
		foreach(Transform menu in menus)
		{
			bool containsMenuName = menu.name.Contains(menuName);
			menu.GetComponent<Menu>().isActive = containsMenuName;

		}	

		currentMenu = menuName;

	}



}
