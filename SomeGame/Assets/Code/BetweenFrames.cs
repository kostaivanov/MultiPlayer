﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetweenFrames : MonoBehaviour
{
    [SerializeField] private GameObject[] virtualCams;
    [SerializeField] private FadeInOut fadeInOutScript;

    private TeleportController teleportController;
    private GameObject player;

    internal bool finishedFading = false;
    private bool triggered = false;
    private GameObject currentTeleport;

    // Start is called before the first frame update
    void Start()
    {
        fadeInOutScript.whiteFade.canvasRenderer.SetAlpha(0.0f);
        teleportController = new TeleportController();
        virtualCams[1].SetActive(false);
        virtualCams[2].SetActive(false);
        virtualCams[3].SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(finishedFading);

        if (currentTeleport != null && fadeInOutScript.whiteFade.canvasRenderer.GetAlpha() == 1 && finishedFading == false && currentTeleport.name == this.gameObject.name)
        {
            currentTeleport = null;
            if (player != null)
            {
                teleportController.Teleport(player, this.gameObject);
            }

            finishedFading = true;

            StartCoroutine(WaitBeforeStartTransparent());
        }

        else if (fadeInOutScript.whiteFade.canvasRenderer.GetAlpha() == 0 && finishedFading == true)
        {
            finishedFading = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D otherObject)
    {
        if (otherObject.tag == "Player" && triggered == false)
        {
            currentTeleport = this.gameObject;
            player = otherObject.gameObject;
            //player.GetComponent<PlayerMovement>().enabled = false;
            if (player != null)
            {
                player.GetComponent<PlayerMovement>().CanMove = false;
            }
            //player.GetComponent<WallLocator>().movingBetweenFrames = true;
            fadeInOutScript.whiteFade.enabled = true;
            fadeInOutScript.BecomeDark();
            triggered = true;
        }
    }

    private void OnTriggerStay2D(Collider2D otherObject)
    {
        if (otherObject.tag == "Player" && triggered == true)
        {
            triggered = false;
        }
    }

    private IEnumerator WaitBeforeStartTransparent()
    {
        yield return new WaitForSecondsRealtime(0.8f);
        if (player != null)
        {
            teleportController.ChangeFrameCamera(player, virtualCams, this.gameObject);
        }
        fadeInOutScript.BecomeTransparent();
        //player.GetComponent<PlayerMovement>().enabled = true;
        if (player != null)
        {
            player.GetComponent<PlayerMovement>().CanMove = true;
            //player.GetComponent<WallLocator>().movingBetweenFrames = false;

        }

        finishedFading = false;
    }
}
