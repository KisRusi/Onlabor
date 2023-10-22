using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Turret : MonoBehaviour
{
    
    public GameObject prefab;
    BuildingManager buildingManager;
    private float coolDown;
    private float attackTime;
    private State currentState;
    [SerializeField]
    private Material readyMaterial;
    private List<RtsUnit> enemies;
    private RtsUnit targetUnit;

    public enum State
    {
        Idle,
        Building,
        Ready,
        Attacking
    }


    private void Awake()
    {
        buildingManager = GameObject.Find("BuildingManager").GetComponent<BuildingManager>();
        coolDown = 5f;
        enemies = new List<RtsUnit>();
    }

    private void Update()
    {
        switch (currentState)
        {
            case State.Idle:
                AutomaticAttackInArea(transform.position, 5f);
                break;
            case State.Building:
                GetComponentInChildren<CanvasScaler>(true).gameObject.SetActive(true);
                break;
            case State.Ready:
                gameObject.GetComponent<MeshRenderer>().material = readyMaterial;
                SwitchState(State.Idle);
                break;
            case State.Attacking:
                if (enemies.Count == 0)
                    currentState = State.Idle;
                attackTime -= Time.deltaTime;
                float attackTimerMax = 1f;
                if (targetUnit.IsDead())
                {
                    AutomaticAttackInArea(transform.position, 3f);
                }
                if (attackTime < 0)
                {
                    attackTime += attackTimerMax;
                    targetUnit.Damage(20);
                }
                break;
        }
    }

    public float GetCoolDown()
    {
        return coolDown;
    }

    public void SwitchState(State state)
    {
        currentState = state;
    }

    public State GetState()
    {
        return currentState;
    }

    public void AutomaticAttackInArea(Vector3 position, float radius)
    {
        enemies.Clear();
        enemies = CheckForEnemeis(position, radius);
        var count = enemies.Count;
        var random = UnityEngine.Random.Range(0, count);
        if (count > 0)
        {
            targetUnit = (enemies[random].transform.GetComponent<RtsUnit>());
            currentState = State.Attacking;
        }

    }
    public List<RtsUnit> CheckForEnemeis(Vector3 position, float radius)
    {
        var colliders = Physics.OverlapSphere(position, radius);
        foreach (var collider in colliders)
        {
            if (collider.transform.gameObject.name.Contains("Enemy") && !enemies.Contains(collider.gameObject.GetComponent<RtsUnit>()))
                enemies.Add(collider.transform.gameObject.GetComponent<RtsUnit>());
        }
        return enemies;
    }
}
