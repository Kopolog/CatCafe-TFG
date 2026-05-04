using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int Dinero { get; private set; } = 0;
    public int ClientesServidos { get; private set; } = 0;

    private float multiplicadorGanancias = 1f;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AnadirDinero(int cantidad)
    {
        int cantidadFinal = Mathf.RoundToInt(cantidad * multiplicadorGanancias);
        Dinero += cantidadFinal;
        GestorUI.Instance?.ActualizarDinero(Dinero);
    }


    public bool TieneSuficiente(int cantidad)
    {
        return Dinero >= cantidad;
    }

    public void GastarDinero(int cantidad)
    {
        Dinero -= cantidad;
        GestorUI.Instance?.ActualizarDinero(Dinero);
    }

    public void ModificarMultiplicador(float multiplicador)
    {
        multiplicadorGanancias *= multiplicador;
    }


    public void CargarEscena(string nombreEscena)
    {
        SceneManager.LoadScene(nombreEscena);
    }

    public void GuardarPartida()
    {
        PlayerPrefs.SetInt("Dinero", Dinero);
        PlayerPrefs.SetFloat("Multiplicador", multiplicadorGanancias);
        PlayerPrefs.Save();
    }

    public void CargarPartida()
    {
        Dinero = PlayerPrefs.GetInt("Dinero", 0);
        multiplicadorGanancias = PlayerPrefs.GetFloat("Multiplicador", 1f);
        GestorUI.Instance?.ActualizarDinero(Dinero);
    }
    public void ReiniciarPartida()
    {
        
        Dinero = 0;
        ClientesServidos = 0;
        multiplicadorGanancias = 1f;
        PlayerPrefs.DeleteAll();

     
        GestorGatos.Instance?.ReiniciarGatos();

        CargarEscena("MenuPrincipal");
    }
}