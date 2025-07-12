using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int tirosRestantes = 3; // Número de tiros restantes

    public int puntosRequeridos = 6; // Puntos necesarios para ganar

    public int puntosJugador = 0; // Puntos del jugador actual

    private ButtonManager buttonManager; // Referencia al ButtonManager

    private bool escenaRecargando = false; // Indica si la escena se está recargando


    void Awake()
    {
        buttonManager = GetComponent<ButtonManager>();
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // o Debug.LogWarning si prefieres mantenerlo
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // opcional, si quieres que persista entre escenas
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (escenaRecargando) return;

        if (tirosRestantes < 0)
        {
            escenaRecargando = true;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        if (puntosJugador >= puntosRequeridos)
        {
            buttonManager.MostrarPanel1(); // Muestra el panel de victoria  
        }
    }
}
