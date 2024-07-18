using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Node : MonoBehaviour
{
       
/*     string label;
    string type; 
    float size;
    float coverage;
    float weight;
 */    public string id;
    public NodeInfoPanel infoPanel;
    public List<Node> ConnectedNodes { get; private set; }

    public void Awake()
    {
        ConnectedNodes = new List<Node>();
    }

    //public GameObject infoPanel;


    /* private bool isHovered = false;

    private void Update()
    {
        if (isHovered)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            infoPanel.transform.position = mousePosition;
        }
    }

    public void OnPointerEnter()
    {
        isHovered = true;
        infoPanel.SetActive(true);
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        infoPanel.transform.position = mousePosition;

        
        var infoPanelComponent = infoPanel.GetComponent<NodeInfoPanel>();
        infoPanelComponent.SetInfo(label, type, size, coverage, weight);
    }

    public void OnPointerExit()
    {
        isHovered = false;
        infoPanel.SetActive(false);
    }

    private void OnEnable()
    {
        var input = GetComponent<PlayerInput>();
        input.actions["Point"].performed += ctx => OnPointerEnter();
        input.actions["Point"].canceled += ctx => OnPointerExit();
    }

    private void OnDisable()
    {
        var input = GetComponent<PlayerInput>();
        input.actions["Point"].performed -= ctx => OnPointerEnter();
        input.actions["Point"].canceled -= ctx => OnPointerExit();
    } */
}





