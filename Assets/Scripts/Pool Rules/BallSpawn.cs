using UnityEngine;

public class GeneradorDeBolasBillar : MonoBehaviour
{
    public GameObject[] bolas;          // Array con prefabs de las 15 bolas (index 0 = bola 1, index 7 = bola 8, etc.)
    public GameObject prefabBolaBlanca; // Prefab bola blanca
    public float separacion = 0.3f;     // Separación entre bolas

    void Start()
    {
        Vector3 inicioTriangulo = transform.position;

        // Arreglo de posiciones para el triángulo (filas y columnas)
        int filas = 5;
        int bolaIndex = 0;

        for (int fila = 0; fila < filas; fila++)
        {
            float xOffset = -fila * separacion / 2f;
            for (int columna = 0; columna <= fila; columna++)
            {
                Vector3 pos = inicioTriangulo + new Vector3(xOffset + columna * separacion, 0f, fila * separacion * Mathf.Sqrt(3) / 2f);

                // Instanciar bola según la posición siguiendo reglas:
                //  - Bola 8 en centro (fila=2, col=1)
                //  - Bola lisa en esquina inferior izquierda (fila=4, col=0)
                //  - Bola rayada en esquina inferior derecha (fila=4, col=4)

                int bolaParaInstanciar;

                if (fila == 2 && columna == 1)
                {
                    // Bola 8
                    bolaParaInstanciar = 7; // índice 7 = bola 8
                }
                else if (fila == 4 && columna == 0)
                {
                    // Bola lisa cualquiera (1-7) que no sea la 8, aquí bola 1 (index 0)
                    bolaParaInstanciar = 0;
                }
                else if (fila == 4 && columna == 4)
                {
                    // Bola rayada cualquiera (9-15), aquí bola 9 (index 8)
                    bolaParaInstanciar = 8;
                }
                else
                {
                    // Para el resto de bolas, vamos alternando lisa y rayada, saltando las posiciones especiales ya usadas

                    // Creamos una lista de las bolas que faltan:
                    // - Bolas lisas disponibles: 2,3,4,5,6,7 (indices 1 a 6)
                    // - Bolas rayadas disponibles: 10-15 (indices 9 a 14)

                    // Simplemente alternamos lisas y rayadas en orden

                    // Lista de bolas lisas (sin 1 y 8)
                    int[] lisas = { 1, 2, 3, 4, 5, 6 };
                    // Lista de rayadas (sin 9 que ya usamos)
                    int[] rayadas = { 9, 10, 11, 12, 13, 14 };

                    // Contadores para lisas y rayadas
                    // Usaremos bolaIndex para avanzar en estas listas

                    // Alternar: si bolaIndex es par bola lisa, impar bola rayada

                    int posEnTriangulo = fila * (fila + 1) / 2 + columna;

                    if (posEnTriangulo == 0 || posEnTriangulo == 4 || posEnTriangulo == 14)
                    {
                        // Ya asignadas en casos especiales, saltar
                        continue;
                    }

                    if (bolaIndex % 2 == 0)
                    {
                        int idxL = (bolaIndex / 2) % lisas.Length;
                        bolaParaInstanciar = lisas[idxL];
                    }
                    else
                    {
                        int idxR = (bolaIndex / 2) % rayadas.Length;
                        bolaParaInstanciar = rayadas[idxR];
                    }
                    bolaIndex++;
                }

                Instantiate(bolas[bolaParaInstanciar], pos, Quaternion.identity);
            }
        }

        // Bola blanca separada un poco delante del triángulo
        Vector3 posBolaBlanca = inicioTriangulo + new Vector3(0f, 0f, -separacion * 2f);
        Instantiate(prefabBolaBlanca, posBolaBlanca, Quaternion.identity);
    }
}
