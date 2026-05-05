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

    private Animator anim;
    private SpriteRenderer sr;
    private bool estaAndando;
    private float direccion = 1f;
    private int contadorCaricias = 0;
    private float timerResetCaricias = 0f;
    private bool estaReaccionando = false;
    private float timerReaccion = 0f;

    private void Start()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        estaAndando = Random.value > 0.5f;
        anim.SetBool("Andando", estaAndando);

        float x = Random.Range(limiteXMin, limiteXMax);
        transform.position = new Vector3(x, posicionY, 11f);
        direccion = Random.value > 0.5f ? 1f : -1f;
    }

    private void Update()
    {
        // Reset contador caricias con el tiempo
        if (contadorCaricias > 0)
        {
            timerResetCaricias -= Time.deltaTime;
            if (timerResetCaricias <= 0)
                contadorCaricias = 0;
        }

        // Timer reaccion
        if (estaReaccionando)
        {
            timerReaccion -= Time.deltaTime;
            if (timerReaccion <= 0)
                estaReaccionando = false;
        }

        // No mover si está tumbado o reaccionando
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
}