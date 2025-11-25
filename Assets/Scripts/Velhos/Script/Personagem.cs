using UnityEngine;
using UnityEngine.UI;

public class Personagem : MonoBehaviour
{
    [Header("Variaveis")]
    public int vidaAtual = 0;
    public int vidaMaxima = 100;
    [Space]
    public int moedas = 100;
    [Space]
    [Header("Intancia")]
    public static Personagem instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        vidaAtual = vidaMaxima;
        HUDManeger.instance.fill.color = Color.green;
    }

    private void Update()
    {

        //HUDManeger.instance.vidaSlider.maxValue = vidaMaxima;

        if (Input.GetKeyDown(KeyCode.Z))
        {
            TomarDano(10);
            Debug.Log("Vida Atual: " + vidaAtual);
        }
    }

    private void TomarDano(int Dano)
    {
        vidaAtual -= Dano;
        HUDManeger.instance.vidaSlider.value = vidaAtual;

        HUDManeger.instance.fill.color = Color.Lerp(Color.red, Color.green, (float)vidaAtual/(float)vidaMaxima);

        if (vidaAtual <= 0)
        {
            vidaAtual = 0;
            //Morte
        }
    }
}
