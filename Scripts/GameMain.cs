using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameMain : MonoBehaviour {

    private const float MOUSEDELAY = 0.01f;
    private const int DEFAULTOBJECTNAME = 22;

    public LineRenderer lr;
    bool mouseStatus = false;
    bool corutineStatus = false;

    public static double timer = 0 ;


    void Start ()
    {
        InvokeRepeating("Timer", 0.1f, 0.1f);
    }

	
	// Update is called once per frame
	void Update ()
    {


        if (OnGameSucces())
        {
            Debug.Log("HighScore: " + timer);
        }

        lr.enabled = false;

        if (Input.GetMouseButton(0))
        {
 
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.Log(ray.origin.normalized);


            //Functia drag se desfasoara atunci cand mouse-ul selecteaza un obiect prin raycast,
            //Daca nu respecta conditia va fi activat catch drept valoare nullreference.
            //TODO Shader Outline nu functioneaza 

            try
            {
                var hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
                Debug.DrawLine(ray.origin, ray.direction);
                Debug.Log(hit.collider.gameObject);
                MouseifSelectedCard(hit.collider.gameObject);     
            }
            catch (System.NullReferenceException e)
            {
                Debug.Log("No object found", e);
                
            }


        }
         //MouseifSelectedCard();

    }

    

    //urmarirea obiectului si activarea shader-ului outline, TODO modifica numele in ceva mai consistent

    private void MouseifSelectedCard(GameObject objectNumer)
    {
        var objectNumerOriginalPos = objectNumer.transform.position;
        if (!corutineStatus)
        {
            corutineStatus = true;
            StartCoroutine(OnHitEvent());
        }
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            objectNumer.transform.position = mousePos;
             Vector2 objectScale = objectNumer.transform.localScale;
             LineTace(objectNumerOriginalPos,mousePos, objectScale, objectNumer);
            
    }

    private void LineTace(Vector3 orginalObjectNumerPos, Vector2 mousePos, Vector2 objectScale, GameObject numerObject)

    {

        Vector2 laserDefaultPosition = new Vector2(0.0f, 3.2f);
        var tempLineRender = lr;

        // RAY CHECKING

        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        Vector2 centerOffset = new Vector2(0, objectScale.y + 0.5f);
        Debug.DrawLine(mousePos + centerOffset, Vector2.up * 100f);
        var hit = Physics2D.Raycast(mousePos + centerOffset, Vector2.up, Mathf.Infinity);

        if (hit.collider)
        {
            tempLineRender.SetPosition(0, mousePos);
            tempLineRender.SetPosition(1, mousePos + laserDefaultPosition);
            lr.enabled = true;

            GameObject affectedGameObject = hit.collider.gameObject;
            StartCoroutine(OnCollideHitEvent(orginalObjectNumerPos,numerObject, affectedGameObject)); ;
        }

    }

    IEnumerator OnHitEvent()
    {
        corutineStatus = false;
        Debug.Log("fsafda  " + mouseStatus);
        yield return new WaitUntil(() => (mouseStatus = Input.GetMouseButtonUp(0)) == true);
    }

    IEnumerator OnCollideHitEvent(Vector3 orginalObjectNumerPos,GameObject onClickGameObject , GameObject affectedGameObject)
    {

        var swapPosition = affectedGameObject.transform.position;
        yield return new WaitForSeconds(MOUSEDELAY);

        if (mouseStatus == true)
        {
            //Schimbare gameObjects, indexarea se obtine dintr-un substring preluat din numele obiectului 

            string swapIdString = onClickGameObject.name.Substring(DEFAULTOBJECTNAME, 1);
            affectedGameObject.transform.position = FindEntityIdUsingStringAndReplace(swapIdString);

            onClickGameObject.transform.position = swapPosition;

            Debug.Log("onClickGame :"+onClickGameObject.transform.position);
            Debug.Log("affectedGameObject :" + affectedGameObject.transform.position);

            UpdateEntityIdTransformPosition(onClickGameObject.name.Substring(DEFAULTOBJECTNAME, 1), 
                                            affectedGameObject.name.Substring(DEFAULTOBJECTNAME, 1),
                                            onClickGameObject.transform.position,
                                            affectedGameObject.transform.position);
    
        }

        yield return 0;
    }


    //Toate metodele contin ca si cuvant cheie Entity, asa cum a fost denumita in clasa ObjectNumber

    private Vector3 FindEntityIdUsingStringAndReplace(string searchIdForTransform)
    {
        foreach (var item in ObjectNumber.sObjectCollectionsMember)
        {

            // Debug.Log(item.entityID);
            if (item.entityID.ToString() == searchIdForTransform)
            {
              return (item.entityObjectPosition);
            }
            //Debug.Log(item.entityObject.transform.position);
        }

        return Vector3.zero;
    }

    //Updateaza informatii curente, referitoare la lista din clasa ObjectNumber pentru Obiecte
    //Updateaza doar pozitia, entitatea ID ramane la fel

    private void UpdateEntityIdTransformPosition (string posObjOnClickId, string posObjAffectedId,Vector3 ObjOnClicked, Vector3 ObjAffected)
    {
        Debug.Log(posObjOnClickId +  posObjAffectedId);
        //obiecte temporare
        var posObjAffectedStructSpecificEntity = ObjectNumber.sObjectCollectionsMember.Find(x => (x.entityID.ToString() == posObjAffectedId));
        var posObjOnCickStructSpecificEntity = ObjectNumber.sObjectCollectionsMember.Find(x => (x.entityID.ToString() == posObjOnClickId));

        posObjOnCickStructSpecificEntity.entityObjectPosition = ObjOnClicked;
        posObjAffectedStructSpecificEntity.entityObjectPosition = ObjAffected;

        ObjectNumber.sObjectCollectionsMember[posObjOnCickStructSpecificEntity.entityID - 1] = posObjOnCickStructSpecificEntity;
        ObjectNumber.sObjectCollectionsMember[posObjAffectedStructSpecificEntity.entityID - 1] = posObjAffectedStructSpecificEntity;

    }

    private bool OnGameSucces()
    {

        List<int> conversion = new List<int>();

        foreach (var item in ObjectNumber.sObjectCollectionsMember)
        {

            conversion.Add(int.Parse((item.entityObject.transform.GetChild(0).gameObject.GetComponent<TextMesh>().text)));
        }

        List<int> correctGuees = new List<int>(conversion);
        correctGuees.Sort();


        foreach (var item in ObjectNumber.sObjectCollectionsMember)
        {
            var auto = item.entityObject.transform.GetChild(0).gameObject.GetComponent<TextMesh>().text;
            Debug.Log("entityID=" + item.entityID +" entityPos="+ item.entityObjectPosition + " entityValue = "+ auto );
        }



        int index = 0;

        foreach (var item in correctGuees)
        {
            Debug.Log(correctGuees[index] + " Compare With "+ conversion[index]);
            if (correctGuees[index] == conversion[index])
           {
               Debug.Log("OK");
           }
           else
            {
                return false;
            }
             index++;

        }

        return true;



    }

    //private void SwapIterate (Spawn.objectCollection obj)
    //{
    //    foreach (var item in ObjectNumber.sObjectCollectionsMember)
    //    {
    //        if (obj.entityID == item.entityID)
    //        {

    //        }
    //    }


    void Timer()
    {
        timer += 0.1f;
    }



}

