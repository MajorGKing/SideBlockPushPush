using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Game : UI_Scene
{

    public TextMeshProUGUI _time;
    public TextMeshProUGUI _score;
    public TextMeshProUGUI _gameOver;
    public override void Init()
	{
		Managers.UI.SetCanvas(gameObject, false);
	}

    public void ResetButtonClicked()
    {
        Managers.Game.ResetButtonClicked();
    }

    public void ExitButtonClicked()
    {
        Managers.Game.ExitButtonClicked();
    }

    public void UpdateTime(float time)
    {
        _time.text = time.ToString("F2");
    }

    public void UpdateScore(int score)
    {
        _score.text = score.ToString();
    }
}
