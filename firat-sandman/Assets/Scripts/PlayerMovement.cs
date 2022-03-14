using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;


public class PlayerMovement : MonoBehaviour
{
    private Vector3 firstPos;
    private Vector3 finalPos;
    private Vector3 dif;
    public float laneSpeed;
    public GameObject playerChild;
    public GameObject posParent;

    public float playerSpeed;
    public float turnAmount;
    public float clampAmount = 5f;

    private bool pressing;
    public bool canMove = false;
    public bool canXChange = false;

    public static PlayerMovement instance;

    private void Awake()
    {

        instance = this;


    }

   


    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerSpeed += 5;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (canMove)
        {
            //formaward movement of the player parent
            transform.Translate(Vector3.forward * Time.deltaTime * playerSpeed);
            if (canXChange)
            {
                PositionInput();
            }

        }

    }

    public void StartGame()
    {
        canMove = true;
        canXChange = true;
    }


    //swerf movement
    private void PositionInput()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
           
            pressing = true;
            Vector3 pos = Input.mousePosition;
            pos.z = 10;
            firstPos = Camera.main.ScreenToViewportPoint(pos);
        }
        else if (Input.GetMouseButton(0) && pressing && canXChange)
        {
            Vector3 pos = Input.mousePosition;
            pos.z = 10;
            finalPos = Camera.main.ScreenToViewportPoint(pos);
            Vector3 change = finalPos - firstPos;
            dif += new Vector3(change.x, 0, 0) * Time.deltaTime * laneSpeed;
            dif.x = Mathf.Clamp(dif.x, -clampAmount, clampAmount);
            firstPos = finalPos;
            posParent.transform.localPosition = dif;

            Vector3 movement = new Vector3(change.x * turnAmount, 0, playerSpeed * Time.deltaTime);
            playerChild.transform.rotation = Quaternion.Lerp(playerChild.transform.rotation, Quaternion.LookRotation(movement), Time.deltaTime * 5);
        }

        else if (Input.GetMouseButtonUp(0))
        {
            pressing = false;
            posParent.transform.DORotate(Vector3.zero, .2f);
        }

    }

    
}
