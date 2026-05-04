using UnityEngine;

[CreateAssetMenu(fileName = "NuevoGato", menuName = "CafeteriaGatos/Gato")]
public class DatosGato : ScriptableObject
{
    [Header("Identidad")]
    public string nombreGato;
    public Sprite fotoGato;
    [TextArea(2, 3)]
    public string descripcion;

    [Header("Dificultad minijuego")]
    [Range(1, 3)]
    public int dificultad; 

    [Header("Bonus al cafetería")]
    public BonusGato tipoBonus;
    public float valorBonus;      
    public string descripcionBonus; 

    [Header("Gasto pasivo")]
    public int gastoPorDia;       

    [Header("Estado")]
    public bool estaSalvado;
}

public enum BonusGato
{
    Ninguno,
    MasPacienciaClientes,
    MasGanancias,
    MasClientesPorHora,
    DescuentoCompras         
}