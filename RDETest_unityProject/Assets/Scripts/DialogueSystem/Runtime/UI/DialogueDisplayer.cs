using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using XomracCore.DialogueSystem.SerializedData;

namespace XomracCore.DialogueSystem
{
    public class DialogueDisplayer : MonoBehaviour
    {
        [SerializeField] private GameObject _canvas;
        [SerializeField] private Image _portrait;
        [SerializeField] private TextMeshProUGUI _speakerNameLabel;
        [SerializeField] private TextMeshProUGUI _beatLabel;
        [SerializeField] private Button _continueButton;
        [SerializeField] private GameObject _choicesContainer;
        [SerializeField] private Button[] _choiceButtons;
        
        private DialoguePlayer _currentPlayer;
		private bool _isPrinting;

        private void Awake()
        {
            _canvas.SetActive(false);
            _choicesContainer.gameObject.SetActive(false);
        }

        public void ShowDialogue(DialoguePlayer player)
        {
            Debug.Log("Showing Dialogue");
            _canvas.SetActive(true);
            _currentPlayer = player;
            _continueButton.onClick.RemoveAllListeners();
            _continueButton.onClick.AddListener(() =>
            {
                if (_isPrinting)
                {
                    BeatPrinter.SpeedUp(0.01f,0.01f);
                    return;
                }
                _currentPlayer.ContinueDialogue();
            });
            _currentPlayer.DialogueFinished += () => _canvas.SetActive(false);
            _currentPlayer.NewNodeReached += ShowBeat;
        }

        private void ShowBeat(BeatNodeData beat)
        {
            _portrait.sprite = beat.speaker.Portrait;
            _speakerNameLabel.text = beat.speaker.Name;
            _beatLabel.gameObject.SetActive(!string.IsNullOrEmpty(beat.beat));
            _beatLabel.text = beat.beat;
            _isPrinting = true;
            BeatPrinter.PrintBeat(_beatLabel, beat.beat,.1f,this, () =>
            {
                _isPrinting = false;
            });
            ShowChoices(beat);
        }

        private void ShowChoices(BeatNodeData beat)
        {
            _choicesContainer.SetActive(false);
            if (beat.choices == null || beat.choices.Count == 0) return;
            _continueButton.interactable = false;
            _choicesContainer.SetActive(true);
            for (int i = 0; i < _choiceButtons.Length; i++)
            {
                if (i >= beat.choices.Count)
                {
                    _choiceButtons[i].gameObject.SetActive(false);
                    continue;
                }
                _choiceButtons[i].gameObject.SetActive(true);
                string displayedValue = beat.choices[i].displayedValue;
                _choiceButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = displayedValue;
                _choiceButtons[i].onClick.RemoveAllListeners();
                _choiceButtons[i].onClick.AddListener(() =>
                {
                    _currentPlayer.ContinueDialogue(displayedValue);
                    _continueButton.interactable = true;
                });
            }
        }

        

        

        
        
        
    }

}
