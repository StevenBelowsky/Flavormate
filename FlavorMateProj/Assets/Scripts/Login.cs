using UnityEngine;
using System.Collections;
using System.Text;
using System.Security;
using System.Collections.Generic;
using System.Linq;
using System;

public class Login : MonoBehaviour
{

    #region Variables

    private FlavorMateSQL flavorMateSQL;
    private ServerLoop serverLoop;
    private XMLManager xmlmanage;

    private GUIText passwordText1, passwordText2;
    private int state = 0;
    private List<string> myJuicesStrings = new List<string>();

    //0 - cant enter
    //1 - can enter
    //2 - attemptingenter

    int selGridInt = 0;
    string[] selStrings = { "Grid 1", "Grid 2", "Grid 3", "Grid 4" };

    #endregion

    void Awake()
    {

        flavorMateSQL = GameObject.FindGameObjectWithTag("SQL").GetComponent<FlavorMateSQL>();
        serverLoop = GameObject.FindGameObjectWithTag("SQL").GetComponent<ServerLoop>();
        xmlmanage = GameObject.FindGameObjectWithTag("SQL").GetComponent<XMLManager>();

        if (PlayerPrefs.GetInt("LoggedIn") == 1)
        {
            flavorMateSQL.enabled = true;
            serverLoop.enabled = true;
            xmlmanage.enabled = true;

            Session.clientID = PlayerPrefs.GetInt("ClientID");
            Destroy(gameObject);
        }
        else
            print("Not logged in yet. Have to enter key.");

        passwordText1 = GameObject.Find("enterpassword1").guiText;
        passwordText2 = GameObject.Find("enterpassword2").guiText;
        passwordText1.enabled = true;
        passwordText2.enabled = false;
    }

    void Update()
    {
        if (state == 0)
        {
            if (Input.anyKey)
            {
                state = 1;
                passwordText1.enabled = false;
                passwordText2.enabled = true;
                passwordText2.text = "";
                passwordText2.color = Color.grey;
            }
        }
        if (state == 1)
        {
            if (Input.GetKey(KeyCode.Backspace))
            {
                passwordText2.text = "";
            }
            else
            {
                passwordText2.text += Input.inputString;
                passwordText2.color = Color.white;

            }
            if (Input.GetKeyDown(KeyCode.Return))
            {
                state = 2;
                passwordText1.enabled = true;
                passwordText2.enabled = false;
                passwordText1.text = "Attempting Login...";
                StartCoroutine("AttemptLogin", passwordText2.text);
            }
        }
        if (state == 2)
        {
            if (Input.anyKey && passwordText1.text != "Attempting Login...")
            {
                state = 0;
                passwordText1.enabled = true;
                passwordText1.text = "Login Not Found\n(Hit enter to try new password)";

                passwordText2.text = "";
                passwordText2.enabled = false;
            }
        }
        if (state == 3)
        {
            if (Input.GetKeyUp(KeyCode.Return))
            {
                state = 0;
                passwordText1.enabled = true;
                passwordText1.text = "(Start typing password)";

                passwordText2.text = "";
                passwordText2.enabled = false;
            }
        }
    }

    private IEnumerator AttemptLogin(string password)
    {
        List<String> returnList = new List<string>();

        WWWForm form = new WWWForm();
        form.AddField("password", password);
        form.AddField("hash", Session.Md5Sum(password + SecurityKeys.secretKey).ToLower());

        WWW www = new WWW(SecurityKeys.loginURL, form);
        yield return www;

        if (www.text == "" || www.text.Split(';').ToList()[0] == "")
        {
            state = 3;
            passwordText1.enabled = true;
            passwordText1.text = "Login Not Found\n(Hit enter to try new password)";

            passwordText2.text = "";
            passwordText2.enabled = false;

        }
        else
        {
            returnList = www.text.Split(';').ToList();
            foreach (string s in returnList)
                print(s);
            Session.clientID = Convert.ToInt32(returnList[0]);
            Session.name = returnList[1];
            Session.logoURL = returnList[2];

            passwordText2.text = "";
            passwordText2.enabled = false;
            passwordText1.enabled = true;
            passwordText1.text = "Name: " + Session.name + "\n" +
                                            "ClientID: " + Session.clientID.ToString() + "\n" +
                                            "Logo URL: " + Session.logoURL + "\n\n";


            WWWForm newform = new WWWForm();
            newform.AddField("clientID", Session.clientID);
            newform.AddField("hash", Session.Md5Sum(Session.clientID.ToString() + SecurityKeys.secretKey).ToLower());

            WWW newWWW = new WWW(SecurityKeys.getJuicesURL, newform);

            yield return newWWW;
            myJuicesStrings = newWWW.text.Split(';').ToList();
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

            passwordText1.text += "\n\n(press enter to attempt new key)";

            PlayerPrefs.SetInt("LoggedIn", 1);
            PlayerPrefs.SetInt("ClientID", Session.clientID);
            flavorMateSQL.enabled = true;
            serverLoop.enabled = true;
            xmlmanage.enabled = true;

            passwordText1.text = "";
            passwordText2.text = "";

            passwordText1.enabled = false;
            passwordText2.enabled = false;
            
            Destroy(gameObject);
        }

        state = 3;
    }
}
