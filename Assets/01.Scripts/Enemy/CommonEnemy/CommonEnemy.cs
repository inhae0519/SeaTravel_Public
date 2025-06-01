using System;
using UnityEngine;

public enum CommonStateEnum
{
    Idle,
    Battle, //추적 상태
    Attack,
    Dead
}
public class CommonEnemy : Enemy
{
    public EnemyStateMachine<CommonStateEnum> StateMachine { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        StateMachine = new EnemyStateMachine<CommonStateEnum>();
        
        // 여기에 상태를 불러오는 코드가 필요하다.
        foreach (CommonStateEnum stateEnum in Enum.GetValues(typeof(CommonStateEnum)))
        {
            string typeName = stateEnum.ToString();
            Type t = Type.GetType($"Common{typeName}State");

            try
            {
                EnemyState<CommonStateEnum> state = 
                    Activator.CreateInstance(t, this, StateMachine, typeName) 
                        as EnemyState<CommonStateEnum>;
                StateMachine.AddState(stateEnum, state);
            }
            
            catch (Exception e)
            {
                Debug.LogError($"{typeName} is loading error! check Message");
                Debug.LogError(e.Message);
            }
        }
    }

    private void Start()
    {
        StateMachine.Initialize(CommonStateEnum.Idle, this);
    }

    private void Update()
    {
        StateMachine.CurrentState.UpdateState();
    }

    public void SetDead()
    {
        StateMachine.ChangeState(CommonStateEnum.Dead, true);
        CanStateChangeable = false;
    }

    public override void AnimationEndTrigger()
    {
        StateMachine.CurrentState.AnimationFinishTrigger();
    }
}