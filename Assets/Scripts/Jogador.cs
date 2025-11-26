using UnityEngine;
using System.Collections;

public class Jogador : MonoBehaviour
{
    [Header("Configurações de Vida")]
    public int vida = 3;

    [Header("Clips de Áudio")]
    public AudioClip somTrombando;
    public AudioClip somMorrendo;

    [Header("Componentes")]
    private AudioSource audioSource;

    void Start()
    {
        // Obtém ou adiciona o componente AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        Debug.Log($"Jogador iniciado com {vida} vidas");
    }

    // Módulo Trombando - executado quando há colisão mas ainda tem vida
    public void Trombando()
    {
        Debug.Log("Trombando! Vida restante: " + vida);

        // Toca o som de trombando se configurado
        if (somTrombando != null)
        {
            audioSource.PlayOneShot(somTrombando);
        }
        else
        {
            Debug.LogWarning("Som de trombando não configurado!");
        }

        // Aqui você pode adicionar outros efeitos visuais ou de gameplay
        // como piscar o objeto, aplicar força, etc.
    }

    // Módulo Morrendo - executado quando a vida chega a zero
    public void Morrendo()
    {
        Debug.Log("Morrendo! Fim de jogo.");

        // Toca o som de morrendo se configurado
        if (somMorrendo != null)
        {
            audioSource.PlayOneShot(somMorrendo);
        }
        else
        {
            Debug.LogWarning("Som de morrendo não configurado!");
        }

        // Inicia a corrotina para fechar o jogo após 5 segundos
        StartCoroutine(FecharJogo());
    }

    private IEnumerator FecharJogo()
    {
        Debug.Log("Fechando jogo em 5 segundos...");

        yield return new WaitForSeconds(10f);

        // Fecha o jogo
        FecharAplicacao();
    }

    private void FecharAplicacao()
    {
#if UNITY_EDITOR
            // No Editor - para a execução
            UnityEditor.EditorApplication.isPlaying = false;
#else
        // Em build - fecha a aplicação
        Application.Quit();
#endif
    }

    // Método público para adicionar vida (opcional)
    public void AdicionarVida(int quantidade)
    {
        vida += quantidade;
        Debug.Log($"Vida adicionada. Total: {vida}");
    }

    // Método público para verificar se está vivo (opcional)
    public bool EstaVivo()
    {
        return vida > 0;
    }

    void Update()
    {
        // Debug rápido para testar (remover na versão final)
        if (Input.GetKeyDown(KeyCode.O))
        {
            Trombando();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            Morrendo();
        }
    }
}