using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class ZonaClicable : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private string nombreZona;
    [SerializeField] private Color colorResaltado = new Color(1, 1, 1, 0.5f);

    [Header("Eventos")]
    public UnityEvent AlHacerClic;

    private SpriteRenderer sr;
    private Color colorOriginal;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if (sr != null) colorOriginal = sr.color;

        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    void OnMouseEnter()
    {
        if (sr != null) sr.color = colorResaltado;
    }

    void OnMouseExit()
    {
        if (sr != null) sr.color = colorOriginal;
    }

    void OnMouseDown()
    {
        AlHacerClic?.Invoke();
    }
}
