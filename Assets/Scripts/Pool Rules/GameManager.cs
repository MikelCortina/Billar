using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Valores actuales")]
    public int tirosRestantes = 3;
    public int puntosJugador = 0;
    public int puntosRequeridos = 3;

    [Header("Valores iniciales")]
    public int tirosRestantesInicio = 3;
    public int puntosJugadorInicio = 0;
    public int puntosRequeridosInicio = 3;

    public ButtonManager buttonManager;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Mantiene el GameManager entre escenas si llegas a usar más

        buttonManager = GetComponent<ButtonManager>();
        ReiniciarEstado(); // Establece estado inicial
    }

    private void Update()
    {
        if (tirosRestantes < 0)
        {
            // Jugador perdió, reiniciar ronda (no escena)
            ReiniciarRonda();
        }

        if (puntosJugador >= puntosRequeridos)
        {
            buttonManager.MostrarPanel1(); // Panel de victoria
            puntosJugador = 0;
        }
    }

    public void ReiniciarEstado()
    {
        puntosJugador = puntosJugadorInicio;
        tirosRestantes = tirosRestantesInicio;
        puntosRequeridos = puntosRequeridosInicio;
    }

    public void ReiniciarRonda()
    {
        puntosJugador = puntosJugadorInicio;
        tirosRestantes = tirosRestantesInicio;

        buttonManager.ReiniciarPosiciones();
        
    }

    public void SiguienteNivel()
    {
        puntosJugador = 0;
        tirosRestantes = tirosRestantesInicio;
        puntosRequeridos += 2;

        buttonManager.ReiniciarPosiciones();
    }

    public void AddTiros(int cantidad)
    {
        tirosRestantes += cantidad;
        
    }

    public void MultiplicarPuntaje(float factor)
    {
        puntosJugador = Mathf.RoundToInt(puntosJugador * factor);
    }

    public void AplicarJokers()
    {
        GetComponent<JokerManager>().ActivarJokers(this);
    }
}
