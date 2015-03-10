using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IngredientsMenu : Menu {

	[SerializeField]
	private int IngredientsPerRow = 3;
	private int ingredientInRow = -1;
	[SerializeField]
	private float verticalPadding = 1.0f, horizontalPadding = 1.0f, buttonWidth = 50f, buttonHeight = 50f,
	confirmBoxLeft = 50f, confirmBoxTop = 50f, confirmBoxWidth = 50f, confirmBoxHeight = 50f;

	[SerializeField]
	private List<bool> ingredientsSelected = new List<bool>();

	[SerializeField]
	private GUIStyle ingredientStyle;

	void Start()
	{
		XMLManager.ReadJuicesFromXML();

		StartCoroutine(GetIngredientsSelected());
	}
	void OnGUI()
	{
		if(!isActive)
			return;

		int paddingMultiplier = -1;
		for(int i = 0; i < Session.myIngredients.Count; i ++)
		{
			Ingredient currentIngredient = Session.myIngredients[i];
			if(i % IngredientsPerRow == 0)
			{
				paddingMultiplier++;
				ingredientInRow = -1;
			}
			ingredientInRow++;
		
			if(GUI.Button(new Rect(baseHorizontalPosition + (horizontalPadding * ingredientInRow), baseVerticalPosition + (verticalPadding * paddingMultiplier), 
			                       buttonWidth, buttonHeight), currentIngredient.name))
			{
				ingredientsSelected[i] = !ingredientsSelected[i];
			}
			GUI.skin.button.wordWrap = true;

		}

		if(GUI.Button(new Rect(confirmBoxLeft, confirmBoxTop, confirmBoxWidth, confirmBoxHeight), "Confirm"))
		{
			MenuManager.ChangeMenu("JuicesMenu");
		}


	}


	//turn this into something dynamic like currently selected ingredients, etc

	private IEnumerator GetIngredientsSelected()
	{
		yield return null;
		for(int i = 0; i < Session.myIngredients.Count; i++)
		{
			ingredientsSelected.Add(false);
		}
	}
}
