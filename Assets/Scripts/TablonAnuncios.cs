using UnityEngine;

public class TablonAnuncios : MonoBehaviour
{
    [Header("Referencias UI")]
    [SerializeField] private GameObject panelTablon;

    private bool _panelAbierto = false;

    private void Update()
    {
        // Cerrar con Escape
        if (_panelAbierto && Input.GetKeyDown(KeyCode.Escape))
            CerrarPanel();
    }

    public void AlHacerClic()
    {
        if (_panelAbierto)
            CerrarPanel();
        else
            AbrirPanel();
    }

    public void AbrirPanel()
    {
        panelTablon.SetActive(true);
        _panelAbierto = true;
        Time.timeScale = 0f;
    }

    public void CerrarPanel()
    {
        panelTablon.SetActive(false);
        _panelAbierto = false;
        Time.timeScale = 1f;
    }
}