using Ink.Runtime;
using ISN.SO;
using ISN.Text;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ISN.Manager
{
    public class VNManager : MonoBehaviour
    {
        public static VNManager Instance { private set; get; }

        [SerializeField]
        private TextDisplay _display;

        private SO.CharacterInfo _currentSpeaker;
        private string _speakerNameOverrides;

        private Story _story;

        [SerializeField]
        private GameObject _container;

        [SerializeField]
        private GameObject _namePanel;

        [SerializeField]
        private TMP_Text _nameText;

        [SerializeField]
        private Transform _choiceContainer;

        [SerializeField]
        private GameObject _choicePrefab;

        private bool _isSkipEnabled;
        private float _skipTimer;
        private float _skipTimerRef = .1f;

        private void Awake()
        {
            Instance = this;

            _display.OnDisplayDone += (_sender, _e) =>
            {
                if (_story.currentChoices.Any())
                {
                    ResetVN();
                    foreach (var choice in _story.currentChoices)
                    {
                        var button = Instantiate(_choicePrefab, _choiceContainer);
                        button.GetComponentInChildren<TMP_Text>().text = choice.text;

                        var elem = choice;
                        button.GetComponent<Button>().onClick.AddListener(() =>
                        {
                            _story.ChoosePath(elem.targetPath);
                            for (int i = 0; i < _choiceContainer.childCount; i++)
                                Destroy(_choiceContainer.GetChild(i).gameObject);
                            DisplayStory(_story.Continue());
                        });
                    }
                }
            };
        }

        public bool IsPlayingStory => _container.activeInHierarchy;

        private void Update()
        {
            if (_isSkipEnabled)
            {
                _skipTimer -= Time.deltaTime;
                if (_skipTimer < 0)
                {
                    _skipTimer = _skipTimerRef;
                    DisplayNextDialogue();
                }
            }
        }

        private void ResetVN()
        {
            _isSkipEnabled = false;

            _container.SetActive(true);
            /*
            if (_currentSpeaker != null)
            {
                _characterImage.gameObject.SetActive(true);
            }
            */
            _choiceContainer.gameObject.SetActive(true);
        }

        public void ShowStory(TextAsset asset, Dictionary<string, object> variables = null)
        {
            _currentSpeaker = null;
            _story = new(asset.text);
            if (variables != null)
            {
                foreach (var v in variables)
                {
                    _story.variablesState[v.Key] = v.Value;
                }
            }
            ResetVN();
            DisplayStory(_story.Continue());
        }

        private void DisplayStory(string text)
        {
            _container.SetActive(true);
            _namePanel.SetActive(false);

            foreach (var tag in _story.currentTags)
            {
                var s = tag.Split(' ');
                var content = string.Join(' ', s.Skip(1)).ToLowerInvariant();
                switch (s[0])
                {
                    case "speaker":
                        if (content == "none") _currentSpeaker = null;
                        else
                        {
                            _currentSpeaker = ResourceManager.Instance.GetMapResource<SO.CharacterInfo>(content);
                            if (_currentSpeaker == null)
                            {
                                Debug.LogError($"[STORY] Unable to find character {content}");
                            }
                        }
                        break;

                    case "name":
                        if (content == "none") _speakerNameOverrides = null;
                        else _speakerNameOverrides = content;
                        break;

                    default:
                        Debug.LogError($"Unknown story key: {s[0]}");
                        break;
                }
            }
            _display.ToDisplay = text;
            if (_currentSpeaker == null && _speakerNameOverrides == null)
            {
                _namePanel.SetActive(false);
                //_characterImage.gameObject.SetActive(false);
            }
            else
            {
                _namePanel.SetActive(true);
                _nameText.text = _speakerNameOverrides ?? _currentSpeaker.DisplayName;
                // TODO: If VN sprites are added, add this back
                //_characterImage.gameObject.SetActive(true);
                // _characterImage.sprite = _currentCharacter.Image;
            }
        }

        public void DisplayNextDialogue()
        {
            if (!_container.activeInHierarchy)
            {
                return;
            }
            if (!_display.IsDisplayDone)
            {
                // We are slowly displaying a text, force the whole display
                _display.ForceDisplay();
            }
            else if (_story.canContinue && // There is text left to write
                !_story.currentChoices.Any()) // We are not currently in a choice
            {
                DisplayStory(_story.Continue());
            }
            else if (!_story.canContinue && !_story.currentChoices.Any())
            {
                _container.SetActive(false);
            }
        }

        public void ToggleSkip()
        {
            _isSkipEnabled = !_isSkipEnabled;
        }

        public void OnNextDialogue()
        {
            if (!_isSkipEnabled)
            {
                if (_container.activeInHierarchy)
                {
                    /*
                    // If we click on a button, we don't advance the 
                    PointerEventData pointerEventData = new(EventSystem.current)
                    {
                        position = Mouse.current.position.ReadValue()
                    };
                    List<RaycastResult> raycastResultsList = new List<RaycastResult>();
                    EventSystem.current.RaycastAll(pointerEventData, raycastResultsList);
                    for (int i = 0; i < raycastResultsList.Count; i++)
                    {
                        if (raycastResultsList[i].gameObject.TryGetComponent<Button>(out var _))
                        {
                            return;
                        }
                    }*/

                    ResetVN();
                    DisplayNextDialogue();
                }
                else
                {
                    // Hide mode is active
                    _container.SetActive(true);
                }
            }
        }

        public void OnSkip(bool toggle)
        {
            _isSkipEnabled = toggle;
        }
    }
}