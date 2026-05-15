using UnityEngine;
using UnityEngine.SceneManagement;

public class GestorPausa : MonoBehaviour
{
    [Header("Panel pausa")]
    [SerializeField] private GameObject panelPausa;

    private bool pausado = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePausa();
    }

    public void TogglePausa()
    {
        pausado = !pausado;
        panelPausa?.SetActive(pausado);
        Time.timeScale = pausado ? 0f : 1f;
    }

    public void GuardarYSalir()
    {
        Time.timeScale = 1f;
        GameManager.Instance?.GuardarPartida();
        SceneManager.LoadScene("MenuInicio");
    }
}