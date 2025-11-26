using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MoveJogador : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    public float velocidade = 5f;

    [Header("Limites de Movimento")]
    public float limiteEsquerdo = -5f;
    public float limiteDireito = 5f;

    [Header("Botões UI - Arraste os botões aqui")]
    public Button botaoEsquerda;
    public Button botaoDireita;

    [Header("Status")]
    [SerializeField] private bool movendoEsquerda = false;
    [SerializeField] private bool movendoDireita = false;

    void Start()
    {
        ConfigurarBotoes();
    }

    void Update()
    {
        ProcessarInputTeclado();
        ProcessarInputGamepad();
        MoverJogador();
    }

    private void ConfigurarBotoes()
    {
        // Configura eventos para o botão esquerdo
        if (botaoEsquerda != null)
        {
            // Adiciona EventTrigger para detectar pressionar e soltar
            EventTrigger triggerEsquerda = botaoEsquerda.gameObject.GetComponent<EventTrigger>();
            if (triggerEsquerda == null)
            {
                triggerEsquerda = botaoEsquerda.gameObject.AddComponent<EventTrigger>();
            }

            // Evento de pressionar
            EventTrigger.Entry entryPointerDown = new EventTrigger.Entry();
            entryPointerDown.eventID = EventTriggerType.PointerDown;
            entryPointerDown.callback.AddListener((data) => { IniciarMovimentoEsquerda(); });
            triggerEsquerda.triggers.Add(entryPointerDown);

            // Evento de soltar
            EventTrigger.Entry entryPointerUp = new EventTrigger.Entry();
            entryPointerUp.eventID = EventTriggerType.PointerUp;
            entryPointerUp.callback.AddListener((data) => { PararMovimentoEsquerda(); });
            triggerEsquerda.triggers.Add(entryPointerUp);

            // Evento de cancelar (quando o toque sai do botão)
            EventTrigger.Entry entryPointerExit = new EventTrigger.Entry();
            entryPointerExit.eventID = EventTriggerType.PointerExit;
            entryPointerExit.callback.AddListener((data) => { PararMovimentoEsquerda(); });
            triggerEsquerda.triggers.Add(entryPointerExit);

            Debug.Log("Botão esquerdo configurado");
        }
        else
        {
            Debug.LogWarning("Botão esquerdo não atribuído!");
        }

        // Configura eventos para o botão direito
        if (botaoDireita != null)
        {
            // Adiciona EventTrigger para detectar pressionar e soltar
            EventTrigger triggerDireita = botaoDireita.gameObject.GetComponent<EventTrigger>();
            if (triggerDireita == null)
            {
                triggerDireita = botaoDireita.gameObject.AddComponent<EventTrigger>();
            }

            // Evento de pressionar
            EventTrigger.Entry entryPointerDown = new EventTrigger.Entry();
            entryPointerDown.eventID = EventTriggerType.PointerDown;
            entryPointerDown.callback.AddListener((data) => { IniciarMovimentoDireita(); });
            triggerDireita.triggers.Add(entryPointerDown);

            // Evento de soltar
            EventTrigger.Entry entryPointerUp = new EventTrigger.Entry();
            entryPointerUp.eventID = EventTriggerType.PointerUp;
            entryPointerUp.callback.AddListener((data) => { PararMovimentoDireita(); });
            triggerDireita.triggers.Add(entryPointerUp);

            // Evento de cancelar (quando o toque sai do botão)
            EventTrigger.Entry entryPointerExit = new EventTrigger.Entry();
            entryPointerExit.eventID = EventTriggerType.PointerExit;
            entryPointerExit.callback.AddListener((data) => { PararMovimentoDireita(); });
            triggerDireita.triggers.Add(entryPointerExit);

            Debug.Log("Botão direito configurado");
        }
        else
        {
            Debug.LogWarning("Botão direito não atribuído!");
        }
    }

    private void ProcessarInputTeclado()
    {
        // Teclas para esquerda: A ou Seta Esquerda
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            movendoEsquerda = true;
        }
        else if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow))
        {
            movendoEsquerda = false;
        }

        // Teclas para direita: D ou Seta Direita
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            movendoDireita = true;
        }
        else if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            movendoDireita = false;
        }
    }

    private void ProcessarInputGamepad()
    {
        // Direcional digital do gamepad (D-Pad)
        float direcionalHorizontal = Input.GetAxis("Horizontal");

        if (direcionalHorizontal < -0.5f)
        {
            movendoEsquerda = true;
            movendoDireita = false;
        }
        else if (direcionalHorizontal > 0.5f)
        {
            movendoDireita = true;
            movendoEsquerda = false;
        }
        else
        {
            // Se não está pressionando o direcional, para o movimento
            if (Mathf.Abs(direcionalHorizontal) < 0.1f)
            {
                movendoEsquerda = false;
                movendoDireita = false;
            }
        }
    }

    private void MoverJogador()
    {
        float movimento = 0f;

        // Combina todos os inputs
        if (movendoEsquerda)
        {
            movimento -= velocidade * Time.deltaTime;
        }

        if (movendoDireita)
        {
            movimento += velocidade * Time.deltaTime;
        }

        // Aplica o movimento
        if (movimento != 0f)
        {
            Vector3 novaPosicao = transform.position + new Vector3(movimento, 0f, 0f);

            // Aplica os limites de movimento
            novaPosicao.x = Mathf.Clamp(novaPosicao.x, limiteEsquerdo, limiteDireito);

            transform.position = novaPosicao;
        }
    }

    // Métodos públicos para os botões
    public void IniciarMovimentoEsquerda()
    {
        movendoEsquerda = true;
        Debug.Log("Movimento esquerda iniciado");
    }

    public void PararMovimentoEsquerda()
    {
        movendoEsquerda = false;
        Debug.Log("Movimento esquerda parado");
    }

    public void IniciarMovimentoDireita()
    {
        movendoDireita = true;
        Debug.Log("Movimento direita iniciado");
    }

    public void PararMovimentoDireita()
    {
        movendoDireita = false;
        Debug.Log("Movimento direita parado");
    }

    // Método para alternar movimento (útil para toggle)
    public void AlternarMovimentoEsquerda()
    {
        movendoEsquerda = !movendoEsquerda;
        movendoDireita = false;
    }

    public void AlternarMovimentoDireita()
    {
        movendoDireita = !movendoDireita;
        movendoEsquerda = false;
    }

    // Métodos para definir limites dinamicamente
    public void DefinirLimites(float esquerdo, float direito)
    {
        limiteEsquerdo = esquerdo;
        limiteDireito = direito;
    }

    public void DefinirVelocidade(float novaVelocidade)
    {
        velocidade = novaVelocidade;
    }

    // Métodos para verificar estado
    public bool EstaMovendoParaEsquerda()
    {
        return movendoEsquerda;
    }

    public bool EstaMovendoParaDireita()
    {
        return movendoDireita;
    }

    public bool EstaMovendo()
    {
        return movendoEsquerda || movendoDireita;
    }

    // Gizmos para visualização no Editor
    void OnDrawGizmosSelected()
    {
        // Desenha os limites de movimento
        Gizmos.color = Color.red;
        Vector3 limiteEsq = new Vector3(limiteEsquerdo, transform.position.y, transform.position.z);
        Vector3 limiteDir = new Vector3(limiteDireito, transform.position.y, transform.position.z);

        Gizmos.DrawLine(limiteEsq + Vector3.up * 2f, limiteEsq + Vector3.down * 2f);
        Gizmos.DrawLine(limiteDir + Vector3.up * 2f, limiteDir + Vector3.down * 2f);

        Gizmos.DrawWireCube(limiteEsq, Vector3.one * 0.5f);
        Gizmos.DrawWireCube(limiteDir, Vector3.one * 0.5f);

        // Labels para os limites
#if UNITY_EDITOR
        UnityEditor.Handles.Label(limiteEsq + Vector3.up * 3f, "Limite Esquerdo");
        UnityEditor.Handles.Label(limiteDir + Vector3.up * 3f, "Limite Direito");
#endif
    }
}