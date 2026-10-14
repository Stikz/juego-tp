using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Entrada serializable para registrar tipos de enemigos en el Inspector de Unity.
/// Permite extender nuevos enemigos sin modificar codigo C# (Principio Abierto/Cerrado - OCP).
/// </summary>
[Serializable]
public class EnemyFactoryEntry
{
    public string id;
    public GameObject enemyPrefab;
}

/// <summary>
/// Patron Factory (con Diccionario): Centraliza la logica de instanciacion de enemigos.
/// Cumple estrictamente con el principio Open-Closed (OCP) de SOLID tal como se ensena
/// en la Diapositiva 8 de la presentacion Factory de la catedra UADE, evitando switches rigidos.
/// </summary>
public class EnemyFactory : MonoBehaviour
{
    public static EnemyFactory Instance { get; private set; }

    [Header("Catalogo de Enemigos (Configurable desde el Inspector para OCP)")]
    [SerializeField] private EnemyFactoryEntry[] enemyCatalog;

    private Dictionary<string, GameObject> idEnemyMap;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        InitializeDictionary();
    }

    private void InitializeDictionary()
    {
        idEnemyMap = new Dictionary<string, GameObject>();

        if (enemyCatalog == null) return;

        foreach (var entry in enemyCatalog)
        {
            if (entry != null && !string.IsNullOrEmpty(entry.id) && entry.enemyPrefab != null)
            {
                if (!idEnemyMap.ContainsKey(entry.id))
                {
                    idEnemyMap.Add(entry.id, entry.enemyPrefab);
                }
            }
        }
    }

    /// <summary>
    /// Metodo Create del Patron Factory.
    /// Devuelve la referencia al nuevo enemigo instanciado a partir de su ID.
    /// </summary>
    public GameObject Create(string id, Vector3 position, Quaternion rotation)
    {
        if (idEnemyMap == null)
            InitializeDictionary();

        if (!idEnemyMap.TryGetValue(id, out GameObject prefab))
        {
            Debug.LogWarning($"[EnemyFactory] No se encontro un prefab registrado para el id: '{id}'.");
            return null;
        }

        GameObject enemyInstance = Instantiate(prefab, position, rotation);
        return enemyInstance;
    }

    public GameObject Create(string id, Vector3 position)
    {
        return Create(id, position, Quaternion.identity);
    }
}
