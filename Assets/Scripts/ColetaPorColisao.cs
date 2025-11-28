using UnityEngine;

public class ColetaPorColisao : MonoBehaviour
{
    public AudioSource audio;
    void Start()
    {
        
    }
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Moeda"))
        {
            Debug.Log("Coleta de Moeada");
            audio.Play();
        }

        if (collision.gameObject.CompareTag("Passaro"))
        {
            Debug.Log("Passaro Coletado");
        }
    }
}
