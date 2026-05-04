using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIMinijuego : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI textoTimer;
    [SerializeField] private TextMeshProUGUI textoContador;
    [SerializeField] private TextMeshProUGUI textoMensaje;
    [SerializeField] private Button botonReintentar;
    [SerializeField] private Button botonVolver;

    private void Start()
    {
        textoMensaje?.gameObject.SetActive(false);
        botonReintentar?.gameObject.SetActive(false);
        botonVolver?.gameObject.SetActive(false);

        botonReintentar?.onClick.AddListener(Reintentar);
        botonVolver?.onClick.AddListener(Volver);
    }

    public void ActualizarTimer(float segundos)
    {
        if (textoTimer != null)
            textoTimer.text = $"{Mathf.CeilToInt(segundos)}s";
    }

    public void ActualizarContador(int atrapados, int necesarios)
    {
        if (textoContador != null)
            textoContador.text = $"{atrapados} / {necesarios}";
    }

    public void MostrarMensajeGanado(string mensaje)
    {
        if (textoMensaje != null)
        {
            textoMensaje.text = mensaje;
            textoMensaje.gameObject.SetActive(true);
        }
        botonVolver?.gameObject.SetActive(true);
        botonReintentar?.gameObject.SetActive(false);
    }

    public void MostrarMensajePerdido(string mensaje)
    {
        if (textoMensaje != null)
        {
            textoMensaje.text = mensaje;
            textoMensaje.gameObject.SetActive(true);
        }
        botonReintentar?.gameObject.SetActive(true);
        botonVolver?.gameObject.SetActive(true);
    }

    private void Reintentar()
    {
        GameManager.Instance?.CargarEscena("MinijuegoCazaRatones");
    }

    private void Volver()
    {
        GameManager.Instance?.CargarEscena("CafeScene");
    }
}