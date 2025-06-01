using UnityEngine;

public class BolaCollisionHandler : MonoBehaviour
{
    private BolaFisica bola;

    void Start()
    {
        bola = GetComponent<BolaFisica>();
        if (bola == null)
        {
            Debug.LogError("BolaFisica no encontrado en " + gameObject.name);
        }
    }

    void FixedUpdate()
    {
        if (bola == null) return;

        // --- Colisiones con otras bolas ---
        foreach (var otraBola in FindObjectsOfType<BolaFisica>())
        {
            if (otraBola == bola) continue;

            if (CollisionFunctions.CircleToCircle1(
                bola.transform.position, bola.radio,
                otraBola.transform.position, otraBola.radio,
                out Vector2 contactDirection, out float contactMagnitude, out Vector2 contactPoint))
            {
                Vector2 pos1 = bola.transform.position;
                Vector2 pos2 = otraBola.transform.position;

                Vector2 v1 = bola.velocidad;
                Vector2 v2 = otraBola.velocidad;

                Vector2 normal = (pos2 - pos1).normalized;
                Vector2 tangent = new Vector2(-normal.y, normal.x);

                float v1n = Vector2.Dot(v1, normal);
                float v1t = Vector2.Dot(v1, tangent);
                float v2n = Vector2.Dot(v2, normal);
                float v2t = Vector2.Dot(v2, tangent);

                float restitution = 0.98f;

                float v1nAfter = v2n * restitution;
                float v2nAfter = v1n * restitution;

                bola.velocidad = v1nAfter * normal + v1t * tangent;
                otraBola.velocidad = v2nAfter * normal + v2t * tangent;

                float overlap = bola.radio + otraBola.radio - Vector2.Distance(pos1, pos2);

                // Reproducir sonido de colisión proporcional a la media de las velocidades
                float velocidadMedia = (v1.magnitude + v2.magnitude) / 2f;

                var sonido1 = bola.GetComponent<BolaSonidoColision>();
                var sonido2 = otraBola.GetComponent<BolaSonidoColision>();

                if (sonido1 != null) sonido1.ReproducirSonidoColision(velocidadMedia);
                if (sonido2 != null) sonido2.ReproducirSonidoColision(velocidadMedia);

                if (overlap > 0)
                {
                    Vector2 separation = normal * (overlap / 2f + 0.001f);
                    bola.transform.position -= (Vector3)separation;
                    otraBola.transform.position += (Vector3)separation;
                }
            }
        }

        // --- Colisiones con obstáculos tipo AABB ---
        foreach (var obstaculo in GameObject.FindGameObjectsWithTag("Banda"))
        {
            BoxCollider2D box = obstaculo.GetComponent<BoxCollider2D>();
            if (box == null) continue;

            Vector2 aabbCenter = box.bounds.center;
            Vector2 halfSize = box.bounds.extents;

            if (CollisionFunctions.CircleToAABBResolution(
                bola.transform.position, bola.radio,
                aabbCenter, halfSize,
                out Vector2 contactDir, out float contactMag, out Vector2 contactPoint))
            {
                // Separar bola fuera del rectángulo
                bola.transform.position += (Vector3)(contactDir * (contactMag + 0.001f));

                // Reflejar la velocidad
                bola.velocidad = Vector2.Reflect(bola.velocidad, contactDir) * 0.95f; // 0.95 para simular pérdida de energía
            }
        }
    }
}
