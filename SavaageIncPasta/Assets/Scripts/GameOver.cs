using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour {

    public string MainMenuScene;
    public string BaracksScene;
    public Text ButtonText;

    List<Character> clanMembers;

    private void Awake()
    {
        clanMembers = PersistantData.GetSavedClanData();
        if(clanMembers.Count > 0)
        {
            ButtonText.text = "Continue";
        }
        else
        {
            ButtonText.text = "Quit";

        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Continue()
    {
        var ClanMembers = PersistantData.GetSavedClanData();
        if(ClanMembers.Count > 0)
        {
            SceneManager.LoadScene(BaracksScene);
        }
        else
        {
            SceneManager.LoadScene(MainMenuScene);
        }
    }
}
