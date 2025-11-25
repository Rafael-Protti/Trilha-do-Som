using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManeger : MonoBehaviour
{
    [Space]
    [Header("Elementos do HUD")]
    public TextMeshProUGUI valorMoedas;
    public Slider vidaSlider;
    [Space]
    [Header("Estéticas dos HUDs")]
    public Image fill;

    public static HUDManeger instance;
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

    private void Start()
    {
        AtualizarVidas();
    }

    private void Update()
    {

    }

    private void AtualizarVidas()
    {
        vidaSlider.minValue = 0;
        vidaSlider.maxValue = Personagem.instance.vidaMaxima;
        vidaSlider.value = Personagem.instance.vidaAtual;
    }
}
