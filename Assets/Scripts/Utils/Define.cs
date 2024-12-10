using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum Scene
    {
        Unknown,
        Login,
        Lobby,
        Game,
    }

    public enum Sound
    {
        Bgm,
        Effect,
        MaxCount,
    }

    public enum UIEvent
    {
        Click,
        Drag,
    }

    public enum MouseEvent
    {
        Press,
        Click,
    }

    public enum CameraMode
    {
        QuarterView,
        NormalView,
    }

    public enum EHeroState
	{
		None,
		Idle,
		Move,
		Attack,
        Dead,
	}
}

public static class AnimName
{
	public const string ATTACK_A = "attack";
	public const string ATTACK_B = "attack";
	public const string SKILL_A = "skill";
	public const string SKILL_B = "skill";
	public const string IDLE = "idle";
	public const string MOVE = "move";
	public const string DAMAGED = "hit";
	public const string DEAD = "dead";
	public const string EVENT_ATTACK_A = "event_attack";
	public const string EVENT_ATTACK_B = "event_attack";
	public const string EVENT_SKILL_A = "event_attack";
	public const string EVENT_SKILL_B = "event_attack";
}
