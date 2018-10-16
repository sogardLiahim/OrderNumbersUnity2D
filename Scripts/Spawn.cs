using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{

    public GameObject prefabObject;
    public Transform spawnerLocation;

    public int numbersOfPrefabsDesired = 8;
    public const int MAXNUMBER = 10;
    public const float OFFSET = 2.5f;

    public struct objectCollection
    {
        public int entityID;

        public GameObject entityObject;
        public Vector3 entityObjectPosition;
    }

    public static List<objectCollection> sObjectCollections = new List<objectCollection>();

    /// <summary>
    ///   Obiectele sunt initialiazate cu un offset de 2.5f.
    ///   Apple : ObjectNumber
   
    ///   public class ObjectNumber : MonoBehaviour {

    ///static public List<Spawn.objectCollection> sObjectCollectionsMember = new List<Spawn.objectCollection>();
    ///  static public List<int> randomNumbersList = new List<int>();
    /// </summary>
    /// 

    void Awake()
    {
        if (numbersOfPrefabsDesired <= MAXNUMBER)
        {

            Vector3 offsetVector = new Vector3(OFFSET, 0f, 0f);

            //Instantiere obiecte de tip gameobject si popularea lor in lista.

            for (int i = 1; i < numbersOfPrefabsDesired; i++)
            {
                //OFFSET si instatiere obiect macheta pentru popularea dinamica in functie de dificultate.

                spawnerLocation.position += offsetVector;

                var prefabTemp = Instantiate(prefabObject);
                prefabTemp.transform.position = spawnerLocation.position;
                prefabTemp.transform.rotation = spawnerLocation.rotation;


                //Initializare lista sObjectCollection si populare
                objectCollection temporaryObjectCollection;
                temporaryObjectCollection.entityID = i;
                temporaryObjectCollection.entityObject = prefabTemp;
                temporaryObjectCollection.entityObjectPosition = prefabTemp.transform.position;

                sObjectCollections.Add(temporaryObjectCollection);
            }

        }


    }

    void Start()
    {

        //TODO UPDATE STRUCT, ADAUGA SI VALUAREA INT INAUNTRU LISTEI.
        foreach (var obj in sObjectCollections)
        {

            Vector2 sprite_size = obj.entityObject.GetComponent<SpriteRenderer>().sprite.rect.size;
            Debug.Log(obj.entityObject.transform.position);

            Debug.DrawLine(obj.entityObject.transform.position,
                           obj.entityObject.transform.position - Vector3.up * 25f,
                           Color.red, Mathf.Infinity, false);
        }

        Apple objectRetriva = new Apple(sObjectCollections);

    }


    // Update is called once per frame
    void Update()
    {

    }

}

//public delegate void jobSendListObjects (List <Spawn.objectCollection> sObjectCollections);

public class Apple : ObjectNumber
{


    public Apple(List<Spawn.objectCollection> sObjectCollections) : base (sObjectCollections)
    {
        this.sObjectCollections = sObjectCollections;

    }

    private List<Spawn.objectCollection> sObjectCollections;
}

