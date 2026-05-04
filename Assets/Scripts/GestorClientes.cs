using UnityEngine;
using TMPro;
using System.Collections;

public class GestorClientes : MonoBehaviour
{
    public static GestorClientes Instance { get; private set; }

    [Header("Config")]
    [SerializeField] private float tiempoEntreClientes = 8f;
    [SerializeField] private float pacienciaMaxima = 15f;

    [Header("Productos")]
    [SerializeField] private string[] menu = { "Cafe", "Latte", "Capuccino", "Te", "Zumo" };
    [SerializeField] private int[] precios = { 5, 8, 10, 4, 6 };

    [Header("UI")]
    [SerializeField] private GameObject panelPedido;
    [SerializeField] private TextMeshProUGUI textoPedido;
    [SerializeField] private UnityEngine.UI.Slider barraPaciencia;

    [Header("Mesas")]
    [SerializeField] private SpriteRenderer mesa1Sprite;
    [SerializeField] private SpriteRenderer mesa2Sprite;
    [SerializeField] private Color colorMesaOcupada = new Color(1f, 0.8f, 0.5f, 1f);

    private bool clienteEsperando = false;
    private int pedidoActual;
    private float pacienciaActual;
    private int mesaActual; 
    private Color colorOriginalMesa;

    void Awake() { Instance = this; }

    void Start()
    {
        panelPedido?.SetActive(false);
        if (mesa1Sprite != null)
            colorOriginalMesa = mesa1Sprite.color;
        StartCoroutine(SpawnClientes());
    }

    void Update()
    {
        if (!clienteEsperando) return;

        pacienciaActual -= Time.deltaTime;

        if (barraPaciencia != null)
            barraPaciencia.value = pacienciaActual / pacienciaMaxima;

        if (pacienciaActual <= 0)
        {
            ClienteSeFue();
        }
    }

    private IEnumerator SpawnClientes()
    {
        yield return new WaitForSeconds(2f);
        NuevoCliente();

        while (true)
        {
            yield return new WaitForSeconds(tiempoEntreClientes);

            if (!clienteEsperando)
            {
                NuevoCliente();
            }
        }
    }

    private void NuevoCliente()
    {
        pedidoActual = Random.Range(0, menu.Length);
        pacienciaActual = pacienciaMaxima;
        clienteEsperando = true;

        mesaActual = Random.Range(1, 3);

        ResaltarMesa(mesaActual, true);

        panelPedido?.SetActive(true);
        if (textoPedido != null)
            textoPedido.text = "Mesa " + mesaActual + ": Quiero un " + menu[pedidoActual];

        GestorDialogos.Instance?.MostrarDialogo(
            "Nuevo cliente en la mesa " + mesaActual + "!"
        );
    }

    public void PrepararPedido()
    {
        if (!clienteEsperando) return;

        GestorDialogos.Instance?.MostrarDialogo(
            "Preparando " + menu[pedidoActual] + "... Ahora llevalo a la mesa " + mesaActual + "!"
        );
    }

    public void ServirMesa1()
    {
        IntentarServir(1);
    }

    public void ServirMesa2()
    {
        IntentarServir(2);
    }

    private void IntentarServir(int mesa)
    {
        if (!clienteEsperando) return;

        if (mesa == mesaActual)
        {
            clienteEsperando = false;
            panelPedido?.SetActive(false);
            ResaltarMesa(mesaActual, false);

            int dineroGanado = precios[pedidoActual];
            GameManager.Instance?.AnadirDinero(dineroGanado);

            GestorDialogos.Instance?.MostrarDialogo(
                new string[] {
                    "Aqui tiene su " + menu[pedidoActual] + "!",
                    "+" + dineroGanado + " monedas. Buen trabajo!"
                }
            );
        }
        else
        {
            GestorDialogos.Instance?.MostrarDialogo(
                "Esa no es la mesa correcta! El cliente esta en la mesa " + mesaActual
            );
        }
    }

    private void ClienteSeFue()
    {
        clienteEsperando = false;
        panelPedido?.SetActive(false);
        ResaltarMesa(mesaActual, false);

        GestorDialogos.Instance?.MostrarDialogo(
            "El cliente se fue enfadado..."
        );
    }

    private void ResaltarMesa(int mesa, bool resaltar)
    {
        SpriteRenderer sr = (mesa == 1) ? mesa1Sprite : mesa2Sprite;
        if (sr != null)
            sr.color = resaltar ? colorMesaOcupada : colorOriginalMesa;
    }

    public void ModificarPaciencia(float multiplicador)
    {
        pacienciaMaxima *= multiplicador;
    }

    public void ModificarRitmoAparicion(float multiplicador)
    {
        tiempoEntreClientes /= multiplicador; 
    }
}