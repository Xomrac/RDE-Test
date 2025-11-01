using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace XomracCore.DialogueSystem
{

	public class BeatPrinter
	{

		private static float _printSpeed;
		private static float _fadeSpeed = 0.05f;
		private static float? _spedUpSpeed;
		private static float? _spedUpFadeSpeed;

		private static Coroutine _printingCoroutine;

		public static void SpeedUp(float newSpeed, float newFadeSpeed)
		{
			Debug.Log("speeding up");
			_printSpeed = newSpeed;
			_fadeSpeed = newFadeSpeed;
		}

		public static void ResetSpeed()
		{
			_spedUpSpeed = null;
		}

		public static void PrintBeat(TextMeshProUGUI label, string beat, float timeBetweenWords, MonoBehaviour mono, Action OnEnd)
		{
			if (string.IsNullOrEmpty(label.text)) return;
			_printSpeed = timeBetweenWords;
			label.ForceMeshUpdate();
			string[] words = beat.Split(' ');
			Debug.Log(words.Length);
			mono.StartCoroutine(AnimateText(label, words, OnEnd));
		}

		private static IEnumerator AnimateText(TextMeshProUGUI dialogueLabel, string[] words, Action OnEnd)
		{
			TMP_TextInfo textInfo = dialogueLabel.textInfo;
			SetTextAlpha(0, textInfo, dialogueLabel);
			int currentWordStartingIndex = 0;
			foreach (string word in words)
			{
				(int wordStartingIndex,int wordLength ) wordData = (currentWordStartingIndex,word.Length);
				float elapsedTime = 0f;
				float fadeSpeed = _spedUpFadeSpeed ?? _fadeSpeed;
				while (elapsedTime < fadeSpeed)
				{
					byte alpha = CalculateAlpha(elapsedTime);
					SetWordAlpha(wordData, alpha, textInfo, dialogueLabel);
					elapsedTime += Time.deltaTime;
					yield return null;
				}
				SetWordAlpha(wordData, 255, textInfo, dialogueLabel);
				currentWordStartingIndex += wordData.wordLength + 1; // +1 for the space
				float waitTime = _spedUpSpeed ?? _printSpeed;
				yield return new WaitForSeconds(waitTime);
			}
			RestoreAfterAnimation(textInfo, dialogueLabel, OnEnd);
		}

		private static void SetWordAlpha((int wordStartingIndex, int wordLength) wordInfo, byte alpha, TMP_TextInfo textInfo, TextMeshProUGUI label)
		{
			for (int wordCharacterIndex = 0; wordCharacterIndex < wordInfo.wordLength; wordCharacterIndex++)
			{
				int characterIndex = wordInfo.wordStartingIndex + wordCharacterIndex;
				TMP_CharacterInfo characterInfo = textInfo.characterInfo[characterIndex];
				SetCharacterAlpha(characterInfo, alpha, textInfo, label);
			}
		}

		private static void SetCharacterAlpha(TMP_CharacterInfo characterInfo, byte alpha, TMP_TextInfo textInfo, TextMeshProUGUI label)
		{
			Color32[] colors = textInfo.meshInfo[characterInfo.materialReferenceIndex].colors32;
			for (int i = 0; i < 4; i++)
			{
				colors[characterInfo.vertexIndex + i].a = alpha;
			}
			label.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
		}

		private static void SetTextAlpha(byte alpha, TMP_TextInfo textInfo, TextMeshProUGUI label)
		{
			foreach (TMP_CharacterInfo characterInfo in textInfo.characterInfo)
			{
				SetCharacterAlpha(characterInfo, alpha, textInfo, label);
			}
			label.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
		}

		private static byte CalculateAlpha(float elapsedTime)
		{
			float fadeSpeed = _spedUpFadeSpeed ?? _fadeSpeed;
			float alpha = Mathf.Clamp01(elapsedTime / fadeSpeed);
			return (byte)(alpha * 255);
		}

		private static void RestoreAfterAnimation(TMP_TextInfo textInfo, TextMeshProUGUI label, Action OnEnd)
		{
			_spedUpSpeed = null;
			OnEnd?.Invoke();
			SetTextAlpha(255, textInfo, label);
		}
	}

}