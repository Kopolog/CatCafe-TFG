using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PanelGestorGatos : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject panelGatos;
    [SerializeField] private GameObject prefabFichaGato;
    [SerializeField] private Transform contenedor;
    [SerializeField] private TextMeshProUGUI textoBoton;

    [Header("Prefabs de gatos")]
    [SerializeField] private List<GameObject> prefabsGatos;

    private bool panelAbierto = false;
    private DatosGato gatoActivo = null;
    private GameObject gatoEnEscena = null;

    private void Start()
    {
        panelGatos.SetActive(false);
    }

    public void TogglePanel()
    {
        panelAbierto = !panelAbierto;
        panelGatos.SetActive(panelAbierto);
        textoBoton.text = panelAbierto ? "▲" : "▼";

        if (panelAbierto)
            RefrescarPanel();
    }

    private void RefrescarPanel()
    {
        foreach (Transform hijo in contenedor)
            Destroy(hijo.gameObject);

        List<DatosGato> salvados = GestorGatos.Instance.ObtenerGatosSalvados();

        foreach (DatosGato gato in salvados)
            CrearFicha(gato);
    }

    private void CrearFicha(DatosGato gato)
    {
        GameObject ficha = Instantiate(prefabFichaGato, contenedor);

        ficha.transform.Find("ImagenGato")?.GetComponent<Image>()
            ?.SetSprite(gato.fotoGato);

        ficha.transform.Find("nombreGato")?.GetComponent<TextMeshProUGUI>()
            ?.SetText(gato.nombreGato);

        ficha.transform.Find("descripcion")?.GetComponent<TextMeshProUGUI>()
            ?.SetText(gato.descripcion);

        ficha.transform.Find("textoBonus")?.GetComponent<TextMeshProUGUI>()
            ?.SetText(gato.descripcionBonus);

        Toggle toggle = ficha.transform.Find("activargato")?.GetComponent<Toggle>();
        if (toggle != null)
        {
            toggle.isOn = gatoActivo == gato;
            toggle.onValueChanged.AddListener((activo) =>
            {
                if (activo) ActivarGato(gato);
            });
        }
    }

    private void ActivarGato(DatosGato gato)
    {
        gatoActivo = gato;

        if (gatoEnEscena != null)
            Destroy(gatoEnEscena);

        GameObject prefabElegido = null;
        foreach (GameObject prefab in prefabsGatos)
        {
            GatoSalvado gs = prefab.GetComponent<GatoSalvado>();
            if (gs != null && gs.datosGato == gato)
            {
                prefabElegido = prefab;
                break;
            }
        }

        if (prefabElegido != null)
        {
            gatoEnEscena = Instantiate(prefabElegido);
            GatoSalvado gsInstanciado = gatoEnEscena.GetComponent<GatoSalvado>();
            if (gsInstanciado != null)
                GameManager.Instance?.RegistrarGatoActivo(gsInstanciado);
        }

        RefrescarPanel();
    }
}