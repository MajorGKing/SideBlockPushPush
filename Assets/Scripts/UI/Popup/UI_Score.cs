using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class UI_Score : UI_Popup
{
    enum Buttons
    {
        ResetButton,
        ExitButton
    }

    enum Texts
    {
        ScoreText,
        InfoText,
    }

    enum GameObjects
    {
        TestObject,
    }

    enum Images
    {
        ItemIcon,
    }

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

		Bind<Button>(typeof(Buttons));
		Bind<TextMeshProUGUI>(typeof(Texts));
		Bind<GameObject>(typeof(GameObjects));
		Bind<Image>(typeof(Images));

		GetButton((int)Buttons.ResetButton).gameObject.BindEvent(OnResetButtonClicked);
        GetButton((int)Buttons.ExitButton).gameObject.BindEvent(OnExitButtonClicked);

        SetScore(Managers.Game._score);
    }

    private void OnResetButtonClicked(PointerEventData data)
    {
        Debug.Log("Reset button clicked");
        Managers.Game.ResetButtonClicked();
    }

    private void OnExitButtonClicked(PointerEventData data)
    {
        Debug.Log("Exit button clicked");
        Managers.Game.ExitButtonClicked();
    }

    public void SetScore(int score)
    {
        Get<TextMeshProUGUI>((int)Texts.ScoreText).text = $"Score: {score}";
    }
}