using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void Jugar()
    {
        if (!GameManager.Instance.HayPartidaGuardada)
            GameManager.Instance?.ReiniciarPartida();

        SceneManager.LoadScene("CafeScene");
    }

    public void Opciones()
    {
        Debug.Log("Opciones pendiente de implementar");
    }

    public void Salir()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}