using UnityEngine;

public class GatoSalvado : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float velocidad = 1.5f;
    [SerializeField] private float tiempoEspera = 2f;

    [Header("Límites de la cafetería")]
    [SerializeField] private float limiteXMin = -7f;
    [SerializeField] private float limiteXMax = 7f;
    [SerializeField] private float posicionY = -1f;

    [Header("Datos del gato")]
    public DatosGato datosGato;

    private SpriteRenderer sr;
    private float direccion = 1f;
    private float timerEspera;
    private bool esperando = false;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        float x = Random.Range(limiteXMin, limiteXMax);
        transform.position = new Vector3(x, posicionY, 0f);


        direccion = Random.value > 0.5f ? 1f : -1f;
    }

    private void Update()
    {
        if (esperando)
        {
            timerEspera -= Time.deltaTime;
            if (timerEspera <= 0)
                esperando = false;
            return;
        }

        transform.Translate(Vector2.right * direccion * velocidad * Time.deltaTime);

        if (sr != null)
            sr.flipX = direccion < 0;


        Vector3 pos = transform.position;
        if (pos.x >= limiteXMax || pos.x <= limiteXMin)
        {
            direccion = -direccion;
            pos.x = Mathf.Clamp(pos.x, limiteXMin, limiteXMax);
            transform.position = pos;

            esperando = true;
            timerEspera = tiempoEspera;
        }
    }
    [Header("Prefab gato salvado")]
    [SerializeField] private GameObject prefabGatoSalvado;

}