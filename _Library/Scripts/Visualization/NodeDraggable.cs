using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

public class DraggableNode : MonoBehaviour
{
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Rigidbody rb;
    private XRGrabInteractable grabInteractable;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        grabInteractable = GetComponent<XRGrabInteractable>();

        originalPosition = transform.position;
        originalRotation = transform.rotation;

        rb.isKinematic = true;

        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    void OnDestroy()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited.RemoveListener(OnRelease);
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        if (args.interactorObject is XRBaseControllerInteractor controllerInteractor)
        {
            controllerInteractor.SendHapticImpulse(0.5f, 0.1f);
        }
    }

    void OnRelease(SelectExitEventArgs args)
    {
        StartCoroutine(ReturnToOriginalPosition());
    }

    private IEnumerator ReturnToOriginalPosition()
    {
        float durationToReturn = 1.0f; 
        float elapsedTime = 0.0f;

        Vector3 startingPosition = transform.position;
        Quaternion startingRotation = transform.rotation;

        while (elapsedTime < durationToReturn)
        {
            transform.position = Vector3.Lerp(startingPosition, originalPosition, elapsedTime / durationToReturn);
            transform.rotation = Quaternion.Lerp(startingRotation, originalRotation, elapsedTime / durationToReturn);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPosition;
        transform.rotation = originalRotation;
    }
}

