using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class GestorClientes : MonoBehaviour
{
    public static GestorClientes Instance { get; private set; }

    [Header("Config clientes")]
    [SerializeField] private float tiempoEntreClientes = 8f;
    [SerializeField] private float pacienciaMaxima = 15f;

    [Header("Productos")]
    [SerializeField] private DatosProducto[] productos;

    [Header("UI pedido")]
    [SerializeField] private GameObject panelPedido;
    [SerializeField] private TextMeshProUGUI textoPedido;
    [SerializeField] private Slider barraPaciencia;

    [Header("Mesas")]
    [SerializeField] private SpriteRenderer mesa1Sprite;
    [SerializeField] private SpriteRenderer mesa2Sprite;
    [SerializeField] private Color colorMesaOcupada = new Color(1f, 0.8f, 0.5f, 1f);

    [Header("NPCs")]
    [SerializeField] private GameObject npc1;
    [SerializeField] private GameObject npc2;
    [SerializeField] private GameObject vineta1;
    [SerializeField] private GameObject vineta2;
    [SerializeField] private Image iconoPedido1;
    [SerializeField] private Image iconoPedido2;

    private bool clienteEsperando = false;
    private DatosProducto pedidoActual;
    private float pacienciaActual;
    private int mesaActual;
    private Color colorOriginalMesa;

    void Awake() { Instance = this; }

    void Start()
    {
        panelPedido?.SetActive(false);
        OcultarNPCs();
        if (mesa1Sprite != null) colorOriginalMesa = mesa1Sprite.color;
        StartCoroutine(SpawnClientes());
    }

    void Update()
    {
        if (!clienteEsperando) return;
        pacienciaActual -= Time.deltaTime;
        if (barraPaciencia != null)
            barraPaciencia.value = pacienciaActual / pacienciaMaxima;
        if (pacienciaActual <= 0) ClienteSeFue();
    }

    private IEnumerator SpawnClientes()
    {
        yield return new WaitForSeconds(2f);
        NuevoCliente();
        while (true)
        {
            yield return new WaitForSeconds(tiempoEntreClientes);
            if (!clienteEsperando) NuevoCliente();
        }
    }

    private void NuevoCliente()
    {
        if (productos == null || productos.Length == 0) return;

        pedidoActual = productos[Random.Range(0, productos.Length)];
        pacienciaActual = pacienciaMaxima;
        clienteEsperando = true;
        mesaActual = Random.Range(1, 3);

        ResaltarMesa(mesaActual, true);
        MostrarNPC(mesaActual);

        panelPedido?.SetActive(true);
        if (textoPedido != null)
            textoPedido.text = "Mesa " + mesaActual + ": " + pedidoActual.nombreProducto;

        GestorDialogos.Instance?.MostrarDialogo(
            "¡Nuevo cliente en la mesa " + mesaActual + "! Quiere un " + pedidoActual.nombreProducto);
    }

    // Llamado por GestorProductos cuando el jugador selecciona un producto
    public void NotificarProductoPreparado(DatosProducto producto)
    {
        if (!clienteEsperando) return;

        if (producto.origen != pedidoActual.origen)
        {
            GestorDialogos.Instance?.MostrarDialogo("¡Ese producto no se prepara aquí!");
            return;
        }

        GestorDialogos.Instance?.MostrarDialogo(
            "Preparando " + producto.nombreProducto + "... ¡Ahora llévalo a la mesa " + mesaActual + "!");
    }

    public void ServirMesa1() { IntentarServir(1); }
    public void ServirMesa2() { IntentarServir(2); }

    private void IntentarServir(int mesa)
    {
        if (!clienteEsperando) return;

        DatosProducto preparado = GestorProductos.Instance?.ObtenerProductoPreparado();

        if (preparado == null)
        {
            GestorDialogos.Instance?.MostrarDialogo("¡Primero prepara el pedido en la cafetera o cocina!");
            return;
        }

        if (preparado != pedidoActual)
        {
            GestorDialogos.Instance?.MostrarDialogo(
                "Eso no es lo que pidió... Quiere un " + pedidoActual.nombreProducto);
            return;
        }

        if (mesa != mesaActual)
        {
            GestorDialogos.Instance?.MostrarDialogo(
                "Mesa equivocada. El cliente está en la mesa " + mesaActual);
            return;
        }

        // Servir correctamente
        clienteEsperando = false;
        panelPedido?.SetActive(false);
        ResaltarMesa(mesaActual, false);
        OcultarNPCs();
        GestorProductos.Instance?.LimpiarProductoPreparado();
        GameManager.Instance?.AnadirDinero(pedidoActual.precio);

        GestorDialogos.Instance?.MostrarDialogo(new string[] {
            "¡Aquí tiene su " + pedidoActual.nombreProducto + "!",
            "+" + pedidoActual.precio + " monedas. ¡Buen trabajo!"
        });
    }

    private void ClienteSeFue()
    {
        clienteEsperando = false;
        panelPedido?.SetActive(false);
        ResaltarMesa(mesaActual, false);
        OcultarNPCs();

        int penalizacion = pedidoActual != null ? pedidoActual.precio / 2 : 5;
        GameManager.Instance?.GastarDinero(penalizacion);

        GestorDialogos.Instance?.MostrarDialogo(new string[] {
            "El cliente se fue enfadado...",
            "-" + penalizacion + " monedas de penalización."
        });
    }

    private void MostrarNPC(int mesa)
    {
        if (mesa == 1)
        {
            if (npc1 != null) npc1.SetActive(true);
            if (vineta1 != null) vineta1.SetActive(true);
            if (iconoPedido1 != null && pedidoActual.iconoProducto != null)
                iconoPedido1.sprite = pedidoActual.iconoProducto;
        }
        else
        {
            if (npc2 != null) npc2.SetActive(true);
            if (vineta2 != null) vineta2.SetActive(true);
            if (iconoPedido2 != null && pedidoActual.iconoProducto != null)
                iconoPedido2.sprite = pedidoActual.iconoProducto;
        }
    }

    private void OcultarNPCs()
    {
        if (npc1 != null) npc1.SetActive(false);
        if (npc2 != null) npc2.SetActive(false);
        if (vineta1 != null) vineta1.SetActive(false);
        if (vineta2 != null) vineta2.SetActive(false);
    }

    private void ResaltarMesa(int mesa, bool resaltar)
    {
        SpriteRenderer sr = (mesa == 1) ? mesa1Sprite : mesa2Sprite;
        if (sr != null) sr.color = resaltar ? colorMesaOcupada : colorOriginalMesa;
    }

    public void ModificarPaciencia(float multiplicador) { pacienciaMaxima *= multiplicador; }
    public void ModificarRitmoAparicion(float multiplicador) { tiempoEntreClientes /= multiplicador; }
}