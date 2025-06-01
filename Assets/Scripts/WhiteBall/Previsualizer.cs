using UnityEngine;

[RequireComponent(typeof(BolaFisica))]
public class BolaDisparoPreview : MonoBehaviour
{
    public float fuerzaDisparo = 10f;
    public float radioBola = 0.15f;
    public LayerMask capaBolas;

    private BolaFisica bola;
    private Camera cam;
    private LineRenderer lineRenderer;

    void Start()
    {
        bola = GetComponent<BolaFisica>();
        cam = Camera.main;

        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
            lineRenderer = gameObject.AddComponent<LineRenderer>();

        lineRenderer.positionCount = 0;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.yellow;
        lineRenderer.endColor = Color.red;
    }

    void Update()
    {
        if (!GetComponent<BolaDisparo>().IsAiming)
        {
            lineRenderer.positionCount = 0;
            return;
        }

        Vector2 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direccion = ((Vector2)transform.position - mouseWorld).normalized;
        float distancia = Vector2.Distance(transform.position, mouseWorld);
        Vector2 velocidadInicial = direccion * distancia * fuerzaDisparo;

        // Raycast
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, radioBola, direccion, 10f, capaBolas);

        if (hit.collider != null && hit.collider.gameObject != gameObject)
        {
            Transform bolaObjetivo = hit.collider.transform;
            Vector2 puntoImpacto = hit.point;

            // Normal del impacto: desde punto de impacto hacia centro de la otra bola
            Vector2 normal = ((Vector2)bolaObjetivo.position - puntoImpacto).normalized;

            // Tangente perpendicular
            Vector2 tangente = new Vector2(-normal.y, normal.x);

            // Proyecciones de velocidad
            float v1n = Vector2.Dot(velocidadInicial, normal);
            float v1t = Vector2.Dot(velocidadInicial, tangente);

            // Resultado final de velocidades (masa igual, bola objetivo en reposo)
            Vector2 velBlancaPost = v1t * tangente;      // La blanca se desliza tangencialmente
            Vector2 velObjetivoPost = v1n * normal;       // La objetivo va en la normal

            // Coordenadas para mostrar
            Vector2 postGolpeBlanca = puntoImpacto + velBlancaPost.normalized * 1.5f;
            Vector2 postGolpeObjetivo = (Vector2)bolaObjetivo.position + velObjetivoPost.normalized * 1.5f;

            lineRenderer.positionCount = 6;
            lineRenderer.SetPosition(0, transform.position);        // Desde blanca
            lineRenderer.SetPosition(1, puntoImpacto);              // Hasta impacto

            lineRenderer.SetPosition(2, puntoImpacto);              // Desde punto de impacto
            lineRenderer.SetPosition(3, postGolpeBlanca);           // Dirección post-impacto blanca

            lineRenderer.SetPosition(4, bolaObjetivo.position);     // Desde objetivo
            lineRenderer.SetPosition(5, postGolpeObjetivo);         // Dirección objetivo
        }
        else
        {
            // Línea recta si no colisiona
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, (Vector2)transform.position + direccion * 5f);
        }
    }
}
