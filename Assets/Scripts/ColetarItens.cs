using UnityEngine;

public class ColetarItens : MonoBehaviour
{
    Camera camera;
    Ray ray;
    RaycastHit hit;

    void Start()
    {
        camera = Camera.main;
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            ray = new Ray(camera.ScreenToWorldPoint(Input.mousePosition), camera.transform.forward);

            if (Physics.Raycast(ray, out hit, 5f))
            {
                if (hit.transform == transform)
                {
                    Destroy(hit.transform.gameObject);
                }
            }
        }
    }
}
