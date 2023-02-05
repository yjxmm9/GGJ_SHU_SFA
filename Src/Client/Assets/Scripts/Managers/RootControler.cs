using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootControler : MonoSingleton<RootControler>
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

    public bool poisonned = false;

    public float depth = 0;
    Vector3[] startPositions;

    protected override void OnStart()
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
        CapacityManager.Instance.Init();
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
    }

    private void UpdateHealth()
    {
        gM.timer += Time.deltaTime;
        if (poisonned)
        {
            health -= health / 5;
            poisonned = false;
        }
        var upgradeWater = CapacityManager.Instance.GetSkillPoint(2);
        health -= Time.deltaTime * upgradeWater;
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
            Water water = hit.collider.GetComponent<Water>();
            if (water != null)
            {
                water.Drink();
                health = Mathf.Min(health + water.waterAmount, maxHealth);
                Move();
                Vector3 oxygenPosition = root.GetPosition(root.positionCount - 1) + new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-0.5f, 0.5f), 0.08f);
                Instantiate(oxygenPrefab, oxygenPosition, oxygenPrefab.transform.rotation);
                AudioManager.Instance.PlayDrinkSFX();
            }

            Organic organic = hit.collider.GetComponent<Organic>();
            if (organic != null)
            {
                organic.Eat();
                Move();
                Vector3 oxygenPosition = root.GetPosition(root.positionCount - 1) + new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-0.5f, 0.5f), 0.08f);
                Instantiate(oxygenPrefab, oxygenPosition, oxygenPrefab.transform.rotation);
                CapacityManager.Instance.AddCash();
                AudioManager.Instance.OrganicSFX();
            }

            Poison poison = hit.collider.GetComponent<Poison>();
            if (poison != null)
            {
                //gM.sfx.HurtSfx();
                poison.Drink();
                poisonned = true;
                Move();
                AudioManager.Instance.PoisonSFX();
            }

            EndTrigger endTrigger = hit.collider.GetComponent<EndTrigger>();
            if (endTrigger != null)
            {
                AudioManager.Instance.VictorSFX();
                RootControler.Instance.dead = true;
                gM.cameraControler.End();
            }


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
            new GradientColorKey(!poisonned?rootWater:rootHurt, (index-health*2f)/index),
            new GradientColorKey(!poisonned?rootWater:rootHurt, (index-health*2f)/index + 1f/index),
            new GradientColorKey(!poisonned?rootWater:rootHurt, 1.0f) },
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
        var upgradeMoveVer = CapacityManager.Instance.GetSkillPoint(1);
        if (direction.normalized.y < -upgradeMoveVer)
        {
            currentTimeUntilNewRootPoint -= Time.fixedDeltaTime;
        }
        else
        {
            currentTimeUntilNewRootPoint -= Time.fixedDeltaTime * (Mathf.Max(0, (-direction.normalized.y + upgradeMoveVer)));
        }

        if (currentTimeUntilNewRootPoint <= 0 && Vector3.Distance(rootTarget.position, lastPosition) > rootTipLenght * 5f)
        {
            root.positionCount = root.positionCount + 1;
            rootPointIndex++;
            var upgradeOxygen = CapacityManager.Instance.GetSkillPoint(3);
            //Debug.Log(upgradeOxygen);
            if (rootPointIndex % (int)(40f / upgradeOxygen + 3f) == 0)
            {
                Vector3 oxygenPosition = lastPosition + new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(0, 1f), 0.08f);
                Instantiate(oxygenPrefab, oxygenPosition, oxygenPrefab.transform.rotation);
            }
            var upgradeSpeed = CapacityManager.Instance.GetSkillPoint(0);
            //Debug.Log(upgradeSpeed);
            currentTimeUntilNewRootPoint = timeUntilNewRootPoint * upgradeSpeed;
            UpdateRootTip();
            //gM.sfx.PlayGrowingSFX();
        }
    }
}
