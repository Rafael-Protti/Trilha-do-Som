using UnityEngine;
using UnityEngine.UI;

public class DesativarBotaoMobile : MonoBehaviour
{
    [Header("Configurações")]
    public bool desativarEmNaoMobile = true;

    private Button botao;
    private CanvasGroup canvasGroup;

    void Start()
    {
        botao = GetComponent<Button>();
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        VerificarEAtualizar();
    }

    private void VerificarEAtualizar()
    {
        bool emMobile = EstaEmPlataformaMobile();
        bool deveDesativar = desativarEmNaoMobile && !emMobile;

        // Desativa visualmente e funcionalmente
        if (canvasGroup != null)
        {
            canvasGroup.alpha = deveDesativar ? 0f : 1f;
            canvasGroup.interactable = !deveDesativar;
            canvasGroup.blocksRaycasts = !deveDesativar;
        }

        // Desativa o componente Button também
        if (botao != null)
        {
            botao.enabled = !deveDesativar;
        }

        // Log informativo
        if (deveDesativar)
        {
            Debug.Log($"Botão {gameObject.name} desativado (não-mobile)");
        }
    }

    private bool EstaEmPlataformaMobile()
    {
#if UNITY_ANDROID || UNITY_IOS
        return true;
#else
        return false;
#endif
    }

    // Método público para verificar status
    public bool BotaoEstaAtivo()
    {
        if (canvasGroup != null)
        {
            return canvasGroup.alpha > 0.5f && canvasGroup.interactable;
        }
        return gameObject.activeInHierarchy;
    }
}