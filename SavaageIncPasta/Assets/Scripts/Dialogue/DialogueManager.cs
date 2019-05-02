using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using UnityEngine.UI;
using System.IO;


public class DialogueManager : MonoBehaviour {
    public static DialogueManager instance;

    public string Message;

    public Text DialogueText;
    public Button PositiveButton;
    public Button NegativeButton;

    public TextAsset DialogueXML;

    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
            //string data = DialogueXML.text;

        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
 
    }
    public void parseXML(string NPCType)
    {
        XmlDocument xmlDoc = new XmlDocument();
        TextAsset textAsset = (TextAsset)Resources.Load("DialogueLines");
        xmlDoc.LoadXml(textAsset.text);


        XmlNode FindAmountOfDialogues = xmlDoc.SelectSingleNode("/NPCS/NPC[@type = '" + NPCType + "']");
        int _amountOfNodes = (FindAmountOfDialogues.SelectNodes("descendant::*").Count); //finds the amount of dialogue options available to the specific NPC type

        var RandomisedDialogue = Random.Range(0, _amountOfNodes);

        XmlNode ChosenDialogue = (xmlDoc.SelectSingleNode("/NPCS/NPC[@type = '" + NPCType + "']").ChildNodes[RandomisedDialogue]); //selects the correct node based on the random value
        Message = ChosenDialogue.InnerXml;
    }

    public void Talk()
    {
        DialogueText.text = Message;
    }
  
}



