using UnityEngine;

public class CorazonFlotante : MonoBehaviour
{
    [SerializeField] private float velocidadSubida = 1f;
    [SerializeField] private float duracion = 1f;

    private float timer = 0f;
    private SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        transform.position += Vector3.up * velocidadSubida * Time.deltaTime;

        float alpha = 1f - (timer / duracion);
        if (sr != null)
        {
            Color c = sr.color;
            c.a = alpha;
            sr.color = c;
        }

        if (timer >= duracion)
            Destroy(gameObject);
    }
}