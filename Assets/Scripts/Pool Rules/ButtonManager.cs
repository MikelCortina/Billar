using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] private GameObject panel1;
    [SerializeField] private GameObject panel2;

    void Start()
    {
       
    }

    // Método que oculta el Panel1 y muestra el Panel2
    public void MostrarPanel2()
    {
        if (panel1 != null) panel1.SetActive(false);
        if (panel2 != null) panel2.SetActive(true);
    }

    public void MostrarPanel1()
    {
        if (panel1 != null) panel1.SetActive(true);
    }
}

