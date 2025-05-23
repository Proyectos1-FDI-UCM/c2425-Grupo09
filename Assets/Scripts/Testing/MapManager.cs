//---------------------------------------------------------
// Este script hace que el mapa se abra y se vuelva a cerrar, con sus respectivos detalles
// Diego García
// The Last Vessel
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using

public class MapManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints
    [SerializeField] GameObject Map;
    [SerializeField] GameObject PlayerIcon;
    [SerializeField] GameObject MapRoomContainer;
    [SerializeField] GameObject MapCamera;
    [SerializeField] Vector2 MinBounds;
    [SerializeField] Vector2 MaxBounds;
    [SerializeField] float velocidad;
    #endregion
    
    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints
    private bool _mapaAbierto;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        Vector2 _moveVector = InputManager.Instance.MapMovementVector;

        if (InputManager.Instance.MapWasPressedThisFrame() && !_mapaAbierto)
        {
            AudioManager.Instance.PlaySFX("map", true);

            Map.SetActive(true);
            PlayerIcon.SetActive(true);
            MapRoomContainer.SetActive(true);
            Time.timeScale = 0f;
            _mapaAbierto = true;
            InputManager.Instance.EnableUIControls();
            MapCamera.transform.position = new Vector3(PlayerIcon.transform.position.x, PlayerIcon.transform.position.y, MapCamera.transform.position.z);
        }
        else if (InputManager.Instance.MapCloseWasPressedThisFrame() && _mapaAbierto)
        {
            AudioManager.Instance.PlaySFX("map", true);

            Map.SetActive(false);
            PlayerIcon.SetActive(false);
            MapRoomContainer.SetActive(false);
            InputManager.Instance.EnablePlayerControls();
            Time.timeScale = 1f;
            _mapaAbierto = false;

        }
        else if (_mapaAbierto)
        {
            Vector3 moveVector3 = new Vector3(_moveVector.x, _moveVector.y, 0f);
            Vector3 newPosition = MapCamera.transform.position + moveVector3 * Time.unscaledDeltaTime * velocidad;

            newPosition.x = Mathf.Clamp(newPosition.x, MinBounds.x, MaxBounds.x);
            newPosition.y = Mathf.Clamp(newPosition.y, MinBounds.y, MaxBounds.y);

            MapCamera.transform.position = newPosition;
        }
    }
    #endregion
} // class MapManager 
// namespace
