using UnityEngine;

public class hideCamDistance : MonoBehaviour
{
    public float HideDistance;
    public GameObject Camera;

    void Update() {
        float distance = Vector3.Distance(transform.position, Camera.transform.position);

        if (distance < HideDistance) {
            GetComponent<MeshRenderer>().enabled = false;
        }
        else
        {
            GetComponent<MeshRenderer>().enabled = true;
        }
    }
}
