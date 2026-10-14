using UnityEngine;

/// <summary>
/// Patron Template Method: Define el esqueleto del algoritmo de deteccion sensorial (vision/cono).
/// Las clases concretas (guardias, camaras de seguridad) heredan este esqueleto e implementan
/// los detalles especificos (origen del raycast, orientacion hacia adelante, etc.).
/// </summary>
public abstract class StealthSensorBase : MonoBehaviour
{
    [Header("Detection Settings")]
    public float viewDistance = 5f;
    public float viewAngle = 45f;
    public LayerMask obstacleMask;
    public LayerMask playerMask;

    protected Transform targetPlayer;

    protected virtual void Start()
    {
        FindTarget();
    }

    protected void FindTarget()
    {
        if (targetPlayer == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) targetPlayer = p.transform;
        }
    }

    /// <summary>
    /// Metodo plantilla (Template Method): fija los pasos invariantes del algoritmo de deteccion.
    /// 1. Comprueba si el objetivo es detectable (cheats/sigilo activo)
    /// 2. Comprueba distancia
    /// 3. Comprueba angulo del cono
    /// 4. Comprueba linea de vision mediante raycast
    /// 5. Ejecuta la respuesta de deteccion
    /// </summary>
    public bool CheckDetection()
    {
        if (!IsTargetDetectable()) return false;
        if (targetPlayer == null)
        {
            FindTarget();
            if (targetPlayer == null) return false;
        }

        Vector2 origin = GetSensorOrigin();
        Vector2 targetPos = targetPlayer.position;
        Vector2 dirToTarget = (targetPos - origin).normalized;
        float distance = Vector2.Distance(origin, targetPos);

        // Paso 2: Rango
        if (distance > viewDistance) return false;

        // Paso 3: Angulo
        Vector2 forwardDir = GetSensorForward();
        float angle = Vector2.Angle(forwardDir, dirToTarget);
        if (angle > viewAngle) return false;

        // Paso 4: Raycast / Obstaculos
        RaycastHit2D hit = Physics2D.Raycast(origin, dirToTarget, viewDistance, obstacleMask | playerMask);
        if (hit.collider != null && ((1 << hit.collider.gameObject.layer) & playerMask) != 0)
        {
            // Paso 5: Reaccion ante deteccion
            OnTargetDetected();
            return true;
        }

        return false;
    }

    protected virtual bool IsTargetDetectable()
    {
        if (CheatManager.Instance != null && CheatManager.Instance.Undetectable)
            return false;
        return true;
    }

    // Metodos abstractos / ganchos (hooks) que las subclases definen
    protected abstract Vector2 GetSensorOrigin();
    protected abstract Vector2 GetSensorForward();

    protected virtual void OnTargetDetected()
    {
        // Patron Observer: Notifica al sistema desacoplado
        GameEvents.TriggerPlayerDetected();
    }
}
