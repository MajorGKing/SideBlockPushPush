using System.Collections;
using System.Collections.Generic;
using Spine;
using Spine.Unity;
using UnityEngine;

public class Hero : MonoBehaviour
{
	private int _currentLine = 3;
	private int _targetLine;
	private int _touchedLine = -1;
    public SkeletonAnimation SkeletonAnim { get; private set; }
    protected Define.EHeroState _heroState;
    public Define.EHeroState HeroState
    {
        get { return _heroState; }
        set
        {
            if(_heroState != value)
            {
				Debug.Log(value);
                _heroState = value;
                UpdateAnimation();
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
		Managers.Game.OnLineTouched -= LineTouched;
        Managers.Game.OnLineTouched += LineTouched;

		SkeletonAnim = GetComponent<SkeletonAnimation>();

		SkeletonAnim.AnimationState.Event -= OnAnimEventHandler;
		SkeletonAnim.AnimationState.Event += OnAnimEventHandler;

        SkeletonAnim = GetComponent<SkeletonAnimation>();
        StartCoroutine(CoUpdateAI());

		HeroState = Define.EHeroState.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void OnAnimEventHandler(TrackEntry trackEntry, Spine.Event e)
	{
		Debug.Log($"trackEntry : {trackEntry}");
		Debug.Log($"event : {e}");

		// trackEntry의 애니메이션이 'attack'일 때만 이벤트 처리
		if (trackEntry.Animation.Name == "attack" && e.Data.Name == "event_attack")
		{
			Managers.Game.LineAttack(_targetLine);
			// 이벤트가 'event_attack'일 때 HeroState를 Idle로 설정
			Managers.Game.TouchEnable = true;
			HeroState = Define.EHeroState.Idle;

			_currentLine = _targetLine;
		}
	}

	public void LineTouched(int lineIndex)
	{
		Debug.Log("lineIndex : " + lineIndex);
		_touchedLine = lineIndex;
	}

    public TrackEntry PlayAnimation(int trackIndex, string animName, bool loop)
	{
		if (SkeletonAnim == null)
			return null;

		TrackEntry entry = SkeletonAnim.AnimationState.SetAnimation(trackIndex, animName, loop);

		// if (animName == AnimName.DEAD)
		// 	entry.MixDuration = 0;
		// else
		// 	entry.MixDuration = 0.2f;

		return entry;
	}

    protected IEnumerator CoUpdateAI()
	{
		while (true)
		{
			switch (HeroState)
			{
				case Define.EHeroState.None:
					break;
                case Define.EHeroState.Idle:
					UpdateIdle();
					break;
				case Define.EHeroState.Move:
					UpdateMove();
					break;
				case Define.EHeroState.Attack:
					UpdateSkill();
					break;
                case Define.EHeroState.Dead:
                    break;
			}

			// if (UpdateAITick > 0)
			// 	yield return new WaitForSeconds(UpdateAITick);
			// else
			yield return null;
		}
	}

    protected void UpdateAnimation()
	{
		switch (HeroState)
		{
            case Define.EHeroState.None:
                break;
			case Define.EHeroState.Idle:
				PlayAnimation(0, AnimName.IDLE, true);
				break;
			case Define.EHeroState.Move:
				PlayAnimation(0, AnimName.MOVE, true);
				break;
			case Define.EHeroState.Attack:
				PlayAnimation(0, AnimName.ATTACK_A, false);
				break;
			default:
				break;
		}
	}

	public void UpdateIdle()
	{
		if(_touchedLine < 0)
			return;

		// LineTouch가 있다
		// 같은 라인 터치 => 제자리 공격
		if(_touchedLine == _currentLine)
			HeroState = Define.EHeroState.Attack;
		// 다른 줄 이동
		else
		{
			HeroState = Define.EHeroState.Move;
			_targetLine = _touchedLine;
		}
		
	}

	public void UpdateMove()
	{
		_touchedLine = -1;
		Managers.Game.TouchEnable = false;

		float moveSpeed = 10f;
		Vector3 destPos = transform.position;
		destPos.y = 3f - (_targetLine * 1.5f) - 0.5f;

		Vector3 dir = destPos - transform.position;

		transform.position = Vector3.MoveTowards(transform.position, destPos, moveSpeed * Time.deltaTime);

		if (dir.magnitude < 0.01f)
			HeroState = Define.EHeroState.Attack;
	}

	public void UpdateSkill()
	{
		_touchedLine = -1;
		Managers.Game.TouchEnable = false;
	}
}
