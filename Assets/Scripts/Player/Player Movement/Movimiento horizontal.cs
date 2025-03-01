//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// Nombre del juego
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using

using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class MovimientoHorizontal : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    [SerializeField] float velocidad;
    [SerializeField] GameObject clock;
    [SerializeField] GameObject timertext;
    private Timer timer;
    #endregion


    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    private Rigidbody2D rig;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour


    private void Awake()
    {
        rig = GetComponent<Rigidbody2D>();
        timer = timertext.GetComponent<Timer>();
    }

    private void FixedUpdate()
    {
        float moveX = InputManager.Instance.MovementVector.x;

        rig.velocity = new Vector2(velocidad * moveX, rig.velocity.y);

        if (moveX > 0)
            transform.rotation = Quaternion.Euler(0, 0, 0);
        else if (moveX < 0)
            transform.rotation = Quaternion.Euler(0, 180, 0);
    }

    void Update()
    {

    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos


    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Clock"))
        {
            timer.Reloj();
            Destroy(collision.gameObject);
        }
    }
    #endregion
    // class NewBehaviourScript 
    // namespace
}
