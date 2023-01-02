﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportController
{

    internal void Teleport(GameObject player, GameObject teleportObject)
    {
        Collider2D teleportColliders = teleportObject.GetComponent<Collider2D>();
        if (teleportColliders.bounds.extents.x > teleportColliders.bounds.extents.y)
        {
            GateTeleportHorizontal(player, teleportObject);
        }
        else
        {
            GateTeleportVertical(player, teleportObject);
        }

    }

    private static void GateTeleportHorizontal(GameObject player, GameObject teleportObject)
    {
        if (Mathf.Abs(player.transform.position.y - teleportObject.transform.position.y) > 1.5f)
        {
            if (player.transform.position.y > teleportObject.transform.position.y)
            {
                if (teleportObject.transform.childCount > 2)
                {
                    if (player.transform.position.x < teleportObject.transform.position.x)
                    {
                        player.transform.position = teleportObject.transform.GetChild(1).position;
                    }
                    else
                    {
                        player.transform.position = teleportObject.transform.GetChild(3).position;
                    }
                }
                else
                {
                    player.transform.position = teleportObject.transform.GetChild(1).position;
                }
            }
            else
            {
                if (teleportObject.transform.childCount > 2)
                {
                    if (player.transform.position.x < teleportObject.transform.position.x)
                    {
                        player.transform.position = teleportObject.transform.GetChild(0).position;
                    }
                    else
                    {
                        player.transform.position = teleportObject.transform.GetChild(2).position;
                    }
                }
                else
                {
                    player.transform.position = teleportObject.transform.GetChild(0).position;
                }
            }
        }
    }

    private static void GateTeleportVertical(GameObject player, GameObject teleportObject)
    {
        if (Mathf.Abs(player.transform.position.x - teleportObject.transform.position.x) > 1.5f)
        {
            if (player.transform.position.x > teleportObject.transform.position.x)
            {
                if (teleportObject.transform.childCount > 2)
                {
                    if (player.transform.position.y < teleportObject.transform.position.y)
                    {
                        player.transform.position = teleportObject.transform.GetChild(0).position;
                        //Debug.Log("otdqsno nalqvo - dolu");
                    }
                    else
                    {
                        player.transform.position = teleportObject.transform.GetChild(2).position;
                        //Debug.Log("otdqsno nalqvo - gore");
                    }
                }
                else
                {
                    player.transform.position = teleportObject.transform.GetChild(0).position;

                }
            }
            else
            {
                if (teleportObject.transform.childCount > 2)
                {
                    if (player.transform.position.y < teleportObject.transform.position.y)
                    {
                        player.transform.position = teleportObject.transform.GetChild(1).position;
                        //Debug.Log("otdqsno nalqvo - dolu dasdasdsa");

                    }
                    else
                    {
                        player.transform.position = teleportObject.transform.GetChild(3).position;
                    }
                }
                else
                {
                    player.transform.position = teleportObject.transform.GetChild(1).position;
                    //Debug.Log("otlqvo nadqsno - dolu");

                }
            }
        }
    }

    internal void ChangeFrameCamera(GameObject player, GameObject[] cameras, GameObject teleportObject)
    {
        string[] name = teleportObject.name.Split(new char[] { '-', '>' }, System.StringSplitOptions.RemoveEmptyEntries);

        string firstFrameIndex = name[0];
        string secondFrameIndex = name[name.Length - 1];
        //Debug.Log(firstFrameIndex + " - " + secondFrameIndex);
        Collider2D teleportCollider = teleportObject.GetComponent<Collider2D>();

        int angleValue = 0;

        if (teleportCollider.bounds.extents.x > teleportCollider.bounds.extents.y)
        {
            angleValue = CalculateAngle(player, teleportObject, player.transform.right);
        }
        else
        {
            angleValue = CalculateAngle(player, teleportObject, player.transform.up);

        }

        if (teleportCollider.bounds.extents.x < teleportCollider.bounds.extents.y)
        {
            if (angleValue < 0)
            {
                cameras[int.Parse(secondFrameIndex)].SetActive(false);
                cameras[int.Parse(firstFrameIndex)].SetActive(true);
                //Debug.Log("preminavame ot 1 kym 0 frame");
            }
            else
            {
                cameras[int.Parse(firstFrameIndex)].SetActive(false);
                cameras[int.Parse(secondFrameIndex)].SetActive(true);
                //Debug.Log("preminavame ot 0 kym 1 frame");
            }
        }
        else if(teleportCollider.bounds.extents.x > teleportCollider.bounds.extents.y)
        {
            if (angleValue < 0)
            {
                cameras[int.Parse(secondFrameIndex)].SetActive(false);
                cameras[int.Parse(firstFrameIndex)].SetActive(true);
            }
            else
            {
                cameras[int.Parse(firstFrameIndex)].SetActive(false);
                cameras[int.Parse(secondFrameIndex)].SetActive(true);
            }
        }
       
    }

    private int CalculateAngle(GameObject player, GameObject teleportObject, Vector3 pForward)
    {
        //Vector3 pFrwd = player.transform.up;
        Vector3 rDir = teleportObject.transform.position - player.transform.position;

        float dot = pForward.x * rDir.x + pForward.y * rDir.y;
        float angle = Mathf.Acos(dot / (pForward.magnitude * rDir.magnitude));

        //Debug.Log("Angle: " + angle * Mathf.Rad2Deg);
        //Debug.Log("Unity angle: " + Vector3.Angle(pFrwd, rDir));

        Debug.DrawRay(player.transform.position, pForward * 15, Color.green, 2);
        Debug.DrawRay(player.transform.position, rDir, Color.red, 2);

        int clockWise = 1;
        if (CrossProduct(pForward, rDir).z < 0)
        {
            clockWise = -1;
        }

        //Unity calculation on the angle
        //float unityAngle = Vector3.SignedAngle(pFrwd, rDir, this.transform.forward);
        //Debug.Log("forward: " + this.transform.forward);

        //this.transform.Rotate(0, 0, unityAngle * 0.02f);

        //this.transform.Rotate(0, 0, angle * Mathf.Rad2Deg * clockWise);

        return clockWise;
    }

    private Vector3 CrossProduct(Vector3 v, Vector3 w)
    {
        float xMult = v.y * w.z - v.z * w.y;
        float yMult = v.z * w.x - v.x * w.z;
        float zMult = v.x * w.y - v.y * w.x;

        Vector3 crossProd = new Vector3(xMult, yMult, zMult);
        return crossProd;
    }
}
