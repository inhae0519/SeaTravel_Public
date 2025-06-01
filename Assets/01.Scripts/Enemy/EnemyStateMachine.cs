using System;
using System.Collections.Generic;

public class EnemyStateMachine<T> where T : Enum
{
    public EnemyState<T> CurrentState { get; private set; }
    public Dictionary<T, EnemyState<T>> StateDictionary = new ();

    private Enemy _enemyBase;

    public void Initialize(T startState, Enemy enemy)
    {
        _enemyBase = enemy;
        CurrentState = StateDictionary[startState];
        CurrentState.Enter();
    }

    public void ChangeState(T newState, bool forceMode = false)
    {
        if(_enemyBase.CanStateChangeable == false && forceMode == false)
            return;
        
        if(_enemyBase.isDead) return;
        
        CurrentState.Exit();
        CurrentState = StateDictionary[newState];
        CurrentState.Enter();
    }

    public void AddState(T startEnum, EnemyState<T> state)
    {
        StateDictionary.Add(startEnum, state);
    }
}