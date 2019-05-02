using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinBattleText : MonoBehaviour {

    public List<Text> textBoxes;

	// Use this for initialization
	void Start ()
    {
        Vector2 newPos = new Vector2(PlayerPrefs.GetFloat("SceneOriginX"), PlayerPrefs.GetFloat("SceneOriginY"));
        PersistantData.SetPlayerPositionInNextScene(newPos);
    }
	
	// Update is called once per frame
	void Update ()
    {

        textBoxes[0].text = "The party gained " + PlayerPrefs.GetInt("Experience") + " experience each!";
        textBoxes[1].text = "The enemies dropped " + PlayerPrefs.GetInt("Gold") + " gold!";
        if(PlayerPrefs.GetString("Player0") != "")
        {
            textBoxes[2].text = PlayerPrefs.GetString("Player0") + " has left the party due to discomfort!";
        }
        if (PlayerPrefs.GetString("Player1") != "")
        {
            textBoxes[3].text = PlayerPrefs.GetString("Player1") + " has left the party due to discomfort!";
        }
        if (PlayerPrefs.GetString("Player2") != "")
        {
            textBoxes[4].text = PlayerPrefs.GetString("Player2") + " has left the party due to discomfort!";
        }
        textBoxes[5].text = "Press A to continue!";

        if (Input.GetButtonDown("A"))
        {
            SceneManager.LoadScene(PlayerPrefs.GetInt("SceneOrigin"), LoadSceneMode.Single);
        }
    }
}
