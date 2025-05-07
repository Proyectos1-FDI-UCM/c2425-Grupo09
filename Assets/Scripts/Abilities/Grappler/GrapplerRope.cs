//---------------------------------------------------------
// Este script se encarga de gestionar la cuerda del grappler (principalmente la animacion)
// Sergio Valiente
// The Last Vessel
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using System;

/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class GrapplerRope : MonoBehaviour
{
     // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    [Header("General Refernces:")]
    [SerializeField] private GrapplerGun _GrapplerGun;
    [SerializeField] private LineRenderer _LineRenderer;

    [Header("General Settings:")]
    [SerializeField] private int Precision = 120;
    [Range(0, 40)] [SerializeField] private float StraightenLineSpeed = 20;

    [Header("Rope Animation Settings:")]
    [SerializeField] private AnimationCurve ropeAnimationCurve;
    [Range(0.01f, 4)] [SerializeField] private float StartWaveSize = 2;
    

    [Header("Rope Progression:")]
    [SerializeField] private AnimationCurve ropeProgressionCurve;
    [SerializeField] [Range(1, 50)] private float RopeProgressionSpeed = 10;

    //Evento que se llama cuando cambia el estado de IsGrappling
    public event Action<bool> OnGrappleStateChanged;

    public bool IsGrappling
    {
        get => _isGrappling;
        set
        {
            if (_isGrappling != value) // Detecta cambio de estado
            {
                _isGrappling = value;
                OnGrappleStateChanged?.Invoke(_isGrappling);
            }
        }
    }


    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    private float _moveTime = 0;
    private float _waveSize = 0;
    private bool _straightLine = true;
    private bool _isGrappling;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    /// <summary>
    /// OnEnable se llama cuando se activa el componente
    /// </summary>
    private void OnEnable()
    {
        _moveTime = 0;
        _LineRenderer.positionCount = Precision;
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
        IsGrappling = false;
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
        for (int i = 0; i < Precision; i++)
        {
            _LineRenderer.SetPosition(i, _GrapplerGun.FirePoint.position);
        }
    }

    /// <summary>
    /// Dibuja la cuerda
    /// </summary>
    void DrawRope()
    {
        if (!_straightLine)
        {
            if (_LineRenderer.GetPosition(Precision - 1).x == _GrapplerGun.GrapplePoint.x)
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
            if (!IsGrappling)
            {
                _GrapplerGun.Grapple();
                IsGrappling = true;
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
    /// Dibujar cuerda con las ondas
    /// </summary>
    void DrawRopeWaves()
    {
        for (int i = 0; i < Precision; i++)
        {
            float delta = (float)i / ((float)Precision - 1f);
            Vector2 offset = Vector2.Perpendicular(_GrapplerGun.GrappleDistanceVector).normalized * ropeAnimationCurve.Evaluate(delta) * _waveSize;
            Vector2 targetPosition = Vector2.Lerp(_GrapplerGun.FirePoint.position, _GrapplerGun.GrapplePoint, delta) + offset;
            Vector2 currentPosition = Vector2.Lerp(_GrapplerGun.FirePoint.position, targetPosition, ropeProgressionCurve.Evaluate(_moveTime) * RopeProgressionSpeed);

            _LineRenderer.SetPosition(i, currentPosition);
        }
    }

    /// <summary>
    /// Dibujar cuerda sin las ondas
    /// </summary>
    void DrawRopeNoWaves()
    {
        _LineRenderer.SetPosition(0, _GrapplerGun.FirePoint.position);
        _LineRenderer.SetPosition(1, _GrapplerGun.GrapplePoint);
    }

    #endregion
}
