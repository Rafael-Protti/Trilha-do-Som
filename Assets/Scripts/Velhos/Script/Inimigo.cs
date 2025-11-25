using System;
using UnityEngine;

public class Inimigo : MonoBehaviour
{
    public int vida = 3;
    public int AreaMovimento = 1000;

    Rigidbody rb;
    bool movendoSe;
    bool morto = false;

    Transform AncoraIndicador;
    public Transform particulaMorte;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AncoraIndicador = GameObject.Find("Indicador").transform;
    }

    // Update is called once per frame
    void Update()
    {
        Decidindo();
    }

    public void LevarDano(int dano = 1)
    {
        vida -= dano;
        Debug.Log("Inimigo levou " + dano + " de dano. Vida restante: " + vida);

        if (vida <= 0)
        {
            rb = transform.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.constraints = RigidbodyConstraints.None;
            }

            Morrer();
        }
        else
        {
            rb = transform.GetComponent<Rigidbody>();
            if (rb != null)
            {
               rb.mass = 100;
            }
        }
    }

    void Morrer()
    {
        if (morto) return;
        morto = true;
        Debug.Log("Inimigo morreu!");

        Transform instaciado = Instantiate(particulaMorte);
        instaciado.position = AncoraIndicador.position;
        instaciado.rotation = AncoraIndicador.rotation;
        instaciado.localScale = AncoraIndicador.localScale;

        rb = transform.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.mass = 1;
        }

        transform.GetComponent<AudioSource>().Play();

        Destroy(gameObject, 2.5f);
    }

    void Decidindo()
    {
        movendoSe = true;
        Movimentacao();
    }

    void Movimentacao()
    {

        int Alea = UnityEngine.Random.Range(0, AreaMovimento);
        // Simulação de movimento
        transform.Rotate(Vector3.right * Alea * Time.deltaTime);

        if (movendoSe)
        {
            Debug.Log("Inimigo se movendo...");
            // Implementar movimentos do inimigo aqui
            rb = transform.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.mass = 1;
            }

            // Gera um número inteiro entre 0 e 200 (inclusive)
            transform.Translate(Vector3.forward * Alea * Time.deltaTime);
        }
    }

}
