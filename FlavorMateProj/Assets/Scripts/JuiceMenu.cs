using UnityEngine;
using System.Collections;

public class JuiceMenu : Menu {

	public GUIStyle style;
    [SerializeField]
    private float rowPadding = 1.0f, ingredientsPadding = 1.0f, inner_ingredientsPadding = 50f, nameBoxWidth = 50f, nameBoxHeight = 50f,
    ingredientBoxWidth = 50f, ingredientBoxHeight = 50f, selectedBoxLeft = 50f, selectedBoxTop = 50f, selectedBoxWidth = 100f, selectedBoxHeight = 100f,
    rowButtonWidth = 50f, ingredientBGBoxWidth = 50f, ingredientBGBoxHeight = 50f, ingredientsBoxLeft = 50f, ingredientsBoxTop = 50f, ingredientsBoxWidth = 50f, ingredientsBoxHeight = 50f,
    description_juiceNameBoxLeft, description_juiceNameBoxTop, description_juiceNameBoxWidth, description_juiceNameBoxHeight,
    description_makerBoxLeft, description_makerBoxTop, description_makerBoxWidth, description_makerBoxHeight,
    description_ingredientBoxLeft, description_ingredientBoxTop, description_ingredientBoxWidth, description_ingredientBoxHeight,
    description_textBoxLeft, description_textBoxTop, description_textBoxWidth, description_textBoxHeight,
    description_saveBoxLeft, description_saveBoxTop, description_saveBoxWidth, description_saveBoxHeight;
	
    
    public static Juice selectedJuice = null;

	[SerializeField]
	private Texture ingredient;


	void Start()
	{
		XMLManager.ReadJuicesFromXML();
	}


    public float left = 50f, top = 50f, width = 50f, height = 50f;
	void OnGUI()
	{
		if(!isActive)
			return;

		//convert this to gui group later?
		for(int i = 0; i < Session.myJuices.Count; i ++)
		{
			Juice juice = Session.myJuices[i];

            if (GUI.Button(new Rect(baseHorizontalPosition, baseVerticalPosition + (rowPadding * i), rowButtonWidth, nameBoxHeight), "",style))
            {
                selectedJuice = juice;
            }


            Rect juiceNameRect = new Rect(baseHorizontalPosition, baseVerticalPosition + (rowPadding * i), nameBoxWidth, nameBoxHeight);
            GUI.Box(juiceNameRect, juice.name);

			GUI.Box(new Rect(baseHorizontalPosition + ingredientsPadding + inner_ingredientsPadding, baseVerticalPosition + (rowPadding * i), ingredientBGBoxWidth, ingredientBGBoxHeight), "");


			for(int ingredientNum = 0; ingredientNum < juice.ingredients.Count; ingredientNum++)
			{
				GUI.Label(new Rect((baseHorizontalPosition + ingredientsPadding) + (inner_ingredientsPadding * (ingredientNum + 1)), baseVerticalPosition + (rowPadding * i), ingredientBoxWidth, ingredientBoxHeight), ingredient);
			}

			//GUI.Box(new Rect(baseHorizontalPosition + horizontalPadding, baseVerticalPosition + (verticalPadding * i), descriptionBoxWidth, descriptionBoxHeight), juice.description);
			GUI.skin.box.wordWrap = true;
			GUI.skin.button.wordWrap = true;
		}

		if(GUI.Button(new Rect(baseHorizontalPosition, baseVerticalPosition + (rowPadding * Session.myJuices.Count), nameBoxWidth, nameBoxHeight), "+"))
		{
			Juice newJuice = new Juice();
            selectedJuice = newJuice;
		}


        if (selectedJuice != null)
        {
            Rect descriptionRect = new Rect(selectedBoxLeft, selectedBoxTop, selectedBoxWidth, selectedBoxHeight);
            Rect description_makerRect = new Rect(selectedBoxLeft + description_makerBoxLeft, selectedBoxTop + description_makerBoxTop, description_makerBoxWidth, description_makerBoxHeight);
            Rect description_juiceNameRect = new Rect(selectedBoxLeft + description_juiceNameBoxLeft, selectedBoxTop + description_juiceNameBoxTop, description_juiceNameBoxWidth, description_juiceNameBoxHeight);
            Rect description_ingredientsRect = new Rect(selectedBoxLeft + description_ingredientBoxLeft, selectedBoxTop + description_ingredientBoxTop, description_ingredientBoxWidth, description_ingredientBoxHeight);
            Rect description_textRect = new Rect(selectedBoxLeft + description_textBoxLeft, selectedBoxTop + description_textBoxTop, description_textBoxWidth, description_textBoxHeight);


            GUI.Box(descriptionRect, "");

            if (selectedJuice.juiceID > 0)
            {

                GUI.Box(description_juiceNameRect, selectedJuice.name);
                GUI.Box(description_makerRect, selectedJuice.maker.name);
                GUI.Box(description_ingredientsRect, "Ingredients");       
                GUI.Box(description_textRect, selectedJuice.description);
            }

            else
            {
                selectedJuice.name = GUI.TextArea(description_juiceNameRect, selectedJuice.name);
                if (GUI.Button(description_makerRect, selectedJuice.maker.name))
                {
                    MenuManager.ChangeMenu("MakerMenu");
                }
                GUI.Button(description_ingredientsRect, "Ingredients");
                selectedJuice.description = GUI.TextArea(description_textRect, selectedJuice.description);

                Rect saveBoxRect = new Rect(selectedBoxLeft + description_saveBoxLeft, selectedBoxTop + description_saveBoxTop, description_saveBoxWidth, description_saveBoxHeight);
                if (GUI.Button(saveBoxRect, "Save"))
                {
                    Session.myJuices.Add(selectedJuice);
                    //call this after save
                    Session.JuiceUpdateSQL(selectedJuice, false);
                }
            }
        }
		if(GUI.Button(new Rect(ingredientsBoxLeft, ingredientsBoxTop, ingredientsBoxWidth, ingredientsBoxHeight), "Ingredients"))
		{
			MenuManager.ChangeMenu("IngredientsMenu");
		}

		


	}
}
