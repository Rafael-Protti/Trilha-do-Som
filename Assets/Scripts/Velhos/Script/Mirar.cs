using UnityEngine;

public class Mirar : MonoBehaviour
{

    Animator animador;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animador = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            HUDManeger.instance.valorMoedas.text = "1";
        }

        if (Input.GetButtonDown("Fire2"))
        {
            animador.SetBool("Mirando", true);
        }
        if (Input.GetButtonUp("Fire2"))
        {
            animador.SetBool("Mirando", false);
        }
    }
}
