//---------------------------------------------------------
// Script que lleva la bala para gestionar su movimiento y colsiones
// Sergio Valiente Urueña
// The Last Vessel
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Script que lleva la bala para gestionar su movimiento y colsiones
/// </summary>
public class Bullet : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    
    //Velocidad bala
    [SerializeField] private float Speed;
    //Daño bala
    [SerializeField] private int Damage;

    #endregion
    
    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    //Rigidbody2D bala
    private Rigidbody2D _rb;

    #endregion
    
    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    
    /// <summary>
    /// En el start, se le da la velocidad a la bala.
    /// </summary>
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.velocity = transform.right * Speed;

        Destroy(gameObject, 5f);
    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
 

    #endregion
    
    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    /// <summary>
    /// Detecta la colision, y destruye la bala al detectarla. Además verifica si esta colisionando con un animal, en cuyo caso aumenta su barra de sueño.
    /// </summary>
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.gameObject.GetComponent<BarraDeSueño>() != null)
        {
            coll.gameObject.GetComponent<BarraDeSueño>().Dormir(Damage);

            //Si el enemigo está de espaldas, lo gira para que ataque
            if (Mathf.Abs(transform.rotation.eulerAngles.y - coll.gameObject.transform.rotation.eulerAngles.y) <= 0.001f) 
                coll.gameObject.GetComponent<AnimalController>().TurnAround();
        } 
        Destroy(gameObject);
    }

    #endregion   

} // class Bullet 
// namespace
