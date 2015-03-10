using UnityEngine;
using System.Collections;
using System.Text;
using System.Security;
using System.Collections.Generic;
using System.Linq;
using System;
using System.IO;

public class FlavorMateSQL : MonoBehaviour
{
    private bool savingJuice = false;
    private bool savingMaker = false;

    void Awake()
    {
        //only 1 non-destroyed SQL object at a time
        if (GameObject.FindGameObjectsWithTag("SQL").Length > 1)
            Destroy(gameObject);
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);

        Ingredient newIngredient = new Ingredient();
        newIngredient.name = "Vanilla";
        newIngredient.ingredientID = 1;
        Session.myIngredients.Add(newIngredient);
        newIngredient = new Ingredient();
        newIngredient.name = "Chocolate";
        newIngredient.ingredientID = 2;
        Session.myIngredients.Add(newIngredient);
        newIngredient = new Ingredient();
        newIngredient.name = "Sprite";
        newIngredient.ingredientID = 3;
        Session.myIngredients.Add(newIngredient);
        newIngredient = new Ingredient();
        newIngredient.name = "Mango";
        newIngredient.ingredientID = 4;
        Session.myIngredients.Add(newIngredient);
        newIngredient = new Ingredient();
        newIngredient.name = "Mocha";
        newIngredient.ingredientID = 5;
        Session.myIngredients.Add(newIngredient);
        newIngredient = new Ingredient();
        newIngredient.name = "Boba";
        newIngredient.ingredientID = 6;
        Session.myIngredients.Add(newIngredient);
        newIngredient = new Ingredient();
        newIngredient.name = "AdidaSauceStuff";
        newIngredient.ingredientID = 7;
        Session.myIngredients.Add(newIngredient);
    }

    #region Juices

    public void JuiceUpdate(Juice juice, bool isDeleting)
    {
        if (!savingJuice)
        {
            if (!isDeleting)
                StartCoroutine("SaveJuiceRoutine", juice);
            else
                StartCoroutine("DeleteJuiceRoutine", juice);
        }
    }

    private IEnumerator SaveJuiceRoutine(Juice juice)
    {
        savingJuice = true;


        string ingredients = "";
        foreach (Ingredient i in juice.ingredients)
            ingredients += i.ingredientID + ",";
        ingredients.TrimEnd(',');

        WWWForm form = new WWWForm();
        form.AddField("clientID", Session.clientID);
        form.AddField("id", juice.juiceID);
        form.AddField("name", juice.name.Replace(',', ' ').Replace('|', ' ').Replace(';', ' '));
        form.AddField("description", juice.description);
        form.AddField("makerID", juice.maker.makerID);
        form.AddField("ingredients", ingredients);
        form.AddField("isDeleting", 0);
        form.AddField("hash", Session.Md5Sum(SecurityKeys.secretKey).ToLower());

        WWW www = new WWW(SecurityKeys.updateJuiceURL, form);

        yield return www;

        print(www.text);
        savingJuice = false;
    }

    private IEnumerator DeleteJuiceRoutine(Juice juice)
    {
        savingJuice = true;

        string ingredients = "";
        foreach (Ingredient i in juice.ingredients)
            ingredients += i.ingredientID + ",";
        ingredients.TrimEnd(',');

        WWWForm form = new WWWForm();
        form.AddField("clientID", Session.clientID);
        form.AddField("id", juice.juiceID);
        form.AddField("name", juice.name);
        form.AddField("description", juice.description);
        form.AddField("makerID", juice.maker.makerID);
        form.AddField("ingredients", ingredients);
        form.AddField("isDeleting", 1);
        form.AddField("hash", Session.Md5Sum(SecurityKeys.secretKey).ToLower());

        WWW www = new WWW(SecurityKeys.updateJuiceURL, form);

        yield return www;

        print(www.text);
        savingJuice = false;
    }

    #endregion

    #region Makers

    public void MakerUpdate(Maker maker, bool isDeleting)
    {
        if (!savingMaker)
        {
            if (!isDeleting)
                StartCoroutine("SaveMakerRoutine", maker);
            else
                StartCoroutine("DeleteMakerRoutine", maker);
        }
    }

    private IEnumerator SaveMakerRoutine(Maker maker)
    {
        savingMaker = true;

        WWWForm form = new WWWForm();
        form.AddField("clientID", Session.clientID);
        form.AddField("id", maker.makerID);
        form.AddField("name", maker.name);
        form.AddField("isDeleting", 0);
        form.AddField("hash", Session.Md5Sum(SecurityKeys.secretKey).ToLower());

        WWW www = new WWW(SecurityKeys.updateMakerURL, form);

        yield return www;

        print(www.text);
        savingMaker = false;
    }

    private IEnumerator DeleteMakerRoutine(Maker maker)
    {
        savingMaker = true;

        WWWForm form = new WWWForm();
        form.AddField("clientID", Session.clientID);
        form.AddField("id", maker.makerID);
        form.AddField("name", maker.name);
        form.AddField("isDeleting", 1);
        form.AddField("hash", Session.Md5Sum(SecurityKeys.secretKey).ToLower());

        WWW www = new WWW(SecurityKeys.updateMakerURL, form);

        yield return www;

        print(www.text);
        savingMaker = false;
    }

    #endregion

    #region ManagerMode

    public void attemptManagerLogin(string password)
    {
        StartCoroutine("AttemptManagerLogin", password);
    }

    private IEnumerator AttemptManagerLogin(string password)
    {
        print("Attempting to login to manager mode");
        WWWForm form = new WWWForm();
        form.AddField("clientID", Session.clientID);
        form.AddField("password", password);

        form.AddField("hash", Session.Md5Sum(SecurityKeys.secretKey).ToLower());

        WWW www = new WWW(SecurityKeys.managerLoginURL, form);

        yield return www;

        if (www.text == "Success")
        {
            Session.isManagerMode = true;
            print("Manager Mode activated");
        }
        else
        {
            print("Error: " + www.text);
            print("Error: " + www.error);
        }
    }

    public void logoutManager()
    {
        Session.isManagerMode = false;
    }

    #endregion


}

public static class Session
{
    public static int clientID = 0;

    public static string logoURL = "";
    public static string name = "";
    public static string xmlPathString
    {
        get
        {
            return Path.Combine(Application.persistentDataPath, "Data.xml");
        }
    }

    public static bool isManagerMode = false;

    public static List<Juice> myJuices = new List<Juice>();

    public static List<Maker> myMakers = new List<Maker>();
    public static List<Ingredient> myIngredients = new List<Ingredient>();

    public static DateTime LastUpdate
    {
        get
        {
            if (PlayerPrefs.HasKey("LastUpdate"))
                return Convert.ToDateTime(PlayerPrefs.GetString("LastUpdate"));
            else
                return Convert.ToDateTime(DateTime.Now);

        }
    }

    public static void JuiceUpdateSQL(Juice juice, bool isDeleting)
    {
        GameObject.FindGameObjectWithTag("SQL").GetComponent<FlavorMateSQL>().JuiceUpdate(juice, isDeleting);
    }
    public static void MakerUpdateSQL(Maker maker, bool isDeleting)
    {
        GameObject.FindGameObjectWithTag("SQL").GetComponent<FlavorMateSQL>().MakerUpdate(maker, isDeleting);
    }

    public static bool MakersContains(int makerID)
    {
        foreach (Maker m in Session.myMakers)
            if (m.makerID == makerID)
                return true;

        return false;
    }

    public static Maker GetMakerByID(int id)
    {
        Maker returnMaker = new Maker();

        foreach (Maker m in Session.myMakers)
            if (m.makerID == id)
                returnMaker = m;

        return returnMaker;
    }
    public static Maker GetMakerByName(string name)
    {
        Maker returnMaker = new Maker();

        foreach (Maker m in Session.myMakers)
            if (m.name.ToLower() == name.ToLower())
                returnMaker = m;

        return returnMaker;
    }

    public static Ingredient GetIngredientByID(int id)
    {
        Ingredient returnIngredient = new Ingredient();

        foreach (Ingredient ingredient in Session.myIngredients)
            if (ingredient.ingredientID == id)
                returnIngredient = ingredient;

        return returnIngredient;
    }
    public static Ingredient GetIngredientByName(string name)
    {
        Ingredient returnIngredient = new Ingredient();

        foreach (Ingredient ingredient in Session.myIngredients)
            if (ingredient.name.ToLower() == name.ToLower())
                returnIngredient = ingredient;

        return returnIngredient;
    }

    public static Juice GetJuiceByID(int id)
    {
        Juice returnJuice = new Juice();

        foreach (Juice juice in Session.myJuices)
            if (juice.juiceID == id)
                returnJuice = juice;

        return returnJuice;
    }
    public static Juice GetJuiceByName(string name)
    {
        Juice returnJuice = new Juice();

        foreach (Juice juice in Session.myJuices)
            if (juice.name.ToLower() == name.ToLower())
                returnJuice = juice;

        return returnJuice;
    }

    public static void SortJuices()
    {
        List<int> selectedIngredients = new List<int>();

        foreach (Ingredient ingredient in myIngredients)
            if (ingredient.isSelected)
                selectedIngredients.Add(ingredient.ingredientID);

        foreach (Juice juice in myJuices)
        {
            bool displayed = false;

            //laziest sort of my life
            foreach (Ingredient juiceIngredient in juice.ingredients)
                if (selectedIngredients.Contains(juiceIngredient.ingredientID))
                    displayed = true;

            juice.isDisplayed = displayed;
            Debug.Log(juice.name + " displayed: " + displayed);
        }

    }

    public static string Md5Sum(string input)
    {
        System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
        byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
        byte[] hash = md5.ComputeHash(inputBytes);

        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < hash.Length; i++)
        {
            sb.Append(hash[i].ToString("X2"));
        }
        return sb.ToString();
    }
}


public static class SecurityKeys
{
    public static string secretKey = "87523";
    public static string loginURL = "http://martianmediainc.com/ecigphp/login.php?";
    public static string getJuicesURL = "http://martianmediainc.com/ecigphp/getJuices.php?";
    public static string updateJuiceURL = "http://martianmediainc.com/ecigphp/updateJuice.php?";
    public static string getMakersURL = "http://martianmediainc.com/ecigphp/getMakers.php?";
    public static string updateMakerURL = "http://martianmediainc.com/ecigphp/updateMakers.php?";
    public static string managerLoginURL = "http://martianmediainc.com/ecigphp/managerLogin.php?";

}
public static class EnumUtil
{
    public static IEnumerable<T> GetValues<T>()
    {
        return Enum.GetValues(typeof(T)).Cast<T>();
    }
}

public class Juice
{
    public string name;
    public string description;
    public Maker maker;
    public int juiceID;
    public List<Ingredient> ingredients = new List<Ingredient>();
    public bool isDisplayed = true;


    public Juice(string jName = "(New Juice)", string jDescription = "(New Description)")
    {
        name = jName;
        description = jDescription;
        Maker newMaker = new Maker();
       // Debug.Log(newMaker.name);
        maker = newMaker;
        isDisplayed = true;

    }
}

public class Ingredient
{
    public string name;
    public int ingredientID;
    public bool isSelected = true;
    public int groupID;

    public Ingredient(string iName = "(New Ingredient)", int Id = 0, bool selected = true)
    {
        name = iName;
        ingredientID = Id;
        isSelected = selected;
    }
}

public class Maker
{
    public string name;
    public int makerID;

    public Maker(string mName = "(Select Maker)", int Id = 0)
    {
        name = mName;
        makerID = Id;
    }

}

