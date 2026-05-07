using UnityEngine;

public class GatoSalvado : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float velocidad = 0.3f;

    [Header("Límites de la cafetería")]
    [SerializeField] private float limiteXMin = -7f;
    [SerializeField] private float limiteXMax = 7f;
    [SerializeField] private float posicionY = -1f;

    [Header("Datos del gato")]
    public DatosGato datosGato;

    [Header("Caricias")]
    [SerializeField] private int cariciasParaEnfadar = 5;
    [SerializeField] private float tiempoResetCaricias = 3f;
    [SerializeField] private float duracionReaccion = 1.5f;

    [Header("Barra felicidad")]
    [SerializeField] private GameObject prefabBarraFelicidad;
    [SerializeField] private float felicidadMaxima = 1f;
    [SerializeField] private float velocidadBajarFelicidad = 0.05f;

    [Header("Corazon flotante")]
    [SerializeField] private GameObject prefabCorazon;

    private Animator anim;
    private SpriteRenderer sr;
    private bool estaAndando;
    private float direccion = 1f;
    private int contadorCaricias = 0;
    private float timerResetCaricias = 0f;
    private bool estaReaccionando = false;
    private float timerReaccion = 0f;
    private float felicidadActual = 0f;
    private float timerOcultarBarra = 0f;
    private float tiempoOcultarBarra = 3f;
    private UnityEngine.UI.Slider barraFelicidad;

    private void Start()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        if (prefabBarraFelicidad != null)
        {
            GameObject barra = Instantiate(prefabBarraFelicidad);
            barraFelicidad = barra.GetComponent<UnityEngine.UI.Slider>();
            barraFelicidad.gameObject.SetActive(false);
        }

        estaAndando = Random.value > 0.5f;
        anim.SetBool("Andando", estaAndando);

        float x = Random.Range(limiteXMin, limiteXMax);
        transform.position = new Vector3(x, posicionY, 11f);
        direccion = Random.value > 0.5f ? 1f : -1f;
        GameManager.Instance?.RegistrarGatoActivo(this);
    }

    private void Update()
    {
        if (contadorCaricias > 0)
        {
            timerResetCaricias -= Time.deltaTime;
            if (timerResetCaricias <= 0)
                contadorCaricias = 0;
        }

        if (estaReaccionando)
        {
            timerReaccion -= Time.deltaTime;
            if (timerReaccion <= 0)
                estaReaccionando = false;
        }

        if (felicidadActual > 0)
        {
            felicidadActual -= velocidadBajarFelicidad * Time.deltaTime;
            felicidadActual = Mathf.Clamp(felicidadActual, 0f, felicidadMaxima);
            if (barraFelicidad != null)
                barraFelicidad.value = felicidadActual / felicidadMaxima;
        }

        if (timerOcultarBarra > 0)
        {
            timerOcultarBarra -= Time.deltaTime;
            if (timerOcultarBarra <= 0 && barraFelicidad != null)
                barraFelicidad.gameObject.SetActive(false);
        }

        if (barraFelicidad != null && barraFelicidad.gameObject.activeSelf)
            barraFelicidad.transform.position = transform.position + new Vector3(0, 0.5f, 0);

        if (!estaAndando || estaReaccionando) return;

        transform.Translate(Vector2.right * direccion * velocidad * Time.deltaTime);

        if (sr != null)
            sr.flipX = direccion > 0;

        Vector3 pos = transform.position;
        if (pos.x >= limiteXMax || pos.x <= limiteXMin)
        {
            direccion = -direccion;
            pos.x = Mathf.Clamp(pos.x, limiteXMin, limiteXMax);
            transform.position = pos;
        }
    }

    private void OnMouseDown()
    {
        felicidadActual = Mathf.Clamp(felicidadActual + 0.2f, 0f, felicidadMaxima);
        timerOcultarBarra = tiempoOcultarBarra;

        if (barraFelicidad != null)
            barraFelicidad.gameObject.SetActive(true);

        if (prefabCorazon != null)
        {
            Vector3 posCorazon = new Vector3(
                transform.position.x + Random.Range(-0.3f, 0.3f),
                transform.position.y + 1.5f,
                0f);
            GameObject corazon = Instantiate(prefabCorazon);
            corazon.transform.position = posCorazon;
        }

        contadorCaricias++;
        timerResetCaricias = tiempoResetCaricias;
        estaReaccionando = true;
        timerReaccion = duracionReaccion;

        if (contadorCaricias >= cariciasParaEnfadar)
        {
            contadorCaricias = 0;
            anim.SetTrigger("Enfadado");
        }
        else
        {
            anim.SetTrigger("Contento");
        }
    }
    public float ObtenerMultiplicadorBonus()
    {
        return felicidadActual / felicidadMaxima;
    }
}