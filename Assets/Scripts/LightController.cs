using UnityEngine;

public class LightController : MonoBehaviour
{
    [SerializeField]
    float rotationSpeed;

    // Update is called once per frame
    void Update()
    {
        this.transform.rotation = Quaternion.AngleAxis(Time.time * rotationSpeed, Vector3.up);
    }
}