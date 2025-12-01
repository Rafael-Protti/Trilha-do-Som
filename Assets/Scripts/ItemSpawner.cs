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
    public GameObject chegadaFinal;

    [Header("Local de Spawn dos obstaculos")]
    public Transform localSpawnC;

    [Header("Local de Spawn da moeda")]
    public Transform localSpawnM;

    [Header("Locais de Spawn dos ruidos")]
    public Transform localSpawnE1;
    public Transform localSpawnE2;
    public Transform localSpawnE3;
    public Transform localSpawnE4;
    public Transform localSpawnE5;
    public Transform localSpawnE6;
    public Transform localSpawnE7;
    public Transform localSpawnE8;
    public Transform localSpawnE9;
    public Transform localSpawnE10;
    public Transform localSpawnE11;
    public Transform localSpawnE12;
    public Transform localSpawnD1;
    public Transform localSpawnD2;
    public Transform localSpawnD3;
    public Transform localSpawnD4;
    public Transform localSpawnD5;
    public Transform localSpawnD6;
    public Transform localSpawnD7;
    public Transform localSpawnD8;
    public Transform localSpawnD9;
    public Transform localSpawnD10;
    public Transform localSpawnD11;
    public Transform localSpawnD12;
    
    [Header("Local de Spawn da chegada")]
    public Transform localSpawnChegada;

    [Header("Configuraçőes")]
    public float intervaloMoedas = 2f;
    public float intervaloObstaculos = 2f;
    public float intervaloPassaros = 1f;
    public float intervaloRuidos = 0.2f;

    [Header("Status")]
    [SerializeField] private bool executando = false;
    [SerializeField] private GameObject moedaAtual;

    private GameObject jogador;
    private Camera cameraPrincipal;
    private Coroutine corrotinaObstaculos;
    private Coroutine corrotinaMoedas;
    private Coroutine corrotinaPassaros;
    private Coroutine corrotinaRuidos;
	
	
    private Vector3 novaPosicao;

    void Start()
    {
        cameraPrincipal = Camera.main;
        // Encontrar o objeto Jogador automaticamente
        jogador = GameObject.Find("Jogador");
        if (jogador == null)
        {
            Debug.LogError("Objeto Jogador não encontrado na cena!");
        }

        IniciarSpawn();
    }

    public void IniciarSpawn()
    {
        if (executando) return;

        executando = true;

        corrotinaObstaculos = StartCoroutine(CorrotinaObstaculos());
        corrotinaPassaros = StartCoroutine(CorrotinaPassaros());
        corrotinaRuidos = StartCoroutine(CorrotinaRuidos());
        corrotinaMoedas = StartCoroutine(CorrotinaMoedas());

        SpawnarRuidosManual();
        SpawnarChegada();
        SpawnarRuidosE();
        SpawnarRuidosD();

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
            yield return new WaitForSeconds(5f * intervaloRuidos);

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

        SelecionarLocalObstaculoComPosicaoJogador();

        GameObject novoObstaculo = Instantiate(obstaculo, novaPosicao, Quaternion.LookRotation(Vector3.forward));
        novoObstaculo.GetComponent<AudioSource>().enabled = true;
        novoObstaculo.GetComponent<SomNoObstaculo>().enabled = true;
        novoObstaculo.GetComponent<Movimentoautomático>().enabled = true;
        novoObstaculo.GetComponent<ResonanceAudioSource>().enabled = true;
        Debug.Log($"Obstáculo spawnado em {localSpawnC.name} com posição X do jogador");
    }

    private void SpawnarMoedas()
    {
        if (moeda == null || !executando) return;

        SelecionarLocalMoedaComPosicaoContraria();

        moedaAtual = Instantiate(moeda, novaPosicao, Quaternion.LookRotation(Vector3.left));
        Transform instanciado = moedaAtual.transform;
        instanciado.GetComponent<Movimentoautomático>().enabled = true;
        instanciado.GetComponent<AudioSource>().enabled = true;
        instanciado.GetComponent<ResonanceAudioSource>().enabled = true;
        instanciado.GetComponent<SomNoObstaculo>().enabled = true;
        Debug.Log($"Moeda spawnada na posição contrária ao jogador: {localSpawnM.position}");
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
            Debug.Log($"Pássaro spawnado na posiçăo: {localSpawn}");
        }
    }

    private void SpawnarRuidosE()
    {
        if (passaro == null) return;

        Transform localSpawn = SelecionarLocalRuidosEAleatorio();

        if (localSpawn != null)
        {
            GameObject novoRuido = Instantiate(ruidoE, localSpawn.position, Quaternion.LookRotation(Vector3.left));
            novoRuido.GetComponent<AudioSource>().enabled = true;
            novoRuido.GetComponent<Movimentoautomático>().enabled = true;
            novoRuido.GetComponent<SomNoObstaculo>().enabled = true;
            novoRuido.GetComponent<ResonanceAudioSource>().enabled = true;
            Debug.Log($"Pássaro spawnado na posiçăo: {localSpawn}");
        }
    }

    private void SpawnarRuidosD()
    {
        if (passaro == null) return;

        Transform localSpawn = SelecionarLocalRuidosDAleatorio();

        if (localSpawn != null)
        {
            GameObject novoRuido = Instantiate(ruidoD, localSpawn.position, Quaternion.LookRotation(Vector3.left));
            novoRuido.GetComponent<AudioSource>().enabled = true;
            novoRuido.GetComponent<Movimentoautomático>().enabled = true;
            novoRuido.GetComponent<SomNoObstaculo>().enabled = true;
            novoRuido.GetComponent<ResonanceAudioSource>().enabled = true;
            Debug.Log($"Pássaro spawnado na posiçăo: {localSpawn}");
        }
    }

    private void SpawnarChegada()
    {
        if (obstaculo == null) return;

        Transform localSpawn = localSpawnChegada;
        if (localSpawn != null)
        {
            GameObject chegada = Instantiate(chegadaFinal, localSpawn.position, Quaternion.LookRotation(Vector3.forward));
            chegada.GetComponent<AudioSource>().enabled = true;
            chegada.GetComponent<Movimentoautomático>().enabled = true;
            chegada.GetComponent<SomNoObstaculo>().enabled = true;
            chegada.GetComponent<ResonanceAudioSource>().enabled = true;
            Debug.Log($"Obstáculo spawnado em {localSpawn.name}");
        }
    }

    // Seleciona o localSpawnM mas com a posição X contrária ao jogador
    private void SelecionarLocalMoedaComPosicaoContraria()
    {
        if (jogador == null || jogador.transform.position.x == 0)
        {
            Debug.LogWarning("Jogador não encontrado ou no centro da tela, usando spawn padrão");
             novaPosicao = localSpawnM.position;
        }

        if (localSpawnM == null)
        {
            Debug.LogError("localSpawnM não está atribuído!");
            novaPosicao = localSpawnM.position;
        }


        // Cria uma nova posição mantendo Y e Z do localSpawnM, mas com X contrário ao jogador
        novaPosicao = new Vector3(
            -jogador.transform.position.x, // Posição X contrária
            localSpawnM.position.y,
            localSpawnM.position.z
            );
    }

    // Seleciona um local de spawn C mas com a posição X do jogador
    private void SelecionarLocalObstaculoComPosicaoJogador()
    {
        // Cria uma nova posição mantendo Y e Z do local de spawn, mas com X do jogador
			novaPosicao = new Vector3(
            jogador.transform.position.x,
            localSpawnC.position.y,
            localSpawnC.position.z
        );
    }

    private Transform SelecionarLocalPassaroAleatorio()
    {
        Transform[] locais = { localSpawnD1, localSpawnE1 };
        Transform locaisValidos = locais[Random.Range(0, locais.Length)];
        return locaisValidos;
    }

    private Transform SelecionarLocalRuidosEAleatorio()
    {
        Transform[] locais = { localSpawnE1, localSpawnE2, localSpawnE3, localSpawnE4, localSpawnE5, localSpawnE6, localSpawnE7, localSpawnE8, localSpawnE9, localSpawnE10, localSpawnE11, localSpawnE12 };
        Transform locaisValidos = locais[Random.Range(0, locais.Length)];
        return locaisValidos;
    }

    private Transform SelecionarLocalRuidosDAleatorio()
    {
        Transform[] locais = { localSpawnD1, localSpawnD2, localSpawnD3, localSpawnD4, localSpawnD5, localSpawnD6, localSpawnD7, localSpawnD8, localSpawnD9, localSpawnD10, localSpawnD11, localSpawnD12 };
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

    // Gizmos para visualizaçăo no Editor
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
        DesenharGizmoSpawn(localSpawnC, Color.red, "Obstáculo 0");
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