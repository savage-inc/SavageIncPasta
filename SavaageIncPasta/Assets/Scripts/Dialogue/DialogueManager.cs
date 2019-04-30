using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using UnityEngine.UI;
using System.IO;


public class DialogueManager : MonoBehaviour {
    public static DialogueManager instance;


    public string NPCName;
    public string Message;
    public string i = "ShopKeeper";

    public TextAsset DialogueXML;




    private void Awake()
    {

        if (instance == null)
        {
            instance = this;

        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
 
    }
    private void Start()
    {
        string data = DialogueXML.text;
        _parseXML(data);
    }
    private void _parseXML(string xmlData)
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load("Assets\\Resources\\DialogueLines.xml");
        string xmlPath = "//NPCS/NPC";

        XmlNodeList DialogueOptions = xmlDoc.SelectNodes("/NPCS/NPC[@type = 'ShopKeeper']");
        Debug.Log("testing\n");
        foreach (XmlNode node in DialogueOptions)
        {
            
            XmlNode name = node.FirstChild;
            XmlNode dialogue = name.NextSibling;
            Debug.Log(name.InnerXml);
            Debug.Log(dialogue.InnerXml);
        }


    }
}



