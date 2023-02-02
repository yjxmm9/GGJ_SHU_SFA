﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootControler : MonoBehaviour
{
    public GM gM;
    public float health = 10f;
    public float maxHealth = 10f;
    public LineRenderer root;
    public GameObject oxygenPrefab;
    public Camera mainCamera;
    public Transform rootTarget;
    public float rootTipLenght;
    public float timeUntilNewRootPoint;
    public float currentTimeUntilNewRootPoint;
    public int rootPointIndex = 1;

    public LayerMask raycastMask;

    public bool dead = true;
    public bool growing = false;
    public List<GameObject> smallRoots;

    public Color rootBase, rootTip, rootWater, rootEnd, rootHurt, rootMagic;

    public float poisonned = 0f;

    public float depth = 0;
    Vector3[] startPositions;
    private void Start()
    {
        currentTimeUntilNewRootPoint = timeUntilNewRootPoint;
        health = maxHealth;
        startPositions = new Vector3[] { root.GetPosition(0), root.GetPosition(1), root.GetPosition(2) };
    }

    public void StartGame()
    {
        dead = false;
        growing = false;
        currentTimeUntilNewRootPoint = timeUntilNewRootPoint + 1f;
        health = maxHealth;
        root.positionCount = 3;
        rootPointIndex = 2;
        depth = 0;
        root.SetPositions(startPositions);
        //gM.ui.HideDeathUI();
        gM.timer = 0;
        foreach (GameObject smallRoot in smallRoots)
        {
            Destroy(smallRoot);
        }
        smallRoots.Clear();
    }

    void FixedUpdate()
    {
        if (dead) { return; }
        UpdateRootTarget();
        UpdateRoot();
        UpdateRootWidth();
    }

    private void Update()
    {
        if (dead || !growing) { return; }
        UpdateHealth();
        ProduceOxygen();
    }

    private void ProduceOxygen()
    {
        
    }

    private void UpdateHealth()
    {
        gM.timer += Time.deltaTime;
        if (poisonned > 0f)
        {
            //health -= Time.deltaTime * gM.CapStrength;
            health -= Time.deltaTime * 1;
            poisonned -= Time.deltaTime;
        }
        //health -= Time.deltaTime * gM.CapWater;
        health -= Time.deltaTime * 1.5f;
        if (health <= 0 && !dead)
        {
            dead = true;
            gM.cameraControler.Death();
        }
    }

    void UpdateRootTarget()
    {
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        worldPosition = new Vector3(worldPosition.x, worldPosition.y, 0);

        rootTarget.position = worldPosition;
    }


    void UpdateRoot()
    {
        UpdateRootTip();

        Vector3 lastPosition = root.GetPosition(rootPointIndex - 1);
        Vector3 direction = rootTarget.position - lastPosition;
        direction = direction.normalized * rootTipLenght;

        // check if on rock
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(lastPosition, direction, out hit, rootTipLenght, raycastMask))
        {
            //if (hit.collider.tag == "Core")
            //{
            //    dead = true;
            //    if (gM.timer < gM.maxTime || !gM.newGamePlus)
            //    {
            //        gM.maxTime = gM.timer;
            //        PlayerPrefs.SetFloat("MaxTime", gM.maxTime);
            //    }
            //    gM.cameraControler.End();
            //    return;
            //}
            Water water = hit.collider.GetComponent<Water>();
            if (water != null)
            {
                water.Drink();
                //gM.sfx.PlayDrinkSFX();
                health = Mathf.Min(health + water.waterAmount, maxHealth);
                Move();
                Vector3 oxygenPosition = root.GetPosition(root.positionCount - 1) + new Vector3(UnityEngine.Random.Range(-0.5f, 0.5f), UnityEngine.Random.Range(-0.5f, 0.5f), -0.02f);
                Instantiate(oxygenPrefab, oxygenPosition, transform.rotation);
                //oxygen.Initialize(root.GetPosition(root.positionCount - 1), gM, 0.5f);
                //smallRoots.Add(smallRoot.gameObject);
            }

            //Poison poison = hit.collider.GetComponent<Poison>();
            //if (poison != null)
            //{
            //    gM.sfx.HurtSfx();
            //    poisonned = Mathf.Min(4f, poisonned + 1f);
            //    Move();
            //}


        }
        else
        {
            Move();
        }

    }

    void UpdateRootTip()
    {
        Vector3 lastPosition = root.GetPosition(rootPointIndex - 1);
        Vector3 direction = rootTarget.position - lastPosition;
        direction = direction.normalized * rootTipLenght;
        root.SetPosition(rootPointIndex, lastPosition + direction);
    }

    void UpdateRootWidth()
    {
        float rootIndex = (float)rootPointIndex;
        AnimationCurve curve = new AnimationCurve();
        curve.AddKey(0, 1);
        curve.AddKey((rootIndex - 7f) / rootIndex, 0.9f);
        curve.AddKey((rootIndex - 6f) / rootIndex, 0.9f);
        curve.AddKey((rootIndex) / rootIndex, 0.2f);
        curve.AddKey(1, 0);
        curve.SmoothTangents(1, 1f);
        curve.SmoothTangents(2, 1f);
        root.widthCurve = curve;
        root.widthMultiplier = Mathf.Min(0.2f, root.positionCount * 0.0002f + 0.1f);





        float index = (float)rootPointIndex;
        float alpha = 1.0f;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(rootBase, 0.0f),
            new GradientColorKey(rootBase, (index-health*2f)/index - 1f/index),
            new GradientColorKey(poisonned <= 0?rootWater:rootHurt, (index-health*2f)/index),
            new GradientColorKey(poisonned <= 0?rootWater:rootHurt, (index-health*2f)/index + 1f/index),
            new GradientColorKey(poisonned <= 0?rootWater:rootHurt, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
        );
        root.colorGradient = gradient;
    }

    void Move()
    {
        growing = true;
        Vector3 lastPosition = root.GetPosition(rootPointIndex - 1);
        if (lastPosition.y < depth) { depth = lastPosition.y; }
        Vector3 direction = rootTarget.position - lastPosition;
        //if (direction.normalized.y < -gM.CapUpward)
        if (direction.normalized.y < -0.2f)
        {
            currentTimeUntilNewRootPoint -= Time.fixedDeltaTime;
        }
        else
        {
            //currentTimeUntilNewRootPoint -= Time.fixedDeltaTime * (Mathf.Max(0, (-direction.normalized.y + gM.CapUpward)));
            currentTimeUntilNewRootPoint -= Time.fixedDeltaTime * (Mathf.Max(0, (-direction.normalized.y + 0.2f)));
        }

        if (currentTimeUntilNewRootPoint <= 0 && Vector3.Distance(rootTarget.position, lastPosition) > rootTipLenght * 5f)
        {
            root.positionCount = root.positionCount + 1;
            rootPointIndex++;
            //if (rootPointIndex % (int)(40f / gM.CapRoot + 3f) == 0)
            //{
            //    SmallRoot smallRoot = Instantiate(smallRootPrefab, transform).GetComponent<SmallRoot>();
            //    smallRoot.Initialize(direction, root.GetPosition(root.positionCount - 3), gM, 0.3f + gM.CapRoot / 5f);
            //    smallRoots.Add(smallRoot.gameObject);

            //}
            //currentTimeUntilNewRootPoint = timeUntilNewRootPoint * gM.CapSpeed;
            currentTimeUntilNewRootPoint = timeUntilNewRootPoint * 1.1f;
            UpdateRootTip();
            //gM.sfx.PlayGrowingSFX();
        }
    }
}
