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
            && hitInfo.collider.gameObject.layer == 6) // 6 = "Selectable" layer index.
        {
            var hitObject = hitInfo.collider.gameObject;

            if (selectedObject is null)
            {
                selectedObject = hitObject;
                Debug.Log($"Selected {selectedObject.name}");
            }
            else if (selectedObject.Equals(hitObject))
            {
                Debug.Log($"DEselected {selectedObject.name}");
                selectedObject = null;
            }
            else
            {
                if (selectedObject.tag == "TestTube" && hitObject.tag == "TestTubeStand")
                {
                    var stand = hitObject.GetComponent<TestTubeStand>();
                    var tube = selectedObject.GetComponent<TestTube>();
                    stand.AddTube(tube);

                    Debug.Log($"DEselected {selectedObject.name}");
                    selectedObject = null;
                }
                else if (selectedObject.tag == "SolidReactive" && hitObject.tag == "TestTube")
                {
                    var reactiveGroup = selectedObject.GetComponent<SolidReactive>();
                    var tube = hitObject.GetComponent<TestTube>();
                    var reactiveData = reactiveGroup.Generate();

                    tube.AddSolidReactive(reactiveData.Item1, reactiveData.Item2);

                    Debug.Log($"DEselected {selectedObject.name}");
                    selectedObject = null;
                }
            }
        }
    }
}
