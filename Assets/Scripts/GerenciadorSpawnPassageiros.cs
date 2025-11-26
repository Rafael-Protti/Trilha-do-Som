using UnityEngine;
using System.Collections;

public class GerenciadorSpawnPassageiros : MonoBehaviour
{
    [Header("Objetos para Spawn")]
    public GameObject ruidoD;
    public GameObject ruidoE;
    public GameObject ruidoT;

    [Header("Locais de Spawn")]
    public Transform localSpawnE;
    public Transform localSpawnD;
    public Transform localSpawnT;

    [Header("Configurações")]
    public float intervaloPassageiros = 1f;

    [Header("Status")]
    [SerializeField] private int comportamentoSelecionado = -1;
    [SerializeField] private bool executando = false;
    
    private Coroutine corrotinaPassageiros;

    void Start()
    {
        IniciarSistema();
    }

    public void IniciarSistema()
    {
        if (executando) return;

        SelecionarInvocacao();
        IniciarCorrotinas();
        executando = true;

        Debug.Log($"Sistema iniciado com comportamento: {ObterNomeComportamento(comportamentoSelecionado)}");
    }

    public void PararSistema()
    {
        if (!executando) return;

        if (corrotinaPassageiros != null)
            StopCoroutine(corrotinaPassageiros);

        executando = false;
        Debug.Log("Sistema parado");
    }

    private void SelecionarInvocacao()
    {
        // 0 = Esquerda, 1 = Direita, 2 = Trás
        comportamentoSelecionado = Random.Range(0, 3);
    }

    private void IniciarCorrotinas()
    {
        corrotinaPassageiros = StartCoroutine(CorrotinaPassageiros());
    }

    private IEnumerator CorrotinaPassageiros()
    {
        while (executando)
        {
            yield return new WaitForSeconds(2f * intervaloPassageiros);

            if (executando)
            {
                InvocarRuidoConformeComportamento();
            }
        }
    }


    private void InvocarRuidoConformeComportamento()
    {
        GameObject ruidoParaSpawn = null;
        Transform localSpawn = null;

        switch (comportamentoSelecionado)
        {
            case 0: // Esquerda
                ruidoParaSpawn = ruidoE;
                localSpawn = localSpawnE;
                break;
            case 1: // Direita
                ruidoParaSpawn = ruidoD;
                localSpawn = localSpawnD;
                break;
            case 2: // Trás
                ruidoParaSpawn = ruidoT;
                localSpawn = localSpawnT;
                break;
        }

        if (ruidoParaSpawn != null && localSpawn != null)
        {
            GameObject passageiro = Instantiate(ruidoParaSpawn, localSpawn.position, localSpawn.rotation);
            passageiro.GetComponent<Movimentoautomático>().enabled = true;
            Debug.Log($"Invocado {ruidoParaSpawn.name} em {localSpawn.name}");
        }
        else
        {
            Debug.LogWarning("Objeto de ruído ou local de spawn não configurado!");
        }
    }

    // Métodos públicos para controle externo
    public void AlterarComportamento(int novoComportamento)
    {
        if (novoComportamento >= 0 && novoComportamento <= 2)
        {
            comportamentoSelecionado = novoComportamento;
            Debug.Log($"Comportamento alterado para: {ObterNomeComportamento(comportamentoSelecionado)}");
        }
    }

    public void ReiniciarComComportamentoAleatorio()
    {
        PararSistema();
        SelecionarInvocacao();
        IniciarCorrotinas();
        executando = true;
    }

    // Métodos de utilidade
    private string ObterNomeComportamento(int comportamento)
    {
        switch (comportamento)
        {
            case 0: return "Esquerda";
            case 1: return "Direita";
            case 2: return "Trás";
            default: return "Desconhecido";
        }
    }

    public int GetComportamentoAtual()
    {
        return comportamentoSelecionado;
    }

    public string GetNomeComportamentoAtual()
    {
        return ObterNomeComportamento(comportamentoSelecionado);
    }

    public bool IsExecutando()
    {
        return executando;
    }

    void OnDestroy()
    {
        PararSistema();
    }

    // Gizmos para visualização no Editor
    void OnDrawGizmosSelected()
    {
        // Desenha gizmos para os locais de spawn
        DesenharGizmoSpawn(localSpawnE, Color.red, "Spawn E");
        DesenharGizmoSpawn(localSpawnD, Color.blue, "Spawn D");
        DesenharGizmoSpawn(localSpawnT, Color.green, "Spawn T");
    }

    private void DesenharGizmoSpawn(Transform spawn, Color cor, string nome)
    {
        if (spawn != null)
        {
            Gizmos.color = cor;
            Gizmos.DrawWireSphere(spawn.position, 0.5f);
            Gizmos.DrawRay(spawn.position, spawn.forward * 2f);

#if UNITY_EDITOR
            UnityEditor.Handles.Label(spawn.position + Vector3.up, nome);
#endif
        }
    }
}