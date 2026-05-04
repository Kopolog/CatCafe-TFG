using UnityEngine;
using TMPro;
using System.Collections;

public class GestorDialogos : MonoBehaviour
{
    public static GestorDialogos Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private GameObject panelDialogo;
    [SerializeField] private TextMeshProUGUI textoDialogo;
    [SerializeField] private float velocidadTexto = 0.03f;

    private string[] lineasActuales;
    private int lineaActual;
    private bool escribiendo = false;
    private bool esperandoInput = false;
    private Coroutine coroutinaTexto;

    void Awake() { Instance = this; }

    void Start() { panelDialogo?.SetActive(false); }

    public void MostrarDialogo(string texto)
    {
        MostrarDialogo(new string[] { texto });
    }

    public void MostrarDialogo(string[] lineas)
    {
    
        if (coroutinaTexto != null)
            StopCoroutine(coroutinaTexto);

        lineasActuales = lineas;
        lineaActual = 0;
        panelDialogo?.SetActive(true);
        esperandoInput = false;
        MostrarLineaActual();
    }

    void Update()
    {
        if (panelDialogo == null || !panelDialogo.activeSelf) return;
        if (!esperandoInput) return;

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            if (escribiendo)
            {
               
                StopCoroutine(coroutinaTexto);
                textoDialogo.text = lineasActuales[lineaActual];
                escribiendo = false;
            }
            else
            {
               
                lineaActual++;
                if (lineaActual < lineasActuales.Length)
                    MostrarLineaActual();
                else
                    CerrarDialogo();
            }
        }
    }

    private void MostrarLineaActual()
    {
        coroutinaTexto = StartCoroutine(EscribirTexto(lineasActuales[lineaActual]));
    }

    private IEnumerator EscribirTexto(string texto)
    {
        escribiendo = true;
        esperandoInput = false;
        textoDialogo.text = "";

        yield return null;
        yield return null;
        esperandoInput = true;

        foreach (char c in texto)
        {
            textoDialogo.text += c;
            yield return new WaitForSeconds(velocidadTexto);
        }
        escribiendo = false;
    }

    private void CerrarDialogo()
    {
        panelDialogo?.SetActive(false);
        esperandoInput = false;
    }

    public bool EstaActivo => panelDialogo != null && panelDialogo.activeSelf;
}