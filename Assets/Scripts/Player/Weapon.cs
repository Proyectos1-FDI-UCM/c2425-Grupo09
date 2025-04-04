//---------------------------------------------------------
// Script que se encarga de gestionar el disparo
// Sergio Valiente Urueña
// The Last Vessel
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using

/// <summary>
/// Script que se encarga de gestionar el disparo
/// </summary>
public class Weapon : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
   
    //Prefab de la bala
    [SerializeField] private GameObject BulletPrefab;
    [SerializeField] private Transform FiringPointRight;
    [SerializeField] private Transform FiringPointLeft;

    //Cadencia de disparo
    [SerializeField] private float CadenciaDisparo;
    [SerializeField] Animator animator;
    [SerializeField] PlayerController playercontroller;
    #endregion
    
    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    //Variable para almacenar el último tiempo en el que se disparó
    private float _tiempoUltimoDisparo;
    private SpriteRenderer _sr;

    #endregion
    
    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// Si se detecta el input del disparo, y ha pasado el tiempo de cooldown desde el disparo anterior, ejecuta la accion de disparar.
    /// </summary>
    void Update()
    {
        if (InputManager.Instance.FireWasPressedThisFrame() && Time.time > _tiempoUltimoDisparo + CadenciaDisparo)
        {
            animator.SetTrigger("Attack");
            _tiempoUltimoDisparo = Time.time;
        }
            
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    /// <summary>
    /// Instancia la bala a la izquierda o derecha del jugador en función de si esta girado o no, y le otorga velocidad en esa dirección.
    /// </summary>
    private void Shoot()
    {
        bool isFlipped = playercontroller.FlippedRight;

        Vector3 _firingPoint = (isFlipped ? FiringPointRight : FiringPointRight).position;
        Vector3 _bulletDirection = isFlipped ? Vector3.right : Vector3.left;

        GameObject newBullet = Instantiate(BulletPrefab, _firingPoint, Quaternion.identity);
        Bullet bulletScript = newBullet.GetComponent<Bullet>();

        bulletScript.ImpulseBullet(_bulletDirection);
        bulletScript.HabilidadTigre(playercontroller.Tiger());
    }

    #endregion   

} // class Shoot 
// namespace
