using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(CircleCollider2D))]
public class SceneDoor : MonoBehaviour
{
    public string SceneName;
    [TextArea(2,6)]
    public string DialogueText;
    public Vector2 PositionInNewScene;

    private PlayerManager _playerManager;
    private DialogueManager _dialogueManager;
    private UIManager _uiManager;
    private EventSystem _eventSystem;
    private bool _onStart;

    // Use this for initialization
    void Awake () {
        _playerManager = FindObjectOfType<PlayerManager>();
        _dialogueManager = FindObjectOfType<DialogueManager>();
        _uiManager = FindObjectOfType<UIManager>();
        _eventSystem = FindObjectOfType<EventSystem>();
    }

    private void Start()
    {
        StartCoroutine(onStart());
    }

    IEnumerator onStart()
    {
        _onStart = true;
        yield return new WaitForSeconds(1);
        _onStart = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_onStart)
        {
            return;
        }
        if (_dialogueManager != null)
        {
            _dialogueManager.DialogueText.transform.parent.GetChild(0).GetComponent<Text>().text = "Change Level:";
            _dialogueManager.Message = DialogueText;
            _uiManager.OpenDialogueBox();
            _dialogueManager.Talk();
            _dialogueManager.PositiveButton.onClick.AddListener(ChangeScene);
            _dialogueManager.PositiveButton.gameObject.SetActive(true);
            _dialogueManager.NegativeButton.onClick.AddListener(_uiManager.Close);
            _eventSystem.SetSelectedGameObject(null, null);
            _eventSystem.SetSelectedGameObject(_dialogueManager.PositiveButton.gameObject);

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _uiManager.Close();

        _dialogueManager.PositiveButton.onClick.RemoveAllListeners();
        _dialogueManager.NegativeButton.onClick.RemoveAllListeners();

    }

    void ChangeScene()
    {
        _uiManager.Close();
        if (_playerManager.Characters.Count > 0)
        {
            var scene = SceneManager.GetSceneByName(SceneName);
            if (!scene.IsValid())
            {
                //Save Data
                var worldManager = FindObjectOfType<WorldManager>();
                if (worldManager != null)
                {
                    worldManager.SaveWorld();
                }

                //Get the player
                PersistantData.SetPlayerPositionInNextScene(PositionInNewScene);
                SceneManager.LoadScene(SceneName);
            }
        }
    }
}
