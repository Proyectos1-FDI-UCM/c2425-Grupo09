//---------------------------------------------------------
// Este script se encarga de gestionar la cuerda del grappler (principalmente la animacion)
// Sergio Valiente
// The Last Vessel
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;

/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class GrapplerRope : MonoBehaviour
{
     // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    [Header("General Refernces:")]
    public GrapplerGun _GrapplerGun;
    public LineRenderer _LineRenderer;

    [Header("General Settings:")]
    [SerializeField] private int Percision = 120;
    [Range(0, 40)] [SerializeField] private float StraightenLineSpeed = 20;

    [Header("Rope Animation Settings:")]
    public AnimationCurve ropeAnimationCurve;
    [Range(0.01f, 4)] [SerializeField] private float StartWaveSize = 2;
    

    [Header("Rope Progression:")]
    public AnimationCurve ropeProgressionCurve;
    [SerializeField] [Range(1, 50)] private float RopeProgressionSpeed = 10;

    [HideInInspector] public bool isGrappling = true;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    float _moveTime = 0;
    float _waveSize = 0;
    bool _straightLine = true;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    /// <summary>
    /// OnEnable se llama cuando se activa el componente
    /// </summary>
    private void OnEnable()
    {
        _moveTime = 0;
        _LineRenderer.positionCount = Percision;
        _waveSize = StartWaveSize;
        _straightLine = false;

        LinePointsToFirePoint();

        _LineRenderer.enabled = true;
    }

    /// <summary>
    /// OnDisable se llama cuando se desactiva el componente
    /// </summary>
    private void OnDisable()
    {
        _LineRenderer.enabled = false;
        isGrappling = false;
    }
    
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void Update()
    {
        _moveTime += Time.deltaTime;
        DrawRope();
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    /// <summary>
    /// 
    /// </summary>
    private void LinePointsToFirePoint()
    {
        for (int i = 0; i < Percision; i++)
        {
            _LineRenderer.SetPosition(i, _GrapplerGun.firePoint.position);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    void DrawRope()
    {
        if (!_straightLine)
        {
            if (_LineRenderer.GetPosition(Percision - 1).x == _GrapplerGun.grapplePoint.x)
            {
                _straightLine = true;
            }
            else
            {
                DrawRopeWaves();
            }
        }
        else
        {
            if (!isGrappling)
            {
                _GrapplerGun.Grapple();
                isGrappling = true;
            }
            if (_waveSize > 0)
            {
                _waveSize -= Time.deltaTime * StraightenLineSpeed;
                DrawRopeWaves();
            }
            else
            {
                _waveSize = 0;

                if (_LineRenderer.positionCount != 2) { _LineRenderer.positionCount = 2; }

                DrawRopeNoWaves();
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    void DrawRopeWaves()
    {
        for (int i = 0; i < Percision; i++)
        {
            float delta = (float)i / ((float)Percision - 1f);
            Vector2 offset = Vector2.Perpendicular(_GrapplerGun.grappleDistanceVector).normalized * ropeAnimationCurve.Evaluate(delta) * _waveSize;
            Vector2 targetPosition = Vector2.Lerp(_GrapplerGun.firePoint.position, _GrapplerGun.grapplePoint, delta) + offset;
            Vector2 currentPosition = Vector2.Lerp(_GrapplerGun.firePoint.position, targetPosition, ropeProgressionCurve.Evaluate(_moveTime) * RopeProgressionSpeed);

            _LineRenderer.SetPosition(i, currentPosition);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    void DrawRopeNoWaves()
    {
        _LineRenderer.SetPosition(0, _GrapplerGun.firePoint.position);
        _LineRenderer.SetPosition(1, _GrapplerGun.grapplePoint);
    }

    #endregion
}
