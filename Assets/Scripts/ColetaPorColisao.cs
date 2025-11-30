using UnityEngine;

public class ColetaPorColisao : MonoBehaviour
{
    public AudioClip audio;
    AudioSource audioSource;
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
            audioSource.PlayOneShot(audio);
        }

        if (collision.gameObject.CompareTag("Passaro"))
        {
            Debug.Log("Passaro Coletado");
        }
    }
}
