public class GameManager
{
    private GameScene _scene;
    public void Init()
    {

    }

    public void Clear()
    {

    }

    public void LineTouch(int lineIndex)
    {
        if(_scene == null)
        {
            _scene = (GameScene)Managers.Scene.CurrentScene;
        }

        _scene.LineTouched(lineIndex);
    }
}
