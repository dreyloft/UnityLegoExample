using UnityEngine;

public class spawnedBrick : MonoBehaviour
{
    private float opacity;
    private float r, g, b;
    private float destroyTime = 15.0f;
    private float diff;
    private Material myMaterial;

    private void Start()
    {
        myMaterial = GetComponent<MeshRenderer>().material;
        myMaterial.SetInt("_ZWrite", 1);

        opacity = Time.time;
        diff = Random.Range(0.0f, 7.5f);
        transform.eulerAngles = new Vector3(
            -90.0f + Random.Range(-45.0f, 45.0f), 
            Random.Range(-45.0f, 45.0f), 
            Random.Range(-45.0f, 45.0f));
    }

    void FixedUpdate()
    {
        if (Time.time - opacity >= destroyTime + diff)
        {
            GetComponent<MeshRenderer>().material.color = new Color(
                GetComponent<MeshRenderer>().material.color.r, 
                GetComponent<MeshRenderer>().material.color.g,
                GetComponent<MeshRenderer>().material.color.b,
                (diff + destroyTime + 1.0f) - (Time.time - opacity));

            if (1.0f - (Time.time - opacity) <= (-destroyTime + 0.01f - diff))
            {
                Destroy(gameObject);
            }
        }
    }
}
