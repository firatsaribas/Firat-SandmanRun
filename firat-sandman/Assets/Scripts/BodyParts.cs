using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BodyParts : MonoBehaviour
{
    public bool isLeg;
    [HideInInspector]
    public bool partIsFull;
    private int initialPartCount,currentCount;
    public List<GameObject> partList;
    public List<Transform> openTransformList;
    private float offsetTimer;

    void Start()
    {
        offsetTimer = 0.01f;
        partList = new List<GameObject>();
        openTransformList = new List<Transform>();

        //collects the existing balls in the model, add them to the part list
        foreach(Transform trns in GetComponentsInChildren<Transform>())
        {
            if (trns.gameObject.CompareTag("Part"))
            {
                partList.Add(trns.gameObject);
            }
        }
        initialPartCount = partList.Count;
        currentCount = initialPartCount;
        partIsFull = true;
    }

    //move coroutine to the given position
    public IEnumerator MoveToPoint(Transform target,GameObject newPart)
    {
        float moveDuration = 1f;
        float time = 0;
        while (time < moveDuration)
        {
            time += Time.deltaTime;
            newPart.transform.position = Vector3.Lerp(newPart.transform.position, target.position, time / moveDuration);
            yield return null;
        }
        newPart.transform.SetParent(transform);
    }

    //gives the initial jump effect of the collected part and sends it to the empty position
    public void SetNewPart(GameObject newPart)
    {
        if(openTransformList.Count > 0 && !partIsFull)
        {
            offsetTimer += 0.01f;
            newPart.transform.DOJump(Random.insideUnitSphere, 1f, 1, .5f+offsetTimer).SetRelative().SetEase(Ease.Linear).OnComplete(() =>
               {
                   StartCoroutine(MoveToPoint(openTransformList[openTransformList.Count - 1], newPart));
                               openTransformList.Remove(openTransformList[openTransformList.Count - 1]);


               });
            //newPart.transform.DOLocalMove(openTransformList[openTransformList.Count-1].position, .5f).OnComplete(() => { /*newPart.transform.SetParent(gameObject.transform);*/ });
            currentCount++;



            partList.Add(newPart);
            if(currentCount >= initialPartCount)
            {
                partIsFull = true;
            }
        }
    }


    //removes the part that is given, removes it from the partlist, adds the position to the open-transform list for the possible new comers
    public void RemovePart(GameObject removedPart)
    {
        if (partList.Contains(removedPart))
        {
            partList.Remove(removedPart);
            openTransformList.Add(removedPart.transform);
            removedPart.SetActive(false);
            currentCount--;
        }
        if (currentCount < initialPartCount)
        {
            partIsFull = false;
        }
        BodyController.instance.CheckForAnims();
    }
  
}
