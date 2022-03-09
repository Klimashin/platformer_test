using System.Collections.Generic;
using UnityEngine;

public class EnemiesDefeatedTrigger : GameEventTrigger
{
    [SerializeField] private List<Enemy> _targets;

    private void Awake()
    {
        foreach (var target in _targets)
        {
            target.OnDefeated += OnTargetDefeated;
        }
    }

    private void OnTargetDefeated(Enemy enemy)
    {
        enemy.OnDefeated -= OnTargetDefeated;
        _targets.Remove(enemy);

        if (_targets.Count == 0)
        {
            OnTriggered.Invoke();
        }
    }
}
