using UnityEngine;
using System.Collections;

public class ItemSpawner : MonoBehaviour
{
    [Header("Objetos para Spawn")]
    public GameObject moeda;
    public GameObject obstaculo;
    public GameObject passaro;
    public GameObject ruidoE;
    public GameObject ruidoD;

    [Header("Locais de Spawn dos obstaculos")]
    public Transform localSpawnC0;
    public Transform localSpawnC1;
    public Transform localSpawnC2;
    [Header("Locais de Spawn dos ruidos")]
    public Transform localSpawnE1;
    public Transform localSpawnE2;
    public Transform localSpawnE3;
    public Transform localSpawnE4;
    public Transform localSpawnE5;
    public Transform localSpawnE6;
    public Transform localSpawnD1;
    public Transform localSpawnD2;
    public Transform localSpawnD3;
    public Transform localSpawnD4;
    public Transform localSpawnD5;
    public Transform localSpawnD6;

    [Header("Configurações")]
    public float intervaloMoedas = 2f;
    public float intervaloObstaculos = 2f;
    public float intervaloPassaros = 1f;
    public float intervaloRuidos = 0.2f;

    [Header("Status")]
    [SerializeField] private bool executando = false;
    [SerializeField] private GameObject moedaAtual;

    private Camera cameraPrincipal;
    private Coroutine corrotinaObstaculos;
    private Coroutine corrotinaMoedas;
    private Coroutine corrotinaPassaros;
    private Coroutine corrotinaRuidos;

    void Start()
    {
        cameraPrincipal = Camera.main;
        IniciarSpawn();
    }

    public void IniciarSpawn()
    {
        if (executando) return;

        executando = true;
        SpawnarRuidosManual();

        corrotinaObstaculos = StartCoroutine(CorrotinaObstaculos());
        corrotinaPassaros = StartCoroutine(CorrotinaPassaros());
        corrotinaRuidos = StartCoroutine(CorrotinaRuidos());
        corrotinaMoedas = StartCoroutine(CorrotinaMoedas());

        SpawnarObstaculoManual();
        SpawnarRuidosManual();


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
        if (corrotinaRuidos != null)
            StopCoroutine(corrotinaRuidos);
        if (corrotinaMoedas != null)
            StopCoroutine(corrotinaMoedas);

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

    private IEnumerator CorrotinaMoedas()
    {
        while (executando)
        {
            yield return new WaitForSeconds(intervaloObstaculos);

            if (executando)
            {
                SpawnarMoedas();
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

    private IEnumerator CorrotinaRuidos()
    {
        while (executando)
        {
            yield return new WaitForSeconds(10f * intervaloRuidos);

            if (executando)
            {
                SpawnarRuidosE();
                SpawnarRuidosD();
            }
        }
    }

    private void SpawnarObstaculo()
    {
        if (obstaculo == null) return;

        Transform localSpawn = SelecionarLocalObstaculoAleatorio();
        if (localSpawn != null)
        {
            GameObject novoObstaculo = Instantiate(obstaculo, localSpawn.position, Quaternion.LookRotation(Vector3.forward));
            novoObstaculo.GetComponent<AudioSource>().enabled = true;
            novoObstaculo.GetComponent<Movimentoautomático>().enabled = true;
            novoObstaculo.GetComponent<SomNoObstaculo>().enabled = true;
            novoObstaculo.GetComponent<ResonanceAudioSource>().enabled = true;
            Debug.Log($"Obstáculo spawnado em {localSpawn.name}");
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

    private void SpawnarMoedas()
    {
        if (obstaculo == null) return;

        Transform localSpawn = SelecionarLocalObstaculoAleatorio();
        if (localSpawn != null)
        {
            GameObject novaMoeda = Instantiate(moeda, localSpawn.position, Quaternion.LookRotation(Vector3.left));
            novaMoeda.GetComponent<AudioSource>().enabled = true;
            novaMoeda.GetComponent<Movimentoautomático>().enabled = true;
            novaMoeda.GetComponent<SomNoObstaculo>().enabled = true;
            novaMoeda.GetComponent<ResonanceAudioSource>().enabled = true;
            Debug.Log($"Obstáculo spawnado em {localSpawn.name}");
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

    private void SpawnarPassaro()
    {
        if (passaro == null) return;

        Transform localSpawn = SelecionarLocalPassaroAleatorio();

        if (localSpawn != null)
        {
            GameObject novoPassaro = Instantiate(passaro, localSpawn.position, Quaternion.LookRotation(Vector3.left));
            novoPassaro.GetComponent<AudioSource>().enabled = true;
            novoPassaro.GetComponent<Movimentoautomático>().enabled = true;
            novoPassaro.GetComponent<SomNoObstaculo>().enabled = true;
            novoPassaro.GetComponent<ResonanceAudioSource>().enabled = true;
            Debug.Log($"Pássaro spawnado na posição: {localSpawn}");
        }
    }

    private void SpawnarRuidosE()
    {
        if (passaro == null) return;

        Transform localSpawn = SelecionarLocalRuidosEAleatorio();

        if (localSpawn != null)
        {
            GameObject novoRuido = Instantiate(passaro, localSpawn.position, Quaternion.LookRotation(Vector3.left));
            novoRuido.GetComponent<AudioSource>().enabled = true;
            novoRuido.GetComponent<Movimentoautomático>().enabled = true;
            novoRuido.GetComponent<SomNoObstaculo>().enabled = true;
            novoRuido.GetComponent<ResonanceAudioSource>().enabled = true;
            Debug.Log($"Pássaro spawnado na posição: {localSpawn}");
        }
    }


    private void SpawnarRuidosD()
    {
        if (passaro == null) return;

        Transform localSpawn = SelecionarLocalRuidosDAleatorio();

        if (localSpawn != null)
        {
            GameObject novoRuido = Instantiate(passaro, localSpawn.position, Quaternion.LookRotation(Vector3.left));
            novoRuido.GetComponent<AudioSource>().enabled = true;
            novoRuido.GetComponent<Movimentoautomático>().enabled = true;
            novoRuido.GetComponent<SomNoObstaculo>().enabled = true;
            novoRuido.GetComponent<ResonanceAudioSource>().enabled = true;
            Debug.Log($"Pássaro spawnado na posição: {localSpawn}");
        }
    }

    private Transform SelecionarLocalObstaculoAleatorio()
    {
        Transform[] locais = { localSpawnC0, localSpawnC1, localSpawnC2 };
        Transform locaisValidos = locais[Random.Range(0, locais.Length)];
        return locaisValidos;
    }

    private Transform SelecionarLocalPassaroAleatorio()
    {
        Transform[] locais = { localSpawnD1, localSpawnE1 };
        Transform locaisValidos = locais[Random.Range(0, locais.Length)];
        return locaisValidos;
    }

    private Transform SelecionarLocalRuidosEAleatorio()
    {
        Transform[] locais = { localSpawnE1, localSpawnE2, localSpawnE3, localSpawnE4, localSpawnE5, localSpawnE6 };
        Transform locaisValidos = locais[Random.Range(0, locais.Length)];
        return locaisValidos;
    }

    private Transform SelecionarLocalRuidosDAleatorio()
    {
        Transform[] locais = { localSpawnD1, localSpawnD2, localSpawnD3, localSpawnD4, localSpawnD5, localSpawnD6 };
        Transform locaisValidos = locais[Random.Range(0, locais.Length)];
        return locaisValidos;
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

    public void SpawnarRuidosManual()
    {
        SpawnarRuidosE();
        SpawnarRuidosD();
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

        // Desenha área de spawn dos pássaros
        DesenharLocaisPassaros();

        // Desenha locais de spawn dos obstáculos
        DesenharLocaisObstaculos();
    }

    private void DesenharLocaisObstaculos()
    {
        DesenharGizmoSpawn(localSpawnC0, Color.red, "Obstáculo 0");
        DesenharGizmoSpawn(localSpawnC1, Color.red, "Obstáculo 1");
        DesenharGizmoSpawn(localSpawnC2, Color.red, "Obstáculo 2");
    }


    private void DesenharLocaisPassaros()
    {
        DesenharGizmoSpawn(localSpawnD1, Color.red, "Ruidos da direita");
        DesenharGizmoSpawn(localSpawnE1, Color.red, "Ruidos da esquerda");
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