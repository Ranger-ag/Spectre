using System;
using UnityEngine;

public abstract class PlayerTrigger<TPlayer> : MonoBehaviour where TPlayer : MonoBehaviour
{
    [SerializeField] private Collider triggerCollider;

    private void Start()
    {
        var gameObjectName = gameObject.name;
        if (triggerCollider is null)
        {
            Debug.LogWarning($"GameObject {gameObjectName} triggerCollider is null!");
        }

        if (!triggerCollider.isTrigger)
        {
            Debug.LogWarning($"GameObject {gameObjectName} is not Trigger!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!TryGetPlayerScript(other, out var playerComponent))
        {
            return;
        }

        OnPlayerTriggerEnter(playerComponent);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!TryGetPlayerScript(other, out var playerComponent))
        {
            return;
        }

        OnPlayerTriggerStay(playerComponent);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!TryGetPlayerScript(other, out var playerComponent))
        {
            return;
        }

        OnPlayerTriggerExit(playerComponent);
    }

    private bool TryGetPlayerScript(Collider other, out TPlayer player)
    {
        player = null;

        var gameObject = other.gameObject;
        if(gameObject is null)
        {
            return false;
        }

        var playerComponent = gameObject.GetComponent<TPlayer>();
        if (playerComponent == null)
        {
            return false;
        }

        player = playerComponent;
        return true;
    }

    protected virtual void OnPlayerTriggerEnter(TPlayer playerController)
    {
    }

    protected virtual void OnPlayerTriggerStay(TPlayer playerController)
    {
    }

    protected virtual void OnPlayerTriggerExit(TPlayer playerController)
    {
    }
}
