
using UnityEngine;
using System.Collections.Generic;

public class JokerManager : MonoBehaviour
{
    public List<Joker> jokersActivos = new List<Joker>();

    public void ActivarJokers(GameManager gameManager)
    {
        foreach (var joker in jokersActivos)
        {
            joker.AplicarEfecto(gameManager);
        }
    }

    public void AddJoker(Joker nuevo)
    {
        jokersActivos.Add(nuevo);
        // Podr�as lanzar un evento o actualizar UI aqu�
    }
}
