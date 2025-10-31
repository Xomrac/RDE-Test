

using System;
using UnityEditor;
using UnityEngine;

namespace XomracCore.DialogueSystem
{

	using System.Collections.Generic;
	using UnityEditor.Experimental.GraphView;
	using UnityEditor.UIElements;
	using UnityEngine.UIElements;
	using XomracCore.DialogueSystem.SerializedData;

	public class BeatNodeDisplayer : ANodeDisplayer
	{
		const float MAX_SPRITE_WIDTH = 256;
		const float MAX_SPRITE_HEIGHT = 256;
		private Image _portrait;
		private TextField _beatField;
		private ObjectField _speaker;
		private readonly List<DialogueChoice> _choices = new();
		public List<DialogueChoice> Choices => _choices;

		private Port _defaultOutputPort;
		public Speaker Speaker => _speaker.value as Speaker;
		public string DisplayedBeat => _beatField.value;

		public BeatNodeDisplayer()
		{
			mainContainer.style.flexDirection = FlexDirection.Column;
			mainContainer.style.marginBottom = 0;
			mainContainer.style.paddingBottom = 0;
			mainContainer.style.flexGrow = 1;

			CreateSpeakerField();
			_portrait = CreatePortraitImage();
			mainContainer.Insert(0, _portrait);
			inputContainer.Add(CreateInputPort());
			titleButtonContainer.Add(CreateAddChoiceButton());
			extensionContainer.Add(_speaker);
			mainContainer.Add(CreateBeatField());
			mainContainer.Add(_beatField);

			UpdatePortrait();
			UpdateOutputPorts();
			RefreshExpandedState();
			RefreshPorts();
		}

		private Image CreatePortraitImage()
		{
			var img = new Image
			{
				image = null,
				scaleMode = ScaleMode.ScaleToFit,
				style =
				{
					alignSelf = Align.Center
				}
			};
			return img;
		}

		private void UpdatePortrait()
		{
			var sprite = GetSpeakerSprite();
			if (sprite != null)
			{
				float spriteWidth = sprite.rect.width;
				float spriteHeight = sprite.rect.height;

				float scale = Math.Min(MAX_SPRITE_WIDTH / spriteWidth, MAX_SPRITE_HEIGHT / spriteHeight);
				
				
				float ppp = EditorGUIUtility.pixelsPerPoint;
				float displayWidth = spriteWidth * scale / ppp;
				float displayHeight = spriteHeight * scale / ppp;

				_portrait.style.width = displayWidth;
				_portrait.style.height = displayHeight;
				_portrait.image = sprite.texture;
				_portrait.visible = true;
			}
			else
			{
				_portrait.image = null;
				_portrait.visible = false;
			}
		}
		

		private Sprite GetSpeakerSprite()
		{
			return Speaker == null ? null : Speaker.Portrait;
		}

		private Button CreateAddChoiceButton()
		{
			var addChoiceButton = new Button(() => { AddChoice("Choice " + (_choices.Count + 1)); })
			{
				text = "+"
			};
			return addChoiceButton;
		}

		#region Builder Methods

		public BeatNodeDisplayer WithChoices(List<DialogueChoice> newChoices)
		{
			_choices.Clear();
			_choices.AddRange(newChoices);
			UpdateOutputPorts();
			return this;
		}

		public BeatNodeDisplayer WithSpeaker(Speaker newSpeaker)
		{
			_speaker.value = newSpeaker;
			UpdatePortrait();
			return this;
		}

		public BeatNodeDisplayer WithBeat(string newBeat)
		{
			_beatField.value = newBeat;
			return this;
		}

		#endregion

		private Label CreateBeatField()
		{
			var label = new Label("Beat:")
			{
				style =
				{
					marginBottom = 4
				}
			};

			_beatField = new TextField
			{
				multiline = true,
				style =
				{
					minHeight = 0,
					flexGrow = 1,
					marginTop = 0,
					marginBottom = 0
				}
			};
			_beatField.RegisterCallback<FocusOutEvent>(_ => View.SaveGraph());
			return label;
		}

		private void CreateSpeakerField()
		{
			_speaker = new ObjectField("Speaker")
			{
				objectType = typeof(Speaker),
				allowSceneObjects = false
			};
			_speaker.RegisterValueChangedCallback(_ =>
			{
				View?.SaveGraph();
				UpdatePortrait();
			});
		}

		private void AddChoice(string choiceText)
		{
			_choices.Add(new DialogueChoice
			{
				displayedValue = choiceText
			});
			UpdateOutputPorts();
			View?.SaveGraph();
		}

		private void RemoveChoice(string guid)
		{
			int choiceIndex = _choices.FindIndex(choice => choice.portGuid == guid);
			if (choiceIndex >= 0)
			{
				_choices.RemoveAt(choiceIndex);
				UpdateOutputPorts();
				View?.SaveGraph();
			}
		}

		private void UpdateOutputPorts()
		{
			outputContainer.Clear();
			if (_choices.Count == 0)
			{
				outputContainer.Add(CreateOutputPort());
				return;
			}

			foreach (var choice in _choices)
			{
				var portContainer = new VisualElement
				{
					style =
					{
						flexDirection = FlexDirection.Row,
						alignItems = Align.Center,
						marginTop = 2,
						marginBottom = 2
					}
				};

				portContainer.Add(CreateRemoveChoiceButton(choice));
				portContainer.Add(CreateChoiceField(choice));
				portContainer.Add(CreateOutputPort("", choice.displayedValue));
				outputContainer.Add(portContainer);
			}

			RefreshExpandedState();
			RefreshPorts();
		}

		private Button CreateRemoveChoiceButton(DialogueChoice choice)
		{
			var removeButton = new Button(() => RemoveChoice(choice.portGuid))
			{
				text = "–",
				style =
				{
					width = 20,
					marginLeft = 4
				}
			};
			return removeButton;
		}

		private TextField CreateChoiceField(DialogueChoice choice)
		{
			var choiceField = new TextField
			{
				value = choice.displayedValue,
				multiline = false,
				style =
				{
					flexGrow = 1,
					minWidth = 60,
					marginLeft = 6
				}
			};
			choiceField.RegisterValueChangedCallback(evt =>
			{
				choice.displayedValue = evt.newValue;
				View?.SaveGraph();
			});
			return choiceField;
		}

	}

}