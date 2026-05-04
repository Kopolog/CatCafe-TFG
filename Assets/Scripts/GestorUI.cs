using UnityEngine;
using TMPro;

public class GestorUI : MonoBehaviour
{
    public static GestorUI Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI textoDinero;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        ActualizarDinero(0);
    }

    public void ActualizarDinero(int cantidad)
    {
        if (textoDinero != null)
            textoDinero.text = cantidad.ToString();
    }
}