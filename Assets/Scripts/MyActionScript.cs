using UnityEngine;
using UnityEngine.InputSystem;

public class MyActionScript : MonoBehaviour
{
    private InputAction myAction;
    private Vector3 initPosition;
    [Space][SerializeField] private InputActionAsset myActionAsset;

    void Start()
    {
        myAction = myActionAsset.FindAction("MyOwn/MyAction");
        initPosition = transform.position;
    }

    private void Update()
    {
        if(myAction.WasPerformedThisFrame())
        {
            transform.position = initPosition;
        }
    }
}
