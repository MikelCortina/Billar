using UnityEngine;

public class ContadorRebotesAntesDeBlanca : MonoBehaviour
{
    public string tagBolaBlanca = "BolaBlanca";
    public int rebotes { get; private set; } = 0;
    public bool haTocadoBlanca { get; private set; } = false;
    public bool haSidoLanzada { get; private set; } = false;

    private BolaCollisionHandler colisionHandler;
    private BolaFisica bolaFisica;
    private GameManager gameManager;


    void Start()
    {
        colisionHandler = GetComponent<BolaCollisionHandler>();
        bolaFisica = GetComponent<BolaFisica>();

        if (colisionHandler == null)
        {
            Debug.LogError("BolaCollisionHandler no encontrado en " + gameObject.name);
            enabled = false;
            return;
        }

        if (bolaFisica == null)
        {
            Debug.LogError("BolaFisica no encontrado en " + gameObject.name);
            enabled = false;
            return;
        }
        gameManager = GameManager.Instance;

        if (gameManager == null)
        {
            Debug.LogError("GameManager no encontrado");
        }

    }

    void Update()
    {
        if (bolaFisica.EstaEnMovimiento && !haSidoLanzada)
        {
            haSidoLanzada = true;
        }
    
      if (!bolaFisica.EstaEnMovimiento)
        {
           Resetear();
        }
    }   

    public void ContarRebote()
    {
        if (haSidoLanzada && !haTocadoBlanca)
        {
            rebotes++;
           // Debug.Log($"{gameObject.name} rebote #{rebotes}");
        }
    }

    public void VerificarBolaBlanca(GameObject otraBola)
    {
        if (!haTocadoBlanca && otraBola.CompareTag(tagBolaBlanca))
        {
            haTocadoBlanca = true;
            Debug.Log($"{gameObject.name} ha tocado la bola blanca tras {rebotes} rebotes.");

            gameManager.puntosJugador += rebotes; // Añadir puntos al jugador
            Debug.Log($"Puntos del jugador: {gameManager.puntosJugador}");
        }
    }

    // Llamar a esto justo cuando se dispare la bola
    public void MarcarComoLanzada()
    {
        haSidoLanzada = true;
    }

    // Llamar al iniciar un nuevo turno
    public void Resetear()
    {
        rebotes = 0;
        haTocadoBlanca = false;
        haSidoLanzada = false;
    }
}
