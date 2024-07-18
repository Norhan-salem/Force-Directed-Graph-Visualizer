using UnityEngine;

public class Link : MonoBehaviour
{
    public Node StartNode;
    public Node EndNode;

    public void UpdateLink()
    {
        if (StartNode == null || EndNode == null)
        {
            return;
        }

        Vector3 start = StartNode.transform.position;
        Vector3 end = EndNode.transform.position;
        Vector3 direction = (end - start);

        transform.position = start + direction / 2;
        transform.rotation = Quaternion.LookRotation(direction)* Quaternion.Euler(90, 0, 0);;
        
        
        transform.localScale = new Vector3(1f, direction.magnitude / 2f, 1f);
    }
}

