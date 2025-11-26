using UnityEngine;
using System.Collections;


public class SomNoObstaculo : MonoBehaviour
{
    [Header("Configuração de Áudio")]
    public AudioClip[] ruidoObstaculo = new AudioClip[0];

    private AudioSource audioSource;
    private bool somConfigurado = false;

    void Start()
    {

        // Configura o AudioSource
        ConfigurarAudioSource();

        // Seleciona e toca o som aleatório
        if (somConfigurado)
        {
            TocarSomAleatorio();
        }
    }

    void Update()
    {
        // Se o objeto for destruído, para o som
        if (audioSource != null && !audioSource.isPlaying && audioSource.clip != null)
        {
            // Opcional: Destruir o objeto após tocar o som
            // Destroy(gameObject, 0.1f);
        }
    }

    private void ConfigurarAudioSource()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 1.0f; // Som 3D
        audioSource.rolloffMode = AudioRolloffMode.Logarithmic;
        audioSource.maxDistance = 150f;
        audioSource.minDistance = 0.01f;
        somConfigurado = true;
    }

    private void TocarSomAleatorio()
    {
        AudioClip clipSelecionado = null;

        if (ruidoObstaculo != null && ruidoObstaculo.Length > 0)
            {
                clipSelecionado = ruidoObstaculo[Random.Range(0, ruidoObstaculo.Length)];
            }

        if (clipSelecionado != null)
        {
            audioSource.clip = clipSelecionado;
            audioSource.Play();
            Debug.Log($"Tocando som '{clipSelecionado.name}' no objeto '{gameObject.name}' (Tipo: Obstáculo)");
        }
        else
        {
            Debug.LogWarning($"Nenhum clip de áudio disponível para o tipo Obstáculos no objeto {gameObject.name}");
            enabled = false;
        }
    }

    // Método público para forçar a reprodução do som
    public void TocarSom()
    {
        if (somConfigurado)
        {
            TocarSomAleatorio();
        }
    }

    // Método para alterar o tipo manualmente
    public void AlterarTipo(int novoTipo)
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        TocarSomAleatorio();
    }

    // Método para verificar se o som está tocando
    public bool EstaTocandoSom()
    {
        return audioSource != null && audioSource.isPlaying;
    }

    void OnDestroy()
    {
        // Para o som quando o objeto for destruído
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}