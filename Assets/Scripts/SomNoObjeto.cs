using UnityEngine;
using System.Collections;

public class SomNoObjeto : MonoBehaviour
{
    [Header("Configuração de Grupos de Áudio")]
    public AudioClip[] ruidoD = new AudioClip[0];
    public AudioClip[] ruidoE = new AudioClip[0];
    public AudioClip[] ruidoT = new AudioClip[0];
    public AudioClip[] passaro = new AudioClip[0];
    public AudioClip[] moeda = new AudioClip[0];

    [Header("Configurações")]
    public bool ItemEssencial = false;

    private AudioSource audioSource;
    private int Tipo = -1;
    private bool somConfigurado = false;


    void Start()
    {
        // Verifica se o item é essencial
        if (ItemEssencial)
        {
            enabled = false;
            return;
        }

        // Determina o tipo baseado no nome do GameObject
        DeterminarTipoPeloNome();

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

    private void DeterminarTipoPeloNome()
    {
        string nomeObjeto = gameObject.name.ToLower();

        if (nomeObjeto.Contains("ruidod"))
        {
            Tipo = 0;
        }
        else if (nomeObjeto.Contains("ruidoe"))
        {
            Tipo = 1;
        }
        else if (nomeObjeto.Contains("ruidot"))
        {
            Tipo = 2;
        }
        else if (nomeObjeto.Contains("passaro"))
        {
            Tipo = 3;
        }
        else if (nomeObjeto.Contains("moeda"))
        {
            Tipo = 4;
        }
        else
        {
            Debug.LogWarning($"Não foi possível determinar o tipo para o objeto: {gameObject.name}");
            enabled = false;
        }
    }

    private void ConfigurarAudioSource()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 1.0f; // Som 3D
        audioSource.rolloffMode = AudioRolloffMode.Logarithmic;
        audioSource.maxDistance = 25f;
        audioSource.minDistance = 1f;
        somConfigurado = true;
    }

    private void TocarSomAleatorio()
    {
        AudioClip clipSelecionado = null;

        switch (Tipo)
        {
            case 0: // ruidoD
                if (ruidoD != null && ruidoD.Length > 0)
                {
                    clipSelecionado = ruidoD[Random.Range(0, ruidoD.Length)];
                }
                break;

            case 1: // ruidoE
                if (ruidoE != null && ruidoE.Length > 0)
                {
                    clipSelecionado = ruidoE[Random.Range(0, ruidoE.Length)];
                }
                break;

            case 2: // ruidoT
                if (ruidoT != null && ruidoT.Length > 0)
                {
                    clipSelecionado = ruidoT[Random.Range(0, ruidoT.Length)];
                }
                break;

            case 3: // passaro
                if (passaro != null && passaro.Length > 0)
                {
                    clipSelecionado = passaro[Random.Range(0, passaro.Length)];
                }
                break;

            case 4: // moeda
                if (moeda != null && moeda.Length > 0)
                {
                    clipSelecionado = moeda[Random.Range(0, moeda.Length)];
                }
                break;
        }

        if (clipSelecionado != null)
        {
            audioSource.clip = clipSelecionado;
            audioSource.Play();
            Debug.Log($"Tocando som '{clipSelecionado.name}' no objeto '{gameObject.name}' (Tipo: {Tipo})");
        }
        else
        {
            Debug.LogWarning($"Nenhum clip de áudio disponível para o tipo {Tipo} no objeto {gameObject.name}");
            enabled = false;
        }
    }

    // Método público para forçar a reprodução do som
    public void TocarSom()
    {
        if (ItemEssencial && somConfigurado)
        {
            TocarSomAleatorio();
        }
    }

    // Método para alterar o tipo manualmente
    public void AlterarTipo(int novoTipo)
    {
        if (novoTipo >= 0 && novoTipo <= 4)
        {
            Tipo = novoTipo;
            if (audioSource != null && audioSource.isPlaying)
            {
                audioSource.Stop();
            }
            TocarSomAleatorio();
        }
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