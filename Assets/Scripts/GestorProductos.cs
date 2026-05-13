using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class GestorProductos : MonoBehaviour
{
    public static GestorProductos Instance { get; private set; }

    [Header("Productos")]
    [SerializeField] private DatosProducto[] todosLosProductos;

    [Header("Paneles")]
    [SerializeField] private GameObject panelCafetera;
    [SerializeField] private GameObject panelCocina;
    [SerializeField] private Transform contenedorCafetera;
    [SerializeField] private Transform contenedorCocina;

    [Header("Prefab")]
    [SerializeField] private GameObject prefabBotonProducto;

    private Dictionary<DatosProducto, Coroutine> cooldownsActivos = new();
    private List<DatosProducto> productosListos = new List<DatosProducto>();

    void Awake() { Instance = this; }

    void Start()
    {
        GenerarBotones(contenedorCafetera, OrigenProducto.Cafetera);
        GenerarBotones(contenedorCocina, OrigenProducto.Cocina);
        panelCafetera?.SetActive(false);
        panelCocina?.SetActive(false);
    }

    private void GenerarBotones(Transform contenedor, OrigenProducto origen)
    {
        if (contenedor == null || prefabBotonProducto == null) return;

        foreach (DatosProducto p in todosLosProductos)
        {
            if (p.origen != origen) continue;

            GameObject btn = Instantiate(prefabBotonProducto, contenedor);
            btn.transform.Find("nombre")?.GetComponent<TextMeshProUGUI>()?.SetText(p.nombreProducto);
            btn.transform.Find("precio")?.GetComponent<TextMeshProUGUI>()?.SetText(p.precio + " monedas");
            Image icono = btn.transform.Find("icono")?.GetComponent<Image>();
            if (icono != null && p.iconoProducto != null) icono.sprite = p.iconoProducto;

            Slider barra = btn.transform.Find("barraCooldown")?.GetComponent<Slider>();
            Button boton = btn.GetComponent<Button>();

            if (boton != null)
            {
                DatosProducto captura = p;
                boton.onClick.AddListener(() => SeleccionarProducto(captura, boton, barra));
            }
        }
    }

    public void AbrirCafetera()
    {
        panelCafetera?.SetActive(true);
        panelCocina?.SetActive(false);
    }

    public void AbrirCocina()
    {
        panelCocina?.SetActive(true);
        panelCafetera?.SetActive(false);
    }

    public void CerrarPaneles()
    {
        panelCafetera?.SetActive(false);
        panelCocina?.SetActive(false);
    }

    private void SeleccionarProducto(DatosProducto producto, Button boton, Slider barra)
    {
        if (cooldownsActivos.ContainsKey(producto))
        {
            GestorDialogos.Instance?.MostrarDialogo("¡Ese producto ya se está preparando!");
            return;
        }

        CerrarPaneles();
        GestorClientes.Instance?.NotificarProductoPreparado(producto);
        Coroutine c = StartCoroutine(EjecutarCooldown(producto, boton, barra));
        cooldownsActivos[producto] = c;
    }

    private IEnumerator EjecutarCooldown(DatosProducto producto, Button boton, Slider barra)
    {
        if (boton != null) boton.interactable = false;
        if (barra != null) barra.value = 0f;

        float elapsed = 0f;
        while (elapsed < producto.cooldown)
        {
            elapsed += Time.deltaTime;
            if (barra != null) barra.value = elapsed / producto.cooldown;
            yield return null;
        }

        productosListos.Add(producto);
        cooldownsActivos.Remove(producto);
        if (boton != null) boton.interactable = true;
        if (barra != null) barra.value = 0f;

        GestorDialogos.Instance?.MostrarDialogo("¡" + producto.nombreProducto + " listo! Llévalo a la mesa.");
    }

    public DatosProducto ObtenerProductoPreparado()
    {
        return productosListos.Count > 0 ? productosListos[0] : null;
    }

    public void LimpiarProductoPreparado()
    {
        if (productosListos.Count > 0)
            productosListos.RemoveAt(0);
    }
}