//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Sergio Valiente
// The Last Vessel
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using System.Collections;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class CameraFollowObject : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    [Header("References")]
    [SerializeField] private Transform PlayerTransform;

    [Header("Flip Rotation Settings")]
    [SerializeField] private float FlipYRotationTime = 0.5f;
    

    #endregion
    
    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    private PlayerController _playerController;
    private bool _isFacingRight;

    private Coroutine _turnCoroutine;
    #endregion
    
    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    

    void Awake()
    { 
        if (PlayerTransform != null)
        {
            _playerController = PlayerTransform.GetComponent<PlayerController>();
        } else {
            Debug.LogError("PlayerTransform no está asignado en el Inspector.");
        }

        _isFacingRight = _playerController.FlippedRight; 
    }


    void Update()
    {
        transform.position = PlayerTransform.position;
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    public void CallTurn()
    {
        //_turnCoroutine = StartCoroutine(FlipYLerp());
        LeanTween.rotateY(gameObject, DetermineEndRotation(), FlipYRotationTime).setEaseInOutSine();
    }

    #endregion
    
    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    /// <summary>
    /// Este método es una alternativa si no se quiere usar LeanTween.
    /// </summary>
   
    private IEnumerator FlipYLerp()
    {
        float _startRotation = transform.localEulerAngles.y;
        float _endRotationAmount = DetermineEndRotation();
        float yRotation = 0f;

        float elapsedTime = 0f;
        while (elapsedTime < FlipYRotationTime)
        {
            elapsedTime += Time.deltaTime;

            //Lerp the y rotation
            yRotation = Mathf.Lerp(_startRotation, _endRotationAmount, (elapsedTime / FlipYRotationTime));
            transform.rotation = Quaternion.Euler(0f, yRotation, 0f);

            yield return null;
        }
    }

    private float DetermineEndRotation()
    {
        _isFacingRight = !_isFacingRight;
        if (_isFacingRight)
        {
            return 0f;
        }
        else
        {
            return 180f;
        }
    }
    #endregion   

} // class CameraFollowObject 
// namespace
