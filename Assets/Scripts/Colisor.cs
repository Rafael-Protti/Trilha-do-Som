using Unity.VisualScripting;
using UnityEngine;

public class Colisor : MonoBehaviour
{
    Collider colisor;

    private void Start()
    {
        colisor = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {

        Debug.Log($"Colidiu com {other}");

        // Verifica se colidiu com um objeto chamado "Pleno" e com tag "Player"
        if (other.gameObject.name == "Pleno" /*&& other.CompareTag("Player")*/)
        {
            // Tenta pegar o script Jogador no objeto colidido
            Jogador jogador = other.GetComponent<Jogador>();

            if (jogador != null)
            {
                // Verifica se a vida já está em zero
                if (jogador.vida <= 0)
                {
                    jogador.Morrendo();
                }
                else
                {
                    // Remove 1 de vida
                    jogador.vida--;

                    // Executa o módulo Trombando
                    jogador.Trombando();

                    Debug.Log($"Colisão com Pleno! Vida restante: {jogador.vida}");
                }
            }
            else
            {
                Debug.LogWarning("Script Jogador não encontrado no objeto Pleno!");
            }
        }
    }
}