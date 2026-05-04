using UnityEngine;

[CreateAssetMenu(fileName = "NuevoAnuncio", menuName = "CafeteriGatos/Anuncio")]
public class DatosAnuncio : ScriptableObject
{
    [Header("Contenido")]
    public string titulo;
    [TextArea(2, 4)]
    public string descripcion;
    public Sprite icono;

    [Header("Tipo")]
    public TipoAnuncio tipo;

    [Header("Coste")]
    public int coste;          
    public bool estaDesbloqueado;

    [Header("Efecto")]
    public EfectoAnuncio efecto;
    public float valorEfecto;   
}

public enum TipoAnuncio
{
    Mejora,    
    Evento,     
    Info        
}

public enum EfectoAnuncio
{
    Ninguno,
    BonusPacienciaClientes,
    MultiplicadorGanancias,
    BonusAparicionClientes,
    DesbloquearMinijuego
}