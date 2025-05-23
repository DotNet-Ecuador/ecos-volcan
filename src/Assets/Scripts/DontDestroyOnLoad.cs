using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistenteEntreEscenas : MonoBehaviour
{
    private static bool existe = false;

    void Awake()
    {
        if (!existe)
        {
            DontDestroyOnLoad(gameObject);
            existe = true;
        }
        else
        { 
            Destroy(gameObject); // Evita duplicados si regresas a una escena anterior
        }
    }
}