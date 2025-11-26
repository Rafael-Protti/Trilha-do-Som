using UnityEngine;

public class MovimentoAutomatico : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    public bool ativarMovimento = true;

    [Header("Status")]
    [SerializeField] private float distanciaPercorrida = 0f;
    [SerializeField] private Vector3 posicaoInicial;
    [SerializeField] private bool movimentoConcluido = false;
    
    [Header("Desligado")]
    public bool Basilar = false;

    // Velocidades convertidas para unidades do Unity (m/s)
    private float velocidadeXNegativo; // 3 km/h em m/s
    private float velocidadeXPositivo; // 20 m/h em m/s
    private float distanciaMaxima = 300f; // 300 metros

    void Start()
    {
        if (Basilar)
        {
            return;
        }

        // Converte velocidades para m/s (unidades do Unity)
        // 3 km/h = 0.833333 m/s | 20 m/h = 0.00555556 m/s
        velocidadeXNegativo = 0.833333f;
        velocidadeXPositivo = 0.00555556f;

        posicaoInicial = transform.position;

        if (!ativarMovimento)
        {
            enabled = false;
            return;
        }
    }

    void Update()
    {
        if (Basilar)
        {
            return;
        }

        if (!movimentoConcluido && ativarMovimento)
        {
            MoverObjeto();
            CalcularDistancia();
            VerificarDestruicao();
        }
    }

    private void MoverObjeto()
    {
        // Determina a direção da "frente" do objeto
        Vector3 direcaoFrente = transform.forward;

        // Calcula a velocidade base baseada na direção X
        float velocidade = (direcaoFrente.x < 0) ? velocidadeXNegativo : velocidadeXPositivo;

        // Move o objeto na direção da frente
        Vector3 movimento = direcaoFrente * velocidade * Time.deltaTime;
        transform.position += movimento;
    }

    private void CalcularDistancia()
    {
        // Calcula a distância percorrida desde a posição inicial
        distanciaPercorrida = Vector3.Distance(posicaoInicial, transform.position);
    }

    private void VerificarDestruicao()
    {
        if (distanciaPercorrida >= distanciaMaxima)
        {
            movimentoConcluido = true;
            DestruirObjeto();
        }
    }

    private void DestruirObjeto()
    {
        Debug.Log($"Objeto {gameObject.name} destruído após percorrer {distanciaPercorrida:F2} metros");
        Destroy(gameObject);
    }

    // Métodos públicos para controle externo
    public void PausarMovimento()
    {
        ativarMovimento = false;
    }

    public void RetomarMovimento()
    {
        ativarMovimento = true;
    }

    public void ReiniciarMovimento()
    {
        transform.position = posicaoInicial;
        distanciaPercorrida = 0f;
        movimentoConcluido = false;
        ativarMovimento = true;
    }

    public float GetDistanciaPercorrida()
    {
        return distanciaPercorrida;
    }

    public float GetDistanciaRestante()
    {
        return Mathf.Max(0f, distanciaMaxima - distanciaPercorrida);
    }

    public bool IsMovimentoConcluido()
    {
        return movimentoConcluido;
    }

    // Gizmos para visualização no Editor
    void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) return;

        // Desenha linha do trajeto percorrido
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(posicaoInicial, transform.position);

        // Desenha esfera na posição inicial
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(posicaoInicial, 0.5f);

        // Desenha esfera na posição atual
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.3f);
    }
}