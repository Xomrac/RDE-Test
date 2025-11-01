using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace XomracCore.DialogueSystem
{

	public static class BeatPrinter
	{
		private static float _printSpeed;
		private static float _fadeSpeed = 0.05f;

		public static void SpeedUp(float newSpeed, float newFadeSpeed)
		{
			Debug.Log("speeding up");
			_printSpeed = newSpeed;
			_fadeSpeed = newFadeSpeed;
		}

		public static void PrintBeat(TextMeshProUGUI label, string beat, float timeBetweenWords, MonoBehaviour mono, Action onEnd)
		{
			if (string.IsNullOrEmpty(beat)) return;

			_printSpeed = timeBetweenWords;

			label.text = beat;
			label.ForceMeshUpdate();

			mono.StartCoroutine(AnimateText(label, onEnd));
		}

		private static IEnumerator AnimateText(TextMeshProUGUI label, Action onEnd)
		{
			SetLabelTextAlpha(label, 0);

			List<(int wordStartIndex, int wordLength)> words = FetchWordsToPrint(label.textInfo);

			foreach ((int wordStartIndex, int wordLength) word in words)
			{
				float elapsed = 0f;
				while (elapsed < _fadeSpeed)
				{
					byte alpha = CalculateAlpha(elapsed);
					
					(int firstCharIndex, int charCount) = word;
					ApplyAlphaToWord(label, firstCharIndex, charCount, alpha);
					
					label.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
					elapsed += Time.deltaTime;
					yield return null;
				}
				(int wordStartIndex, int wordLength) completeBeat = word;
				ApplyAlphaToWord(label, completeBeat.wordStartIndex, completeBeat.wordLength, 255);
				label.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

				yield return new WaitForSeconds(_printSpeed);
			}

			RestoreAfterAnimation(label, onEnd);
		}

		private static List<(int wordStartIndex, int wordLength)> FetchWordsToPrint(TMP_TextInfo textInfo)
		{
			var runs = new List<(int wordStartIndex, int wordLength)>();
			int i = 0;
			int charCount = textInfo.characterCount;

			while (i < charCount)
			{
				// Skip non printable characters (spaces, newlines, etc.)
				while (i < charCount && !textInfo.characterInfo[i].isVisible)
				{
					i++;
				}
				if (i >= charCount) break;

				int start = i;
				i++;

				// continue while characters are visible (includes adjacent punctuation)
				while (i < charCount && textInfo.characterInfo[i].isVisible)
				{
					i++;
				}
				runs.Add((start, i - start));
			}
			return runs;
		}

		private static void ApplyAlphaToWord(TextMeshProUGUI label, int firstCharIndex, int charCount, byte alpha)
		{
			TMP_TextInfo textInfo = label.textInfo;
			for (int i = 0; i < charCount; i++)
			{
				int charIndex = firstCharIndex + i;
				if (charIndex < 0 || charIndex >= textInfo.characterCount) continue;

				TMP_CharacterInfo character = textInfo.characterInfo[charIndex];
				if (!character.isVisible) continue;

				Color32[] colors = textInfo.meshInfo[character.materialReferenceIndex].colors32;
				int vertexIndex = character.vertexIndex;

				colors[vertexIndex + 0].a = alpha;
				colors[vertexIndex + 1].a = alpha;
				colors[vertexIndex + 2].a = alpha;
				colors[vertexIndex + 3].a = alpha;
			}
		}

		private static void SetLabelTextAlpha(TextMeshProUGUI label, byte alpha)
		{
			label.ForceMeshUpdate();

			TMP_TextInfo textInfo = label.textInfo;
			int count = textInfo.characterCount;

			for (int i = 0; i < count; i++)
			{
				TMP_CharacterInfo character = textInfo.characterInfo[i];
				if (!character.isVisible) continue;

				Color32[] colors = textInfo.meshInfo[character.materialReferenceIndex].colors32;
				int vi = character.vertexIndex;

				colors[vi + 0].a = alpha;
				colors[vi + 1].a = alpha;
				colors[vi + 2].a = alpha;
				colors[vi + 3].a = alpha;
			}

			label.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
		}

		private static byte CalculateAlpha(float elapsedTime)
		{
			float t = Mathf.Clamp01(elapsedTime / _fadeSpeed);
			return (byte)(t * 255);
		}

		private static void RestoreAfterAnimation(TextMeshProUGUI label, Action onEnd)
		{
			onEnd?.Invoke();
			SetLabelTextAlpha(label, 255);
			label.ForceMeshUpdate();
		}
	}

}