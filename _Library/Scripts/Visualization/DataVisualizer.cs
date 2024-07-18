using System.Collections.Generic;
using UnityEngine;

public class DataVisualizer : MonoBehaviour
{
    public bool _LoadGraphData = false;
    public TextAsset graphDataJson; 
    public GameObject infoPanelPrefab; 

    public float MoveForce = 10f;
    public int IterationsToStop = 100;

    public GameObject nodePrefab;
    public GameObject linkPrefab;

    public Material blueMaterial;
    public Material redMaterial;

    private List<Node> AllNodes = new List<Node>();
    private List<Link> AllLinks = new List<Link>();
    private int IterationsRemaining;
    private Dictionary<string, Node> nodeDictionary = new Dictionary<string, Node>();

    private void LoadGraphData()
    {
        IterationsRemaining = IterationsToStop;

        foreach (var node in AllNodes)
        {
            Destroy(node.gameObject);
        }
        AllNodes.Clear();

        foreach (var link in AllLinks)
        {
            Destroy(link.gameObject);
        }
        AllLinks.Clear();

        if (nodePrefab == null || linkPrefab == null)
        {
            Debug.LogError("Node prefab or Link prefab is not assigned!");
            return;
        }

        GraphData graphData = JsonUtility.FromJson<GraphData>(graphDataJson.text);

        // Instantiate nodes
        foreach (var nodeData in graphData.nodes)
        {
            var go = Instantiate(nodePrefab, Random.insideUnitSphere * 1f, Quaternion.identity);
            go.name = nodeData.label;
            var nodeComponent = go.GetComponent<Node>();
            if (nodeComponent == null)
            {
                Debug.LogError($"Node component is missing on the prefab instance: {nodeData.label}");
                continue;
            }
            nodeComponent.id = nodeData.id; 
            nodeDictionary[nodeData.id] = nodeComponent;
            AllNodes.Add(nodeComponent);

            
            Renderer renderer = go.GetComponent<Renderer>();
            if (renderer != null)
            {
                Material material = null;
                if (nodeData.type == "x")
                {
                    material = blueMaterial;
                }
                else if (nodeData.type == "c")
                {
                    material = redMaterial;
                }
                else
                {
                    Debug.LogWarning($"Unknown node type: {nodeData.type}");
                }

                if (material != null)
                {
                    renderer.material = material;
                }
                else
                {
                    Debug.LogWarning($"Material not assigned for node type: {nodeData.type}");
                }
            }
            else
            {
                Debug.LogWarning($"Renderer component not found on node: {nodeData.label}");
            }
            
            // Instantiate and configure the info panel
            var infoPanel = Instantiate(infoPanelPrefab, transform);
            infoPanel.SetActive(false); 
            var infoPanelComponent = infoPanel.GetComponent<NodeInfoPanel>();
            if (infoPanelComponent != null)
            {
                infoPanelComponent.SetInfo(nodeData.label, nodeData.type, nodeData.size, nodeData.coverage, nodeData.weight);
                nodeComponent.infoPanel = infoPanelComponent;
            }
            else
            {
                Debug.LogError("InfoPanelComponent is missing on the prefab");
            }
        }

        // Instantiate and configure the info panel
/*             var infoPanel = Instantiate(infoPanelPrefab, transform);
            infoPanel.SetActive(false); 
            var infoPanelComponent = infoPanel.GetComponent<NodeInfoPanel>();
            infoPanelComponent.SetInfo(nodeData.label, nodeData.type, nodeData.size, nodeData.coverage, nodeData.weight);
            nodeComponent.infoPanel = infoPanel; */
        


        // Instantiate links
        foreach (var linkData in graphData.links)
        {
            if (nodeDictionary.TryGetValue(linkData.source, out var startNode) &&
                nodeDictionary.TryGetValue(linkData.target, out var endNode))
            {
                var linkGo = Instantiate(linkPrefab);
                var link = linkGo.AddComponent<Link>();
                link.StartNode = startNode;
                link.EndNode = endNode;
                AllLinks.Add(link);

                startNode.ConnectedNodes.Add(endNode);
                endNode.ConnectedNodes.Add(startNode);
            }
            else
            {
                Debug.LogError($"Link source or target node not found: {linkData.source} -> {linkData.target}");
            }
        }
    }

    private void UpdateLinks()
    {
        foreach (var link in AllLinks)
        {
            link.UpdateLink();
        }
    }

    public void Update()
    {
        if (_LoadGraphData)
        {
            _LoadGraphData = false;
            LoadGraphData();
        }

        ApplyForces();
        UpdateLinks(); 
    }

    private void ApplyForces()
    {
        if (IterationsRemaining-- < 0) return;
        foreach (var n1 in AllNodes)
        {
            ApplyPull(n1, Vector3.zero, n1.transform.position.magnitude);
            foreach (var n2 in AllNodes)
            {
                if (n1 == n2) continue;

                var connected = n1.ConnectedNodes.Contains(n2);
                var distance = (n1.transform.position - n2.transform.position).magnitude;
                if (connected)
                {
                    ApplyPull(n1, n2.transform.position, distance);
                }
                else
                {
                    ApplyPush(n1, n2.transform.position, distance);
                }
            }
        }
    }

    private void ApplyPush(Node n1, Vector3 toPosition, float distance)
    {
        var diff = n1.transform.position - toPosition;
        var dir = diff.normalized;
        var force = dir * MoveForce * Time.deltaTime * (1f - (Mathf.Clamp(distance, 0, 1f)));
        n1.transform.position += force;
    }

    private void ApplyPull(Node n1, Vector3 toPosition, float distance)
    {
        var diff = toPosition - n1.transform.position;
        var dir = diff.normalized;
        var force = dir * MoveForce * Time.deltaTime * Mathf.Clamp(distance, 0, 1f);
        n1.transform.position -= force;
    }

}




