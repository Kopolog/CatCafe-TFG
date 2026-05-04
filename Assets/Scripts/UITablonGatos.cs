using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UITablonGatos : MonoBehaviour
{
    [Header("Prefab ficha de gato y contenedor")]
    [SerializeField] private GameObject prefabFichaGato;
    [SerializeField] private Transform contenedorFichas;

    [Header("Botón cerrar")]
    [SerializeField] private Button botonCerrar;
    [SerializeField] private TablonGatos tablonGatos;

    private void Start()
    {
        botonCerrar?.onClick.AddListener(tablonGatos.CerrarPanel);
    }

    public void RefrescarPanel()
    {
        foreach (Transform hijo in contenedorFichas)
            Destroy(hijo.gameObject);

        List<DatosGato> gatos = GestorGatos.Instance.GatosEnTablon;

        if (gatos.Count == 0)
        {
            GameObject ficha = Instantiate(prefabFichaGato, contenedorFichas);
            ficha.transform.Find("ImagenGato")?.gameObject.SetActive(false);
            ficha.transform.Find("TextoNombre")?.GetComponent<TextMeshProUGUI>()
                ?.SetText("¡No hay gatos perdidos ahora mismo!");
            ficha.transform.Find("BotonSalvar")?.gameObject.SetActive(false);
            return;
        }

        foreach (DatosGato gato in gatos)
            CrearFicha(gato);
    }

    private void CrearFicha(DatosGato gato)
    {
        GameObject ficha = Instantiate(prefabFichaGato, contenedorFichas);

        ficha.transform.Find("TextoNombre")?.GetComponent<TextMeshProUGUI>()
            ?.SetText(gato.nombreGato);

        if (gato.fotoGato != null)
            ficha.transform.Find("ImagenGato")?.GetComponent<Image>()
                ?.SetSprite(gato.fotoGato);

        ficha.transform.Find("TextoDescripcion")?.GetComponent<TextMeshProUGUI>()
            ?.SetText(gato.descripcion);

        string dificultadTexto = gato.dificultad switch
        {
            1 => "Dificultad: Facil",
            2 => "Dificultad: Intermedia",
            3 => "Dificultad: Dificil",
            _ => ""
        };
        ficha.transform.Find("TextoDificultad")?.GetComponent<TextMeshProUGUI>()
            ?.SetText(dificultadTexto);

        ficha.transform.Find("TextoBonus")?.GetComponent<TextMeshProUGUI>()
            ?.SetText($"Bonus: {gato.descripcionBonus}");

        ficha.transform.Find("TextoGasto")?.GetComponent<TextMeshProUGUI>()
            ?.SetText($"Coste: {gato.gastoPorDia} monedas/día");

        Button boton = ficha.transform.Find("BotonSalvar")?.GetComponent<Button>();
        if (boton != null)
        {
            boton.onClick.AddListener(() => IniciarRescate(gato));
        }
    }

    private void IniciarRescate(DatosGato gato)
    {
        tablonGatos.CerrarPanel();
        GestorMinijuegos.Instance.LanzarMinijuego(gato);
    }
}

public static class ExtImagen
{
    public static void SetSprite(this Image img, Sprite sprite)
    {
        if (sprite != null) img.sprite = sprite;
    }
}