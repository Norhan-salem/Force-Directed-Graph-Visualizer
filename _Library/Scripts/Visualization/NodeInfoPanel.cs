using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NodeInfoPanel : MonoBehaviour
{
    public TextMeshProUGUI Label;
    public TextMeshProUGUI Weight;
    public TextMeshProUGUI Coverage;
    public TextMeshProUGUI Size;
    public TextMeshProUGUI Type;

    private void Start()
    {
        HidePanel();
    }

    public void SetInfo(string label, string type, float size, float coverage, float weight)
    {
        Label.text = "Label: " + label;
        Type.text = "Type: " + type;
        Size.text = "Size: " + size.ToString();
        Coverage.text = "Coverage: " + coverage.ToString();
        Weight.text = "Weight: " + weight.ToString();
    }

    public void ShowPanel()
    {
        gameObject.SetActive(true);
    }

    public void HidePanel()
    {
        gameObject.SetActive(false);
    }
}
