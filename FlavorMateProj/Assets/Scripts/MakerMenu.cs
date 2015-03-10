using UnityEngine;
using System.Collections;

//abstract this after!
public class MakerMenu  : Menu {


    public int makerPerRow = 4;

    [SerializeField]
    private float rowPadding = 0f, columnPadding = 0f, itemBoxWidth = 50f, itemBoxHeight = 50f,
    saveBoxLeft, saveBoxTop, saveBoxWidth, saveBoxHeight;

    [SerializeField]
    private GUIStyle style;
    [SerializeField]    
    private Texture maker;

    void OnGUI()
    {
        if (!isActive)
            return;

        int rowNum = 0;
        int columnCounter = 0;
        print(Session.myMakers.Count);
        for (int makerNum = 0; makerNum < Session.myMakers.Count; makerNum++)
			{

                Maker currentMaker = Session.myMakers[makerNum];
                if ( (makerNum + 1) % (makerPerRow + 1) == 0)
                {
                    rowNum++;
                    columnCounter = 0;
                }
               
                Rect makerRect = new Rect(baseHorizontalPosition + (columnPadding * columnCounter), baseVerticalPosition + (rowPadding * rowNum), itemBoxWidth, itemBoxHeight);
                GUI.Label(makerRect, maker);   
                if(GUI.Button(makerRect, "", style))
                {
                    JuiceMenu.selectedJuice.maker.name = currentMaker.name;
                    MenuManager.ChangeMenu("Juices");
                }
                    
                columnCounter++;	
        }


        if (GUI.Button(new Rect(saveBoxLeft, saveBoxTop, saveBoxWidth, saveBoxHeight), "+"))
        {

        }
        

    }
}
