using UnityEngine;

[ExecuteAlways]
public class BirdSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject birdPrefab;

    [SerializeField]
    float circleRadius;

    [SerializeField]
    float raycastHeight;

    [SerializeField]
    int birdCount;

    [SerializeField]
    float spawnHeight;

    void OnEnable()
    {
        int oldChildCount = this.transform.childCount;
        for (int i = 0; i < oldChildCount; i++){
            Object.DestroyImmediate(this.transform.GetChild(0).gameObject);
        }


        int checkMask = LayerMask.GetMask("Ground");
        for (int i = 0; i < birdCount; i++){

            var randomPoint = Random.insideUnitCircle;
            randomPoint *= circleRadius;
            var raycastPosition = new Vector3(randomPoint.x, raycastHeight, randomPoint.y);
            if(Physics.Raycast(raycastPosition, Vector3.down, out var hit, float.MaxValue, checkMask)){

                var lookDirection = new Vector3(raycastPosition.x, 0, raycastPosition.z);
                lookDirection *= -1;
                lookDirection = Quaternion.AngleAxis(90, Vector3.up) * lookDirection;
                var flattenedHitNormal = hit.normal;
                lookDirection = Vector3.Cross(lookDirection, flattenedHitNormal);
                lookDirection = Quaternion.AngleAxis(Random.Range(-180, 180), hit.normal) * lookDirection;

                var newBird = Object.Instantiate(birdPrefab, hit.point + Vector3.up * spawnHeight, Quaternion.LookRotation(lookDirection, hit.normal), this.transform);


                Debug.DrawRay(newBird.transform.position, newBird.transform.forward * 0.1f, Color.white, 2f);
            }

        }



        this.enabled = false;
    }
}