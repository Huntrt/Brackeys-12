using UnityEngine;
using TMPro;

public class GlobalFontChanger : MonoBehaviour
{
	[SerializeField] TextMeshProUGUI[] textGUIs;
	[SerializeField] TMP_FontAsset font;
	[SerializeField] bool setFont;

    void OnValidate()
	{
		if(!setFont) return;
		textGUIs = Object.FindObjectsOfType<TextMeshProUGUI>(true);
		foreach (var text in textGUIs) {text.font = font;}
	}
}