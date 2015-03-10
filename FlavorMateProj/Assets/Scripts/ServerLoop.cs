using UnityEngine;
using System.Collections;
using System.Text;
using System.Security;
using System.Collections.Generic;
using System.Linq;
using System;


public class ServerLoop : MonoBehaviour
{

    private bool attemptingUpdate = false;

    void Start()
    {
        //StartCoroutine("_ServerLoop");
    }


    //attempt update
    //wait a full minute
    //if still attempting, stop attempting and stop coroutine.
    private IEnumerator _ServerLoop()
    {
        try
        {
            StartCoroutine("AttemptJuiceUpdate");
        }
        catch { }

        yield return new WaitForSeconds(60);

        try
        {
            attemptingUpdate = false;
            StopCoroutine("AttemptJuiceUpdate");
        }
        catch { }

        yield return new WaitForSeconds(300);

        try
        {
            StartCoroutine("_ServerLoop");
        }
        catch { }
    }

    private IEnumerator AttemptJuiceUpdate()
    {
        attemptingUpdate = true;

        WWWForm newform = new WWWForm();
        newform.AddField("clientID", Session.clientID);
        newform.AddField("hash", Session.Md5Sum(Session.clientID.ToString() + SecurityKeys.secretKey).ToLower());

        WWW newWWW = new WWW(SecurityKeys.getJuicesURL, newform);

        yield return newWWW;

        try
        {
            List<string> myJuicesStrings = new List<string>();
            myJuicesStrings = newWWW.text.Split(';').ToList();
            Session.myJuices.Clear();

            foreach (string s in myJuicesStrings)
            {

                if (s.Length > 1)
                {
                    Juice newJuice = new Juice();
                    List<string> split = s.Split('|').ToList();
                    int makerID = Convert.ToInt32(split[4]);

                    if (!Session.MakersContains(makerID))
                    {
                        string makerName = split[0];

                        Maker newMaker = new Maker();
                        newMaker.makerID = makerID;
                        newMaker.name = makerName;

                        Session.myMakers.Add(newMaker);
                        newJuice.maker = newMaker;
                    }
                    else
                    {
                        Maker newMaker = Session.GetMakerByID(makerID);
                        newJuice.maker = newMaker;
                    }

                    newJuice.name = split[1];

                    List<Ingredient> newIngredients = new List<Ingredient>();

                    List<string> ingredientsList = split[2].Split('-').ToList();
                    List<string> ingredientsNames = ingredientsList[0].Split(',').ToList();
                    List<string> ingredientsIDs = ingredientsList[1].Split(',').ToList();

                    for (int i = 0; i < ingredientsNames.Count; i++)
                    {
                        Ingredient newIngredient = new Ingredient();
                        newIngredient.name = ingredientsNames[i];
                        newIngredient.ingredientID = Convert.ToInt32(ingredientsIDs[i]);
                        newIngredients.Add(newIngredient);
                    }

                    newJuice.ingredients = newIngredients;

                    newJuice.description = split[3];
                    newJuice.juiceID = Convert.ToInt32(split[4]);

                    Session.myJuices.Add(newJuice);
                }
            }

            XMLManager.UpdateXMLFile();


        }
        catch { print("ERROR! CLICK!"); StopCoroutine("AttemptJuiceUpdate"); }

        yield return new WaitForEndOfFrame();

        attemptingUpdate = false;
    }
}
