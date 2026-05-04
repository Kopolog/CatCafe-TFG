using UnityEngine;

public class RatonMinijuego : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private float tiempoVisible = 1.5f;
    [SerializeField] private float velocidad = 3f;
    [SerializeField] private float tiempoCambioDir = 0.8f;

    [Header("Límites de movimiento")]
    [SerializeField] private float limiteX = 7f;
    [SerializeField] private float limiteY = 3.5f;

    private Animator animador;
    private bool atrapado = false;
    private float timerVisible;
    private float timerCambioDir;
    private Vector2 direccion;
    private SpriteRenderer sr;

    void Awake()
    {
        animador = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        atrapado = false;
        timerVisible = tiempoVisible;
        timerCambioDir = 0f;
        CambiarDireccion();
        animador?.Play("RatonIdle");
    }

    void Update()
    {
        if (atrapado) return;

        timerVisible -= Time.deltaTime;
        if (timerVisible <= 0)
        {
            Esconderse();
            return;
        }

        timerCambioDir -= Time.deltaTime;
        if (timerCambioDir <= 0)
            CambiarDireccion();

        transform.Translate(direccion * velocidad * Time.deltaTime);

        Vector3 pos = transform.position;
        if (pos.x > limiteX || pos.x < -limiteX)
        {
            direccion.x = -direccion.x;
            pos.x = Mathf.Clamp(pos.x, -limiteX, limiteX);
        }
        if (pos.y > limiteY || pos.y < -limiteY)
        {
            direccion.y = -direccion.y;
            pos.y = Mathf.Clamp(pos.y, -limiteY, limiteY);
        }
        transform.position = pos;

        if (sr != null)
            sr.flipX = direccion.x < 0;
    }

    void OnMouseDown()
    {
        if (atrapado) return;
        Atrapar();
    }

    private void CambiarDireccion()
    {
        float angulo = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        direccion = new Vector2(Mathf.Cos(angulo), Mathf.Sin(angulo));
        timerCambioDir = tiempoCambioDir;
    }

    private void Atrapar()
    {
        atrapado = true;
        animador?.Play("ratonMuere");
        GestorCazaRatones.Instance.RatonAtrapado();
        Invoke(nameof(Esconderse), 0.5f);
    }

    private void Esconderse()
    {
        gameObject.SetActive(false);
    }
}