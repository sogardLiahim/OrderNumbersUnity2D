using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectNumber : MonoBehaviour {

    static public List<Spawn.objectCollection> sObjectCollectionsMember = new List<Spawn.objectCollection>();
    static public List<int> randomNumbersList = new List<int>();

    public ObjectNumber(List<Spawn.objectCollection> sObjectCollections)
    {
        Debug.Log(sObjectCollections.Capacity);



        foreach (var obj in sObjectCollections)
        {
          Debug.Log(obj.entityID);
          Debug.Log(obj.entityObject.transform.position);
          obj.entityObject.name = obj.entityObject.name + obj.entityID.ToString();
           
          var text = obj.entityObject.transform.GetChild(0).gameObject; //.GetChild(0);
          Debug.Log(randomNumbersList.IndexOf(obj.entityID));
          text.GetComponent<TextMesh>().text = Random.Range(0, 100).ToString();
        }

        sObjectCollectionsMember = sObjectCollections;

    }




    // Use this for initialization
    void Start () {


        //var parent = transform.parent;

        //var parentRenderer = parent.GetComponent<Renderer>();
        //var renderer = GetComponent<Renderer>();
        //renderer.sortingLayerID = parentRenderer.sortingLayerID;
        //renderer.sortingOrder = parentRenderer.sortingOrder;

        //var spriteTransform = parent.transform;
        //var text = GetComponent<TextMesh>();
        //var pos = spriteTransform.position;
        //text.text = string.Format("{0}, {1}", pos.x, pos.y);

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
