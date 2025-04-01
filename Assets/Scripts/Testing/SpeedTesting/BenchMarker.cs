//---------------------------------------------------------
// El script sirve para medir el renidimiento de un fragmento de código.
// Sergio Valiente Urueña
// The Last Vessel
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using System.Diagnostics;

public class BenchMarker : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    [Range(0f, 1000000), SerializeField] private float _iterations;

    #endregion
    
    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    private BenchMarkTest _benchMarkTest;

    #endregion
    
    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    
    void Awake()
    {
        _benchMarkTest = GetComponent<BenchMarkTest>();
    }

    void Update()
    {
        if(InputManager.Instance.TestingWasPressedThisFrame())
        {
            RunTest();
        }
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    
    /// <summary>
    /// Ejecutamos el metodo PerformBenchmarkTest de la clase BenchMarkTest un numero de veces _iterations.
    /// Se mide el tiempo que tarda en ejecutarse y se muestra por consola.
    /// </summary>
    public void RunTest()
    {
        Stopwatch sw = Stopwatch.StartNew();
        sw.Start();

        for(int i = 0; i < _iterations; i++)
        {
            _benchMarkTest.PerformBenchmarkTest();
        }

        sw.Stop();

        UnityEngine.Debug.Log(sw.ElapsedMilliseconds + "ms");
    }

    #endregion
    


} // class BenchMarker 
// namespace
