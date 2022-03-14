using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyController : MonoBehaviour
{
    private Animator animator;
    public static BodyController instance;
    [SerializeField]
    private List<BodyParts> bodyPartsList;
    private float timerOffset;
    private void Awake()
    {
        instance = this;
        timerOffset = 0.0f;
    }
    private void Start()
    {
        animator = GetComponent<Animator>();
    }


    // Places new parts to the "not full" body parts with respect to the hierarchy in the bodypartList (some parts could are more important) 
    public IEnumerator PlaceNewPart(GameObject newObj)
    {
        timerOffset += 0.01f;
        yield return new WaitForSeconds(timerOffset);
        for(int i = 0; i < bodyPartsList.Count; i++)
        {
            BodyParts part = bodyPartsList[i];
            if (!part.partIsFull)
            {
                part.SetNewPart(newObj);
                break;
            }
        }
        yield return new WaitForSeconds(1f);
        timerOffset = 0;
        CheckForAnims();

    }


    //checks the situation of the leg parts and triggers animation bools
    //if both legs are damged crawl, if one leg damaged onlt jumps 
    public void CheckForAnims()
    {
        int howManyLegMissing=0;
        foreach(BodyParts part in bodyPartsList)
        {
            if (part.isLeg && part.partList.Count <= part.openTransformList.Count)
            {
                howManyLegMissing++;
            }
        }
        if(howManyLegMissing == 1)
        {
            animator.SetBool("jump",true);
            animator.SetBool("run",false);
            animator.SetBool("crawl",false);
        }
        else if(howManyLegMissing == 2)
        {
            animator.SetBool("jump", false);
            animator.SetBool("run", false);
            animator.SetBool("crawl", true);
        }
        else
        {
            animator.SetBool("jump", false);
            animator.SetBool("run", true);
            animator.SetBool("crawl", false);
        }
    }
   
}
