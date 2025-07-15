using UnityEngine;

public class JokerTrigger : MonoBehaviour
{
    public JokerManager jokerManager;

    public void OnInicioTurno()
    {
        jokerManager.ActivarJokers(FindObjectOfType<GameManager>());
    }

}
