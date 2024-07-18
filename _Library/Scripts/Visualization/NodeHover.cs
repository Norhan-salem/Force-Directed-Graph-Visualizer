using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class NodeHover : MonoBehaviour
{
    public NodeInfoPanel infoPanel;
    public string nodeLabel;
    public string nodeType;
    public float nodeSize;
    public float nodeCoverage;
    public float nodeWeight;

    private XRRayInteractor rayInteractor;

    private void Start()
    {
        rayInteractor = FindObjectOfType<XRRayInteractor>();
        if (rayInteractor == null)
        {
            Debug.LogError("XRRayInteractor not found");
        }
    }

private void Update()
{
    if (rayInteractor != null && infoPanel != null)
    {
        RaycastHit hit;
        if (rayInteractor.TryGetCurrent3DRaycastHit(out hit))
        {
            if (hit.transform == transform)
            {
                infoPanel.SetInfo(nodeLabel, nodeType, nodeSize, nodeCoverage, nodeWeight);
                infoPanel.ShowPanel();
            }
            else
            {
                infoPanel.HidePanel();
            }
        }
        else
        {
            infoPanel.HidePanel();
        }
    }
}
}
