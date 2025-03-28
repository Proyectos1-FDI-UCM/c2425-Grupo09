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
    [SerializeField] private float Damage = 10;

    [SerializeField] private float TigerMultiplier = 1.25f;

    #endregion
    
    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    //Rigidbody2D bala
    private Rigidbody2D _rb;
    private SpriteRenderer _sr;
    private float _damage;
    #endregion
    
    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos

    /// <summary>
    /// Método que se llama al disparar la bala. Le da velocidad en la dirección que recibe.
    /// </summary>
    public void ImpulseBullet(Vector3 _direction)
    {
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();

        _rb.velocity = _direction * Speed;
        _sr.flipX = _direction.x == 1;
    }
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
            gameObject.SetActive(false);

            BarraDeSueño _sleepBar =  coll.gameObject.GetComponent<BarraDeSueño>();
            _sleepBar.Dormir(_damage);

            if(coll.gameObject.GetComponent<AnimalController>() != null)
            {
                AnimalController _animalController = coll.gameObject.GetComponent<AnimalController>();

                //Si el enemigo está de espaldas, lo gira para que ataque
                if (_animalController.IsFlipped == _sr.flipX && !_sleepBar.Dormido()) 
                {
                    _animalController.TurnAround();
                }
            }
            
                
        } 
        Destroy(gameObject);
    }

    /// <summary>
    /// Si la bala se sale de la pantalla, la destruye
    /// </summary>
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    public void HabilidadTigre(bool tigre)
    {
        if (!tigre)
        {
            _damage = Damage;
        }
        else
        {
            _damage = Damage * TigerMultiplier;
        }

    }
    #endregion   

} // class Bullet 
// namespace
