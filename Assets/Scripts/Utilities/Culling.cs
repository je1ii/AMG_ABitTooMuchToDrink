using UnityEngine;

public class Culling : MonoBehaviour
{
    [SerializeField] private Transform visualTarget;
    [SerializeField] private bool isInteractable;
    
    private Transform meshTransform;
    private Vector3 originalScale;
    private Transform cameraTransform;

    private const float CullDistanceBuffer = -10f; 
    private const float CullFrontDistanceBuffer = 10f; 

    void Start()
    {
        if (Camera.main == null)
        {
            Debug.LogError("Culling script requires a main camera to function.");
            enabled = false;
            return;
        }

        cameraTransform = Camera.main.transform;

        if (visualTarget == null)
        {
            visualTarget = transform;
        }

        meshTransform = transform;
        originalScale = transform.localScale;
    }

    void Update()
    {
        float cameraX = cameraTransform.position.x;
        float visualTargetX = visualTarget.position.x;
        bool isBehindCamera = (visualTargetX - cameraX) < CullDistanceBuffer;
        bool isOverCamera = (visualTargetX - cameraX)  > CullFrontDistanceBuffer;


        if (isBehindCamera || isOverCamera)
        {
            if (meshTransform.localScale != Vector3.zero)
            {
                meshTransform.localScale = Vector3.zero;
                
                if(isInteractable && isBehindCamera)
                    Destroy(this.gameObject);
            }
        }
        else
        {
            if (meshTransform.localScale == Vector3.zero)
            {
                meshTransform.localScale = originalScale;
            }
        }
    }
}
