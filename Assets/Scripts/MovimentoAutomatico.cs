using UnityEngine;

public class Movimentoautomático : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    [SerializeField] private float velocidade = 5f;
    [SerializeField] private float distanciaMaxima = 10f;

    [Header("Desligado")]
    public bool ItemEssencial = false;

    private Vector3 posicaoInicial;
    private float distanciaPercorrida = 0f;
    private bool movimentoAtivo = true;

    void Start()
    {
        // Verifica se o item é essencial
        if (ItemEssencial)
        {
            enabled = false;
            return;
        }

        // Guarda a posição inicial para calcular a distância percorrida
        posicaoInicial = transform.position;
    }

    void Update()
    {
        if (!movimentoAtivo) return;

        // Move o objeto na direção Z negativo
        Vector3 movimento = Vector3.back * velocidade * Time.deltaTime;
        transform.Translate(movimento, Space.World);

        // Calcula a distância percorrida
        distanciaPercorrida = Vector3.Distance(posicaoInicial, transform.position);

        // Verifica se atingiu a distância máxima
        if (distanciaPercorrida >= distanciaMaxima)
        {
            DestruirObjeto();
        }
    }

    void DestruirObjeto()
    {
        movimentoAtivo = false;
        Destroy(gameObject);
    }

    // Gizmo para visualizar a trajetória e distância
    void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying)
        {
            posicaoInicial = transform.position;
        }

        // Calcula a posição final baseada na distância máxima
        Vector3 posicaoFinal = posicaoInicial + Vector3.back * distanciaMaxima;

        // Desenha a linha da trajetória
        Gizmos.color = Color.red;
        Gizmos.DrawLine(posicaoInicial, posicaoFinal);

        // Desenha esferas nas posições inicial e final
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(posicaoInicial, 0.5f);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(posicaoFinal, 0.5f);

        // Desenha setas ao longo da trajetória para indicar direção
        DesenharSetasNaTrajetoria(posicaoInicial, posicaoFinal);
    }

    void DesenharSetasNaTrajetoria(Vector3 inicio, Vector3 fim)
    {
        Gizmos.color = Color.yellow;
        Vector3 direcao = (fim - inicio).normalized;
        float comprimentoTotal = Vector3.Distance(inicio, fim);

        // Número de setas baseado no comprimento
        int numeroSetas = Mathf.Max(2, Mathf.FloorToInt(comprimentoTotal / 2f));
        float espacamento = comprimentoTotal / numeroSetas;

        for (int i = 1; i < numeroSetas; i++)
        {
            Vector3 posicaoSeta = inicio + direcao * (espacamento * i);
            DesenharSeta(posicaoSeta, direcao, 0.5f);
        }
    }

    void DesenharSeta(Vector3 posicao, Vector3 direcao, float tamanho)
    {
        Vector3 direita = Quaternion.LookRotation(direcao) * Quaternion.Euler(0, 45, 0) * Vector3.forward;
        Vector3 esquerda = Quaternion.LookRotation(direcao) * Quaternion.Euler(0, -45, 0) * Vector3.forward;

        Gizmos.DrawRay(posicao, direita * tamanho);
        Gizmos.DrawRay(posicao, esquerda * tamanho);
    }

    // Métodos públicos para acesso externo se necessário
    public void SetVelocidade(float novaVelocidade)
    {
        velocidade = novaVelocidade;
    }

    public void SetDistanciaMaxima(float novaDistancia)
    {
        distanciaMaxima = novaDistancia;
    }

    public float GetDistanciaPercorrida()
    {
        return distanciaPercorrida;
    }

    public float GetDistanciaRestante()
    {
        return Mathf.Max(0, distanciaMaxima - distanciaPercorrida);
    }
}