using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void Iniciar()
    {
        SceneManager.LoadScene("Estacao");
    }

    public void Creditos()
    {
        SceneManager.LoadScene("Creditos");
    }

    public void Opcoes()
    {
        SceneManager.LoadScene("Opcoes");
    }

    public void Voltar()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Sair()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
