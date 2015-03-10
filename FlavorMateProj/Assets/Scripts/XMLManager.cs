using UnityEngine;
using System.Collections;
using System.Xml;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using System.Security;
using System.Linq;

//This class updates/deletes/creates/etc the XML document used for local storage.
public class XMLManager : MonoBehaviour
{
   

    //this function creates/updates the xml files with whatever is 
    //currently held in "Session.myJuices"
    public static bool UpdateXMLFile()
    {
        if (Session.clientID != 0)
        {
            try
            {
                XmlDocument root = new XmlDocument();
                XmlElement juiceParent = root.CreateElement("Juices");

                foreach (Juice juice in Session.myJuices)
                {
                    string innerXMLString = "";
                    innerXMLString += "<Juice name='" + juice.name +
                        "' description='" + juice.description +
                        "' makerID='" + juice.maker.makerID +
                        "' juiceID='" + juice.juiceID +
                        "' ingredients='";

                    foreach (Ingredient ing in juice.ingredients)
                        innerXMLString += ing.ingredientID + ",";

                    innerXMLString += "' />";
                    juiceParent.InnerXml += innerXMLString;
                }

                juiceParent.InnerXml += "<Info clientID='" + Session.clientID + "' />";

                root.AppendChild(juiceParent);

                root.Save(Session.xmlPathString);
                return true;
            }
            catch (Exception ex)
            {
                print(ex.Message);
                return false;
            }
        }
        else
        {
            print("Cant update XML without being logged in (for various reasons)");
            return false;
        }
    }

    //This function updates "Session.myJuices" with whatever is held in the XML currently
    public static bool ReadJuicesFromXML()
    {
        try
        {
            XmlTextReader reader = new XmlTextReader(Session.xmlPathString);
            Session.myJuices.Clear();

            while (reader.Read())
            {
                if (reader.Name == "Juice")
                {
                    Juice newJuice = new Juice();
                    newJuice.description = reader.GetAttribute("description");
                    newJuice.juiceID = Convert.ToInt32(reader.GetAttribute("id"));
                    newJuice.name = reader.GetAttribute("name");
                    newJuice.maker = Session.GetMakerByID(Convert.ToInt32(reader.GetAttribute("makerID")));

                    string ingredientString = reader.GetAttribute("ingredients");
                    List<string> newIngredientsStrings = ingredientString.Split(',').ToList<string>();
                    List<Ingredient> newIngredients = new List<Ingredient>();

                    foreach (string s in newIngredientsStrings)
                    {
                        try
                        {
                            Ingredient newIngredient = new Ingredient();
                            int ingredientID = Convert.ToInt32(s);
                            newIngredient.ingredientID = ingredientID;
                            newIngredient.name = Session.GetIngredientByID(ingredientID).name;
                            newIngredients.Add(newIngredient);
                        }
                        catch { }
                    }

                    newJuice.ingredients = newIngredients;
                    Session.myJuices.Add(newJuice);
                }
                else if (reader.Name == "Info")
                {
                    try { Session.clientID = Convert.ToInt32(reader.GetAttribute("clientID")); }
                    catch (Exception ex) { print(ex); }
                }
            }

            return true;
        }
        catch (Exception ex)
        {
            print(ex.Message);
            return false;
        }
    }

    void OnGUI()
    {
        /*
        if (GUILayout.Button("Update XML Document (with current juices)"))
            UpdateXMLFile();
        if (GUILayout.Button("Read Juices From XML"))
            ReadJuicesFromXML();
         * */   
    }

}
