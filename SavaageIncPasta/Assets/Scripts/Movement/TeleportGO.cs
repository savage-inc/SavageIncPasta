using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TeleportGO : MonoBehaviour {

	public static TeleportGO instance;
	public GameObject teleportedObject;
	public string[] scenes;



	private TeleportDestination[] _destination;

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


	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {

		
	}
	public void onLevel(bool OffLevelTeleport, char tag, int scene)
	{
        if (OffLevelTeleport != true)
        {
            print("in scene");
            teleport(tag);
        }
        else
        {
            print("off scene");
            DontDestroyOnLoad(teleportedObject);
            StartCoroutine(levelLoad(tag, scene));
            print("coroutine started");
        }
	}
    private IEnumerator levelLoad(char tag, int scene)
    {
        print("levelLoading");
        yield return SceneManager.LoadSceneAsync(scenes[scene]);
        teleport(tag);
    }




    void teleport(char tag)
	{
		//gathers all the destinations available
		_destination = FindObjectsOfType<TeleportDestination>();
		for (int i = 0; i < _destination.Length; i++)
		{
			if (_destination[i].tag == tag)
			{
				teleportedObject.transform.position = _destination[i].transform.position;
                
				break;
			}
			else
			{
				print("no destination target available");
			}
		}
		//find all destinations
		//if the destination tag is the same as the teleporter tag then allow teleport
		//otherwise if theres duplicate dont allow
		//otherwise if its not found dont allow
	}
}
