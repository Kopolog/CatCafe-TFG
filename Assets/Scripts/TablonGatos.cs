using UnityEngine;

public class TablonGatos : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private GameObject panelTablon;
    [SerializeField] private UITablonGatos uiTablon;

    private bool _panelAbierto = false;

    private void Update()
    {
        if (_panelAbierto && Input.GetKeyDown(KeyCode.Escape))
            CerrarPanel();
    }

    public void AlHacerClic()
    {
        if (_panelAbierto) CerrarPanel();
        else AbrirPanel();
    }

    public void AbrirPanel()
    {
        GestorGatos.Instance.GenerarGatosTablon();
        uiTablon.RefrescarPanel();
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