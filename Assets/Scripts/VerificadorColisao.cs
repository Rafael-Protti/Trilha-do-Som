using UnityEngine;

public class VerificadorColisao : MonoBehaviour
{
    [Header("Status do Jogador")]
    [SerializeField] private int vida;
    [SerializeField] private int moeda = 0;

    [Header("Configurações")]
    [SerializeField] private bool debugLog = true;

    private Jogador scriptJogador;
    private BoxCollider boxCollider;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();

        if (boxCollider == null)
        {
            Debug.LogError("BoxCollider não encontrado neste GameObject!");
        }

        // Busca o script Jogador no mesmo GameObject
        scriptJogador = GetComponent<Jogador>();

        if (scriptJogador == null)
        {
            Debug.LogError("Script Jogador não encontrado no mesmo GameObject!");
            return;
        }
        vida = scriptJogador.vida;
    }

    void OnTriggerEnter(Collider other)
    {
        VerificarColisao(other);
    }

    void OnCollisionEnter(Collision collision)
    {
        VerificarColisao(collision.collider);
    }

    private void VerificarColisao(Collider other)
    {
        string nomeObjeto = other.gameObject.name.ToLower();

        if (debugLog)
        {
            Debug.Log($"Colisão detectada com: {other.gameObject.name}");
        }

        // Verifica se contém "obstaculo" no nome
        if (nomeObjeto.Contains("obstaculo"))
        {
            ReduzirVida();
            if (debugLog) Debug.Log($"Obstáculo detectado! Vida: {vida}");
        }

        // Verifica se contém "moeda" no nome
        if (nomeObjeto.Contains("moeda"))
        {
            AdicionarMoeda();
            if (debugLog) Debug.Log($"Moeda coletada! Total: {moeda}");
        }

        // Verifica se contém "passaro" no nome
        if (nomeObjeto.Contains("passaro"))
        {
            if (debugLog) Debug.Log("Pássaro detectado - Nenhuma ação tomada");
        }

        // Verifica se contém "chegada" no nome
        if (nomeObjeto.Contains("ChegadaFinal"))
        {
            if (debugLog) Debug.Log("Você chegou ao final");
            Vitoria();
        }
    }

    private void ReduzirVida()
    {
        vida = scriptJogador.vida;

        // Aqui você pode adicionar mais lógica quando a vida chegar a zero
        if (vida <= 0)
        {
           scriptJogador.Morrendo();
            Debug.Log("Game Over! Vida zerada.");
            // Adicione aqui lógica de game over se necessário
        }

        if (vida > 0)
        {
            vida--;
            scriptJogador.Trombando();
        }

        scriptJogador.vida = vida;
    }

    private void Vitoria()
    {
        Debug.Log("Você venceu o jogo!");
        scriptJogador.Vencendo();
    }

    private void AdicionarMoeda()
    {
        moeda++;

        // Opcional: Destruir a moeda após coleta
        // Collider other precisaria ser passado como parâmetro para destruir o objeto específico
    }

    // Métodos públicos para acesso externo
    public int GetVida()
    {
        return vida;
    }

    public int GetMoeda()
    {
        return moeda;
    }

    public void SetVida(int novaVida)
    {
        vida = Mathf.Max(0, novaVida);
    }

    public void SetMoeda(int novaMoeda)
    {
        moeda = Mathf.Max(0, novaMoeda);
    }

    // Método para reiniciar status
    public void ReiniciarStatus()
    {
        vida = 3;
        moeda = 0;
    }

    // Gizmo para visualizar o BoxCollider
    void OnDrawGizmosSelected()
    {
        if (boxCollider == null)
            boxCollider = GetComponent<BoxCollider>();

        if (boxCollider != null && boxCollider.enabled)
        {
            Gizmos.color = new Color(0, 1, 0, 0.3f); // Verde transparente
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawCube(boxCollider.center, boxCollider.size);

            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(boxCollider.center, boxCollider.size);
        }
    }
}