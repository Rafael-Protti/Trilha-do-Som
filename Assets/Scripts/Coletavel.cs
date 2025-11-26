using UnityEngine;

public class Coletavel : MonoBehaviour
{
    [Header("Configurações de Coleta")]
    public AudioClip somColetado;
    public TipoColetavel tipo = TipoColetavel.Moeda;

    [Header("Estatísticas")]
    public static int totalMoedasSpawnadas = 0;
    public static int totalPassarosSpawnados = 0;
    public static int totalMoedasColetadas = 0;
    public static int totalPassarosColetados = 0;

    [Header("Desligado")]
    public bool Basilar = false;

    private AudioSource audioSource;
    private bool foiColetado = false;
    private Collider objetoCollider;
    private Renderer objetoRenderer;

    public enum TipoColetavel
    {
        Moeda,
        Passaro
    }

    void Start()
    {
        if (Basilar)
        {
            return;
        }

        // Configura componentes
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        objetoCollider = GetComponent<Collider>();
        objetoRenderer = GetComponent<Renderer>();

        // Contabiliza o spawn
        ContabilizarSpawn();

        Debug.Log($"{tipo} spawnado. Total: {GetTotalSpawnado(tipo)}");
    }

    void Update()
    {
        if (Basilar)
        {
            return;
        }
        // Verifica clique do mouse
        if (Input.GetMouseButtonDown(0) && !foiColetado)
        {
            VerificarCliqueNoObjeto();
        }
    }

    private void VerificarCliqueNoObjeto()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider == objetoCollider)
            {
                Coletar();
            }
        }
    }

    public void Coletar()
    {
        if (foiColetado) return;

        foiColetado = true;

        // Contabiliza a coleta
        ContabilizarColeta();

        // Toca o som de coleta
        if (somColetado != null)
        {
            audioSource.PlayOneShot(somColetado);
            Debug.Log($"{tipo} coletado! Som tocando...");
        }
        else
        {
            Debug.LogWarning($"Som de coleta não configurado para {tipo}!");
        }

        // Desativa visualmente o objeto
        if (objetoRenderer != null)
            objetoRenderer.enabled = false;
        if (objetoCollider != null)
            objetoCollider.enabled = false;

        // Destroi o objeto após o som tocar
        if (somColetado != null)
        {
            Destroy(gameObject, somColetado.length);
        }
        else
        {
            Destroy(gameObject, 0.1f);
        }

        Debug.Log($"{tipo} coletado! Total coletado: {GetTotalColetado(tipo)}");
    }

    private void ContabilizarSpawn()
    {
        switch (tipo)
        {
            case TipoColetavel.Moeda:
                totalMoedasSpawnadas++;
                break;
            case TipoColetavel.Passaro:
                totalPassarosSpawnados++;
                break;
        }
    }

    private void ContabilizarColeta()
    {
        switch (tipo)
        {
            case TipoColetavel.Moeda:
                totalMoedasColetadas++;
                break;
            case TipoColetavel.Passaro:
                totalPassarosColetados++;
                break;
        }
    }

    // Métodos estáticos para acessar estatísticas
    public static int GetTotalSpawnado(TipoColetavel tipo)
    {
        return tipo == TipoColetavel.Moeda ? totalMoedasSpawnadas : totalPassarosSpawnados;
    }

    public static int GetTotalColetado(TipoColetavel tipo)
    {
        return tipo == TipoColetavel.Moeda ? totalMoedasColetadas : totalPassarosColetados;
    }

    public static int GetTotalNaoColetado(TipoColetavel tipo)
    {
        return GetTotalSpawnado(tipo) - GetTotalColetado(tipo);
    }

    public static float GetTaxaColeta(TipoColetavel tipo)
    {
        int spawnados = GetTotalSpawnado(tipo);
        if (spawnados == 0) return 0f;

        return (float)GetTotalColetado(tipo) / spawnados;
    }

    public static void ResetarEstatisticas()
    {
        totalMoedasSpawnadas = 0;
        totalPassarosSpawnados = 0;
        totalMoedasColetadas = 0;
        totalPassarosColetados = 0;

        Debug.Log("Estatísticas resetadas!");
    }

    // Método para coleta automática por colisão (opcional)
    private void OnTriggerEnter(Collider other)
    {
        // Se quiser que seja coletado automaticamente ao tocar no jogador
        // if (other.CompareTag("Player"))
        // {
        //     Coletar();
        // }
    }

    void OnDestroy()
    {
        if (!foiColetado)
        {
            Debug.Log($"{tipo} destruído sem ser coletado");
        }
    }
}