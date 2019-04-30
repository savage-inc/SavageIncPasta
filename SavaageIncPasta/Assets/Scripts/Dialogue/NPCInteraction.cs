using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCInteraction : MonoBehaviour {



    public bool IsVendor;
	public bool IsQuestGiver;
	public bool IsClanRecruiter;
    public bool isTownie;
    public bool isBeggar;

    public string Name;

    public Text NPCName;
	public Button PositiveButton;
	public Button NegativeButton;


	private UIManager _uiManager;
    private DialogueManager _diManager;


	private void Awake()
	{
		_uiManager = FindObjectOfType<UIManager>();
        _diManager = FindObjectOfType<DialogueManager>();
        NPCName.text = Name;
	}




    // Use this for initialization
    void OnTriggerStay2D(Collider2D other)
    {
        if (Input.GetButtonDown("X") || Input.GetKeyDown(KeyCode.E))
        {
            if (!_uiManager.DialogueBox.gameObject.activeInHierarchy)
            {

                _uiManager.OpenDialogueBox();
                _diManager.Talk();
            }
            else
            {
                _uiManager.Close();
            }
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    { 
        if (IsClanRecruiter)
		{
            _diManager.parseXML("ClanManager");
            IsVendor = false;
            isTownie = false;
            isBeggar = false;
			PositiveButton.gameObject.SetActive(true);
			PositiveButton.onClick.AddListener(_uiManager.OpenClanUI);
		}
		else if(IsVendor)
		{
            _diManager.parseXML("ShopKeeper");
            isBeggar = false;
            isTownie = false;
            //PositiveButton.gameObject.SetActive(true);
            //PositiveButton.onClick.AddListener(_uiManager.OpenShopUI);
        }
		else if(isTownie)
		{
            _diManager.parseXML("Townie");
            PositiveButton.gameObject.SetActive(false);
		}
        else
        {
            _diManager.parseXML("Beggar");
            PositiveButton.gameObject.SetActive(false);
        }
	}
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.E))
		{

		}
		NegativeButton.onClick.AddListener(_uiManager.Close);
	}
}
