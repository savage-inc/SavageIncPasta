using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TeleportButton : MonoBehaviour {
    public bool OffLevelTeleport = false;
    public new char tag;
    public int scene;
    private Button btn;


    private TeleportGO _teleportManager;
    private void Awake()
    {
        btn = GetComponent<Button>();
        _teleportManager = FindObjectOfType<TeleportGO>();
    }


    // Use this for initialization
    void Start()
    {
        btn.onClick.AddListener(delegate { TaskOnClick(OffLevelTeleport, tag, scene); });
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    void TaskOnClick(bool OffLevelTeleport, char tag, int scene)
    {
        print("bop");
        _teleportManager.onLevel(OffLevelTeleport, tag, scene);

    }
}
