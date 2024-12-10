using System;
using UnityEngine;

public class GameManager
{
    private GameScene _scene;

    private bool m_isGamePlayed = true;
    public bool isGamePlayed { get { return m_isGamePlayed; } }

    private int m_score;
    public int _score { get { return m_score; } }
    private bool _touchEnable;
    public bool TouchEnable { get { return _touchEnable; } set { _touchEnable = value; } }

    public void Init()
    {
        TouchEnable = true;
    }

    public void Clear()
    {

    }

    public void LineTouch(int lineIndex)
    {
        if (TouchEnable == false)
            return;

        OnLineTouched?.Invoke(lineIndex);
    }

    public void LineAttack(int lineIndex)
    {
        OnLineAttack?.Invoke(lineIndex);
    }

    public void ResetButtonClicked()
    {
        if (Managers.Scene.CurrentScene.SceneType == Define.Scene.Game)
        {
            Managers.Scene.LoadScene(Define.Scene.Game);
        }
    }

    public void ExitButtonClicked()
    {
        if (Managers.Scene.CurrentScene.SceneType == Define.Scene.Game)
        {
            Application.Quit();
        }
    }

    public void OnUpdate(float time)
    {
        if (_scene == null)
        {
            _scene = (GameScene)Managers.Scene.CurrentScene;
        }

        _scene.OnUpdate(time);
    }

    public void CallUITimeUpdate(float time)
    {
        Managers.UI.UpdateGameTime(time);
    }

    public void CallUIScoreUpdate(int score)
    {
        Managers.UI.UpdateGameScore(score);

        m_score = score;
    }

    public void GameIsPlayed(bool play)
    {
        m_isGamePlayed = play;

        if (m_isGamePlayed == false)
        {
            Managers.UI.ShowPopupUI<UI_Score>();
        }
    }

    #region Action
    public event Action<int> OnLineTouched;
    public event Action<int> OnLineAttack;
    #endregion
}