using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyType type;
    public Transform target;
    public Quadrant myQuad;
    private float damage;
    private float hp = 100.0f;
    private float walkSpeed;
    private Vector3[] currentPath;
    private bool madePathRequest;
    private bool reachedPylon;

    void Start()
    {
        GameManager.manager.restartGame.AddListener(Restart);
        InitType();
        madePathRequest = false;
        MakePathRequest();
    }
    private void InitType()
    {
        if(type == EnemyType.Hunter)
        {
            damage = GameManager.manager.enemyManager.hunterDamage;
            walkSpeed = GameManager.manager.enemyManager.hunterSpeed;
            GameManager.manager.quadrantChange.AddListener(PlayerChangedQuads);
        }
        else
        {
            damage = GameManager.manager.enemyManager.zergDamage;
            walkSpeed = GameManager.manager.enemyManager.zergSpeed;
        }
    }
    public void SetTarget(Transform targetInit)
    {
        target = targetInit;
        if(type == EnemyType.Hunter)
        {
            GameManager.manager.getNewPaths.AddListener(MakePathRequest);
        }
    }
    public void MakePathRequest()
    {
        if(type == EnemyType.Zergling)
        {
            if(reachedPylon && gameObject.activeSelf)
            {
                HitPylon(target.gameObject.GetComponent<Pylon>());
            }
        }
        if (!madePathRequest)
        {
            madePathRequest = true;
            GameManager.manager.pathManager.RequestPath(transform.position, target.position, ReceivedPath);
        }
    }
    public void HitPylon(Pylon pylon)
    {
        if(type == EnemyType.Zergling)
        {
            pylon.TakeDamage(damage);
            Die();
        }
    }
    public void HitPlayer(Player player)
    {
        player.TakeDamage(damage);
    }
    public void TakeDamage(float damage)
    {
        hp -= damage;
        if(hp <= 0.0f)
        {
            Die();
        }
    }
    private void ReceivedPath(Vector3[] path, bool success)
    {
        madePathRequest = false;
        if(success && gameObject.activeSelf)
        {
            currentPath = path;
            StopCoroutine("Move");
            StartCoroutine("Move");
        }
        else
        {
            MakePathRequest();
        }
    }
    IEnumerator Move()
    {
        int currentGoal = 0;
        while(currentGoal < currentPath.Length)
        {
            if(!GameManager.manager.IsNotPaused())
            {
                yield return new WaitUntil(GameManager.manager.IsNotPaused);
            }
            transform.position = Vector3.MoveTowards(transform.position, currentPath[currentGoal], walkSpeed * Time.deltaTime);
            if(transform.position == currentPath[currentGoal])
            {
                currentGoal++;
            }
            yield return null;
        }
        if(type == EnemyType.Zergling)
        {
            reachedPylon = true;
        }
        MakePathRequest();
    }
    private void PlayerChangedQuads()
    {
        if (myQuad.currentQuad)
        {
            MakePathRequest();
        }
        else
        {
            StopCoroutine(Move());
            //create patrol pathing
        }
    }
    private void Die()
    {
        StopCoroutine("Move");
        if (type == EnemyType.Hunter)
        {
            GameManager.manager.getNewPaths.RemoveListener(MakePathRequest);
            GameManager.manager.quadrantChange.RemoveListener(PlayerChangedQuads);
            GameManager.manager.enemyManager.HunterDied();
        }
        else
        {
            GameManager.manager.enemyManager.ZergDied();
        }
        gameObject.SetActive(false);
    }
    private void Restart()
    {
        StopAllCoroutines();
        GameObject.Destroy(gameObject);
    }
    private void OnDestroy()
    {
        StopCoroutine("Move");
        if (type == EnemyType.Hunter)
        {
            GameManager.manager.getNewPaths.RemoveListener(MakePathRequest);
            GameManager.manager.quadrantChange.RemoveListener(PlayerChangedQuads);
        }
        GameManager.manager.restartGame.RemoveListener(Restart);
    }
}
public enum EnemyType
{
    Zergling,
    Hunter
}
