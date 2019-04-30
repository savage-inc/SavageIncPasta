using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCInteraction : MonoBehaviour {
	public bool IsVendor;
	public bool IsQuestGiver;
	public bool IsClanRecruiter;



	public string Name;

	[TextArea(3,20)]
	public string Message; 


	public Text DialogueText;
	public Button PositiveButton;
	public Button NegativeButton;


	private UIManager _uiManager;


	private void Awake()
	{
		_uiManager = FindObjectOfType<UIManager>();
	}




	// Use this for initialization
	void OnTriggerStay2D(Collider2D other)
	{
		if (Input.GetButtonDown("X") || Input.GetKeyDown(KeyCode.E))
		{
			if (!_uiManager.DialogueBox.gameObject.activeInHierarchy)
			{
				_uiManager.OpenDialogueBox();
			}
			else
			{
				_uiManager.Close();
			}
		}
		if(IsClanRecruiter)
		{
			IsVendor = false;
			PositiveButton.gameObject.SetActive(true);
			PositiveButton.onClick.AddListener(_uiManager.OpenClanUI);
		}
		else if(IsVendor)
		{
			//PositiveButton.gameObject.SetActive(true);
			//PositiveButton.onClick.AddListener(_uiManager.OpenShopUI);
		}
		else
		{
			PositiveButton.gameObject.SetActive(false);
		}
	}
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.E))
		{
			Talk();
		}
		NegativeButton.onClick.AddListener(_uiManager.Close);
	}
	public void Talk()
	{
		DialogueText.text = Message;
	}
}
