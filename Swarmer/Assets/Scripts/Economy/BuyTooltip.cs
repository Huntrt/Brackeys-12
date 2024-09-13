using UnityEngine.EventSystems;
using UnityEngine;
using TMPro;

public class BuyTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField] BuyButton buyButton;
    [SerializeField] TextMeshProUGUI tooltipTxt;
	[SerializeField] GameObject tooltipPanel;
	Vector2 tooltipOffset = new Vector2 (0, -75);

	public void OnPointerEnter(PointerEventData eventData)
	{
		tooltipTxt.text = buyButton.structureData.DisplayName + "\n<color=#fff705>$" + buyButton.structureBuyable.Cost + "</color>";
		tooltipPanel.transform.position = (Vector2)transform.position + tooltipOffset;
		tooltipPanel.SetActive(true);
	}

	void OnDisable()
	{
		tooltipPanel.SetActive(false);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		tooltipPanel.SetActive(false);
	}
}
