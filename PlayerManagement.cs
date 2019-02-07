﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManagement : MonoBehaviour
{
    public GameObject player;
    int totalSizePucks;
    private List<GameObject> playerList = new List<GameObject>();
    private bool hasStarted = false;
    bool isMoved = false;
    public float moveSpeed = 100f;
    public float turnSpeed = -1000f;

    Player pickedPlayer;
    int pickedPlayerIndex = 0;
    // Start is called before the first frame update
    void Start()
    {

        totalSizePucks = Random.Range(2, 4);
        for (int i=0; i<totalSizePucks; i++)
        {
            Vector3 vector3 = new Vector3(transform.position.x + Random.Range(-2.21f, 2.21f), transform.position.y + Random.Range(-1.7f, -0.7f), transform.position.z);
            playerList.Add(Instantiate(player, vector3, transform.rotation) as GameObject) ;

        }
        ChoosePlayer();
    }
    private void ChoosePlayer()
    {
        for(int i=0; i<playerList.Count; i++)
        {
            if (i == 0)
            {
                pickedPlayer = playerList[i].GetComponent<Player>();
                pickedPlayerIndex = 0;

            }

            if (i + 1 != playerList.Count)
            {
                if (playerList[i+1].GetComponent<Player>().transform.position.y >= pickedPlayer.transform.position.y)
                {
                    pickedPlayer = playerList[i+1].GetComponent<Player>();
                    pickedPlayerIndex = i + 1;
                } else
                {
                    pickedPlayer = playerList[i].GetComponent<Player>();
                    pickedPlayerIndex = i;

                }
            }
            
        }
        for (int i = 0; i < playerList.Count; i++)
        {
            if (playerList[i].GetComponent<Player>() != pickedPlayer)
            {
                playerList[i].GetComponent<Player>().arrow.SetActive(false);
            }
        }

    }


    private void PickNewPlayer()
    {
        int i = 0;
        bool isEntered = false;
        while (i < playerList.Count)
        {
            if (!isEntered)
            {

                if (i == pickedPlayerIndex)
                {
                    playerList[i].GetComponent<Player>().arrow.SetActive(false);

                    if (i == playerList.Count - 1)
                    {
                        pickedPlayerIndex = 0;
                        pickedPlayer = playerList[pickedPlayerIndex].GetComponent<Player>();
                        pickedPlayer.arrow.SetActive(true);
                    }
                    else
                    {
                        pickedPlayerIndex = i + 1;
                        pickedPlayer = playerList[pickedPlayerIndex].GetComponent<Player>();
                        pickedPlayer.arrow.SetActive(true);
                    }
                    isEntered = true;

                }
                else
                {
                    playerList[i].GetComponent<Player>().arrow.SetActive(false);

                }
            
        }
            i++;

        }
    }
    


    // Update is called once per frame
    void Update()
    {
        LaunchOnMouseClick(pickedPlayer);

        if (pickedPlayer.arrow.activeSelf)
        {
            TouchControl(pickedPlayer);
        }
    }


    private void LaunchOnMouseClick(Player secilmisPlayer)
    {
        // Mouse click 
        if (Input.GetButtonDown("Submit"))
        {
            if (!hasStarted)
            {
                
                float fRotation = secilmisPlayer.rb.rotation * Mathf.Deg2Rad;
                float fX = Mathf.Sin(fRotation);
                float fY = Mathf.Cos(fRotation);
                Vector2 v2 = new Vector2(fY * moveSpeed, fX * moveSpeed);
                Debug.Log("the vector2:" +v2);

                secilmisPlayer.rb.velocity = v2;
                Debug.Log("secilmisPlayer rb velocity:" + secilmisPlayer.rb.velocity);

                secilmisPlayer.arrow.SetActive(false);
                hasStarted = true;
            }
            else
            {
                secilmisPlayer.rb.velocity = new Vector2(0, 0);
                secilmisPlayer.arrow.SetActive(true);
                hasStarted = false;
            }

        }
        // Touch
        if (Input.touchCount > 0 && (Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(0).phase == TouchPhase.Canceled))
        {
            if (!hasStarted)
            {
                if (!isMoved)
                {
                    float fRotation = secilmisPlayer.rb.rotation * Mathf.Deg2Rad;
                    float fX = Mathf.Sin(fRotation);
                    float fY = Mathf.Cos(fRotation);
                    Vector2 v2 = new Vector2(fY * 10, fX * 10);
                    Debug.Log("the vector2:" + v2);

                    secilmisPlayer.rb.velocity = v2;
                    Debug.Log("secilmisPlayer rb velocity:" + secilmisPlayer.rb.velocity);

                    secilmisPlayer.arrow.SetActive(false);

                    hasStarted = true;
                    isMoved = true;

                    PickNewPlayer();
                }
                else
                {
                    isMoved = false;
                }

            }
            else
            {
                if (pickedPlayer.arrow.activeSelf)
                {
                    float fRotation = secilmisPlayer.rb.rotation * Mathf.Deg2Rad;
                    float fX = Mathf.Sin(fRotation);
                    float fY = Mathf.Cos(fRotation);
                    Vector2 v2 = new Vector2(fY * 10, fX * 10);
                    Debug.Log("the vector2:" + v2);

                    secilmisPlayer.rb.velocity = v2;
                    Debug.Log("secilmisPlayer rb velocity:" + secilmisPlayer.rb.velocity);

                    secilmisPlayer.arrow.SetActive(false);
                    PickNewPlayer();

                }
                else
                {
                    secilmisPlayer.rb.velocity = new Vector2(0, 0);
                    secilmisPlayer.arrow.SetActive(true);
                }
              
                hasStarted = false;
            }
        }

    }
    private void TouchControl(Player secilmisPlayer)
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            var touchDeltaPosition = Input.GetTouch(0).deltaPosition;
            //Debug.Log(Mathf.Clamp(touchDeltaPosition.x, -1, 1));
            //   transform.Translate(-touchDeltaPosition.x * Time.deltaTime * moveSpeed,
            //            -touchDeltaPosition.y * Time.deltaTime * moveSpeed, 0);
            //  var xpos = Mathf.Clamp(transform.position.x + ((touchDeltaPosition.x * Time.deltaTime * moveSpeed) / 10), xMin, xMax);
            //  var ypos = Mathf.Clamp(transform.position.y + ((touchDeltaPosition.y * Time.deltaTime * moveSpeed) / 10), yMin, yMax);
            //  transform.position = new Vector2(xpos, ypos);
            // Debug.Log(Mathf.Clamp(touchDeltaPosition.x, -1, 1));
            secilmisPlayer.transform.Rotate(0, 0, Mathf.Clamp(touchDeltaPosition.x, -1, 1) * turnSpeed * Time.deltaTime);
            isMoved = true;
        }
        //   Debug.Log(Input.GetAxis("Horizontal"));

        // transform.Rotate(0, 0, Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime);




        // rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);
        //  transform.Rotate(Vector3.forward,0,Input.GetAxis("Horizontal")*speed*Time.deltaTime);

    }
    
}