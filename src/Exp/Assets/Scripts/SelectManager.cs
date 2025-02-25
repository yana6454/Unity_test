using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SelectManager : MonoBehaviour
{
    [SerializeField]
    private InputManager inputManager;

    [SerializeField]
    private Camera gameCamera;

    private GameObject selectedObject;

    private void Awake()
    {
        inputManager.PrimaryButtonClicked += OnPrimaryButtonClicked;
    }

    private void OnDestroy()
    {
        inputManager.PrimaryButtonClicked -= OnPrimaryButtonClicked;
    }

    private void OnPrimaryButtonClicked()
    {
        Ray ray = gameCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo)
            && hitInfo.collider.gameObject.tag == "Selectable")
        {
            if (selectedObject is null)
            {
                selectedObject = hitInfo.collider.gameObject;
                Debug.Log($"{selectedObject.name} was selected");
            }
            else if (selectedObject.Equals(hitInfo.collider.gameObject))
            {
                Debug.Log($"{selectedObject.name} was DEselected");
                selectedObject = null;
            }
            else
            {
                // [TODO]: Add logic for combination of two selected objects.
            }
        }
    }
}
