using UnityEngine;
using System.Collections;

public class ItemSpawner : MonoBehaviour
{
    [Header("Objetos para Spawn")]
    public GameObject moeda;
    public GameObject obstaculo;
    public GameObject passaro;

    [Header("Locais de Spawn - Obstáculos")]
    public Transform localSpawnC0;
    public Transform localSpawnC1;
    public Transform localSpawnC2;

    [Header("Configurações")]
    public float intervaloObstaculos = 2f;
    public float intervaloPassaros = 1f;
    public float margemPassaros = 50f;

    [Header("Status")]
    [SerializeField] private bool executando = false;
    [SerializeField] private GameObject moedaAtual;

    private Camera cameraPrincipal;
    private Coroutine corrotinaObstaculos;
    private Coroutine corrotinaPassaros;

    void Start()
    {
        cameraPrincipal = Camera.main;
        IniciarSpawn();
    }

    public void IniciarSpawn()
    {
        if (executando) return;

        executando = true;
        corrotinaObstaculos = StartCoroutine(CorrotinaObstaculos());
        corrotinaPassaros = StartCoroutine(CorrotinaPassaros());

        Debug.Log("Spawner de itens iniciado");
    }

    public void PararSpawn()
    {
        if (!executando) return;

        executando = false;

        if (corrotinaObstaculos != null)
            StopCoroutine(corrotinaObstaculos);
        if (corrotinaPassaros != null)
            StopCoroutine(corrotinaPassaros);

        Debug.Log("Spawner de itens parado");
    }

    private IEnumerator CorrotinaObstaculos()
    {
        while (executando)
        {
            yield return new WaitForSeconds(intervaloObstaculos);

            if (executando)
            {
                SpawnarObstaculo();
            }
        }
    }

    private IEnumerator CorrotinaPassaros()
    {
        while (executando)
        {
            yield return new WaitForSeconds(10f * intervaloPassaros);

            if (executando)
            {
                SpawnarPassaro();
            }
        }
    }

    private void SpawnarObstaculo()
    {
        if (obstaculo == null) return;

        Transform localSpawn = SelecionarLocalObstaculoAleatorio();
        if (localSpawn != null)
        {
            GameObject novoObstaculo = Instantiate(obstaculo, localSpawn.position, Quaternion.LookRotation(Vector3.left));
            novoObstaculo.GetComponent<AudioSource>().enabled = true;
            novoObstaculo.GetComponent<Movimentoautomático>().enabled = true;
            Debug.Log($"Obstáculo spawnado em {localSpawn.name}");
        }

        if (passaro == null) return;

        localSpawn = SelecionarLocalObstaculoAleatorio();

        if (localSpawn != null)
        {
            GameObject novoPassaro = Instantiate(passaro, localSpawn.position, Quaternion.LookRotation(Vector3.left));
            novoPassaro.GetComponent<Movimentoautomático>().enabled = true;
            Debug.Log($"Pássaro spawnado na posição: {localSpawn}");
        }

        if (moeda == null || !executando) return;

        localSpawn = SelecionarLocalObstaculoAleatorio();

        if (localSpawn != null)
        {
            moedaAtual = Instantiate(moeda, localSpawn.position, Quaternion.LookRotation(Vector3.left));
            Transform instanciado = moedaAtual.transform;
            instanciado.GetComponent<Movimentoautomático>().enabled = true;
            //moedaAtual.GetComponent<Movimentoautomático>().ItemEssencial = false;

            Debug.Log($"Moeda spawnada na posição: {localSpawn}");
        }
    }

    private Transform SelecionarLocalObstaculoAleatorio()
    {
        Transform[] locais = { localSpawnC0, localSpawnC1, localSpawnC2 };
        Transform locaisValidos = locais[Random.Range(0, locais.Length)];
        return locaisValidos;
    }


    private Vector3 CalcularPosicaoTelaMoeda()
    {
        // 30% da tela no centro (35% a 65% em X e Y)
        float minX = 0.35f;
        float maxX = 0.65f;
        float minY = 0.35f;
        float maxY = 0.65f;

        float x = Random.Range(minX, maxX);
        float y = Random.Range(minY, maxY);

        return new Vector3(x * Screen.width, y * Screen.height, 0f);
    }

    private void SpawnarPassaro()
    {
        
    }

    private Vector3 CalcularPosicaoTelaPassaro()
    {
        // 30% da tela no topo (0-30% em Y)
        float minX = margemPassaros / Screen.width;
        float maxX = 1f - (margemPassaros / Screen.width);
        float minY = 0.7f; // Topo da tela
        float maxY = 1f;

        float x = Random.Range(minX, maxX);
        float y = Random.Range(minY, maxY);

        return new Vector3(x * Screen.width, y * Screen.height, 0f);
    }

    private Vector3 ConverterTelaParaMundo(Vector3 posicaoTela)
    {
        if (cameraPrincipal == null) return Vector3.zero;

        Ray ray = cameraPrincipal.ScreenPointToRay(posicaoTela);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            return hit.point;
        }

        // Se não houver colisão, usa um plano na posição Y=0 como fallback
        float distancia = (1100f - ray.origin.y) / ray.direction.y;
        return ray.origin + ray.direction * distancia;
    }
        
    // Métodos públicos para controle externo
    public void SpawnarObstaculoManual()
    {
        SpawnarObstaculo();
    }

    public void SpawnarPassaroManual()
    {
        SpawnarPassaro();
    }

    public bool IsExecutando()
    {
        return executando;
    }

    void OnDestroy()
    {
        PararSpawn();
    }

    // Gizmos para visualização no Editor
    void OnDrawGizmosSelected()
    {
        if (cameraPrincipal == null) return;

        // Desenha área de spawn das moedas
        DesenharAreaSpawnMoedas();

        // Desenha área de spawn dos pássaros
        DesenharAreaSpawnPassaros();

        // Desenha locais de spawn dos obstáculos
        DesenharLocaisObstaculos();
    }

    private void DesenharAreaSpawnMoedas()
    {
        Vector3 centroTela = new Vector3(0.5f, 0.5f, 0f);
        Vector3 tamanho = new Vector3(0.3f, 0.3f, 0f);

        DesenharRetanguloNaTela(centroTela, tamanho, Color.green, "Área Moedas");
    }

    private void DesenharAreaSpawnPassaros()
    {
        Vector3 centroTela = new Vector3(0.5f, 0.85f, 0f); // Topo
        Vector3 tamanho = new Vector3(1f, 0.3f, 0f);

        DesenharRetanguloNaTela(centroTela, tamanho, Color.blue, "Área Pássaros");
    }

    private void DesenharRetanguloNaTela(Vector3 centroNormalizado, Vector3 tamanhoNormalizado, Color cor, string label)
    {
        Gizmos.color = cor;

        Vector3 cantoInferiorEsquerdo = new Vector3(
            (centroNormalizado.x - tamanhoNormalizado.x / 2) * Screen.width,
            (centroNormalizado.y - tamanhoNormalizado.y / 2) * Screen.height,
            0f
        );

        Vector3 cantoSuperiorDireito = new Vector3(
            (centroNormalizado.x + tamanhoNormalizado.x / 2) * Screen.width,
            (centroNormalizado.y + tamanhoNormalizado.y / 2) * Screen.height,
            0f
        );

        // Converter para coordenadas de mundo
        Vector3 mundoInferiorEsquerdo = ConverterTelaParaMundo(cantoInferiorEsquerdo);
        Vector3 mundoSuperiorDireito = ConverterTelaParaMundo(cantoSuperiorDireito);

        if (mundoInferiorEsquerdo != Vector3.zero && mundoSuperiorDireito != Vector3.zero)
        {
            Vector3[] vertices = new Vector3[4];
            vertices[0] = mundoInferiorEsquerdo;
            vertices[1] = new Vector3(mundoSuperiorDireito.x, mundoInferiorEsquerdo.y, mundoInferiorEsquerdo.z);
            vertices[2] = mundoSuperiorDireito;
            vertices[3] = new Vector3(mundoInferiorEsquerdo.x, mundoSuperiorDireito.y, mundoSuperiorDireito.z);

            Gizmos.DrawLine(vertices[0], vertices[1]);
            Gizmos.DrawLine(vertices[1], vertices[2]);
            Gizmos.DrawLine(vertices[2], vertices[3]);
            Gizmos.DrawLine(vertices[3], vertices[0]);
        }
    }

    private void DesenharLocaisObstaculos()
    {
        DesenharGizmoSpawn(localSpawnC0, Color.red, "Obstáculo 0");
        DesenharGizmoSpawn(localSpawnC1, Color.red, "Obstáculo 1");
        DesenharGizmoSpawn(localSpawnC2, Color.red, "Obstáculo 2");
    }

    private void DesenharGizmoSpawn(Transform spawn, Color cor, string nome)
    {
        if (spawn != null)
        {
            Gizmos.color = cor;
            Gizmos.DrawWireSphere(spawn.position, 0.3f);
            Gizmos.DrawRay(spawn.position, spawn.forward * 1f);
        }
    }
}