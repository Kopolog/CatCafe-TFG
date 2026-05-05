using UnityEngine;

public class GestorMinijuegos : MonoBehaviour
{
    public static GestorMinijuegos Instance { get; private set; }

    public DatosGato GatoActual { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void LanzarMinijuego(DatosGato gato)
    {
        GatoActual = gato;
        Debug.Log($"Lanzando minijuego para {gato.nombreGato} — Dificultad {gato.dificultad}");
        GameManager.Instance.CargarEscena("MinijuegoCazaRatones");
    }

    public void MinijuegoGanado()
    {
        if (GatoActual != null)
        {
            Debug.Log("MinijuegoGanado llamado, gato: " + GatoActual.nombreGato);
            Debug.Log("GestorGatos instance: " + GestorGatos.Instance);
            GestorGatos.Instance.SalvarGato(GatoActual);
            GatoActual = null;
        }
        else
        {
            Debug.Log("GatoActual es null");
        }
    }

    public void MinijuegoPerdido()
    {
        GestorDialogos.Instance?.MostrarDialogo(
            $"{GatoActual?.nombreGato ?? "El gato"} ha escapado... ¡Inténtalo de nuevo!"
        );
        GatoActual = null;
    }
}