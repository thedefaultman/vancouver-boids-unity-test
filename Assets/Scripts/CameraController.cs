using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField]
    float horizontalSensitivity = 1f;

    [SerializeField]
    float velocityGravity = 0.5f;

    bool dragging;

    float lastMouseX;

    float yVelocity;
    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0)){
            Cursor.lockState = CursorLockMode.Confined;


            if(!dragging){

                lastMouseX = Input.mousePosition.x;

                dragging = true;
            }


        }
        else{
            dragging = false;
        }


        if(dragging){

            yVelocity = Input.mousePosition.x - lastMouseX;
            lastMouseX = Input.mousePosition.x;
        }
        else{
            yVelocity = Mathf.MoveTowards(yVelocity, 0, velocityGravity * Time.deltaTime);
        }


        this.transform.Rotate(Vector3.up, yVelocity * horizontalSensitivity * Time.deltaTime, Space.World);


        if(Input.GetKeyDown(KeyCode.Escape)){
            Cursor.lockState = CursorLockMode.None;
        }
    }
}