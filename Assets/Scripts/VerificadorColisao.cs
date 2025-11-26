using UnityEngine;

public class VerificadorColisao : MonoBehaviour
{
    [Header("Status do Jogador")]
    [SerializeField] private int vida = 3;
    [SerializeField] private int moeda = 0;

    [Header("Configurações")]
    [SerializeField] private bool debugLog = true;

    private BoxCollider boxCollider;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();

        if (boxCollider == null)
        {
            Debug.LogError("BoxCollider não encontrado neste GameObject!");
        }
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
        else if (nomeObjeto.Contains("moeda"))
        {
            AdicionarMoeda();
            if (debugLog) Debug.Log($"Moeda coletada! Total: {moeda}");
        }
        // Verifica se contém "passaro" no nome
        else if (nomeObjeto.Contains("passaro"))
        {
            if (debugLog) Debug.Log("Pássaro detectado - Nenhuma ação tomada");
            // Não faz nada, conforme solicitado
        }
    }

    private void ReduzirVida()
    {
        vida = Mathf.Max(0, vida - 1);

        // Aqui você pode adicionar mais lógica quando a vida chegar a zero
        if (vida <= 0)
        {
            Debug.Log("Game Over! Vida zerada.");
            // Adicione aqui lógica de game over se necessário
        }
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