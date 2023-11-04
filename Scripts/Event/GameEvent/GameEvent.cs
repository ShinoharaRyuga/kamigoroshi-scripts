using Cysharp.Threading.Tasks;
using System;
using UniRx;
using UnityEngine;

[Serializable]
public class TestGameEvent : IGameEvent
{
    public async UniTask ReleaseEvent()
    {
        await UniTask.CompletedTask;
        Debug.Log("TestGameEventを実行します");
        return;
    }
}

//[Serializable]
//public class PlaySoundEvent : IGameEvent
//{
//    [SerializeField] SoundType _type = SoundType.VOICE;
//    [SerializeField]
//    [SerializeField, Range(0f, 1f)] float _volume = 0.5f;

//    public UniTask ReleaseEvent()
//    {
//        AudioManager.Instance.PlaySound(_type, , _volume);
//        return UniTask.CompletedTask;
//    }
//}

[Serializable]
public class Yajima_Laugh : IGameEvent
{
    public async UniTask ReleaseEvent()
    {
        AudioManager.Instance.PlaySound(SoundType.VOICE, "Voice_Yajima_Laugh", 1f);
        return;
    }
}

[Serializable]
public class Yajima_Uh : IGameEvent
{
    public async UniTask ReleaseEvent()
    {
        AudioManager.Instance.PlaySound(SoundType.VOICE, "Voice_Yajima_Uh", 1f);
        return;
    }
}

[Serializable]
public class Yajima_Ah : IGameEvent
{
    public async UniTask ReleaseEvent()
    {
        AudioManager.Instance.PlaySound(SoundType.VOICE, "Voice_Yajima_Ah", 1f);
        return;
    }
}

[Serializable]
public class Boar_Voice : IGameEvent
{
    public async UniTask ReleaseEvent()
    {
        AudioManager.Instance.PlaySound(SoundType.VOICE, "Voice_NPC1_1", 1f);
        return;
    }
}

[Serializable]
public class Raccoon_Voice : IGameEvent
{
    public async UniTask ReleaseEvent()
    {
        AudioManager.Instance.PlaySound(SoundType.VOICE, "Voice_Raccoon", 1f);
        return;
    }
}

[Serializable]
public class MirrorBreak_SE : IGameEvent
{
    public async UniTask ReleaseEvent()
    {
        AudioManager.Instance.PlaySound(SoundType.SE, "SE_Mirror_Break", 1f);
        return;
    }
}

[Serializable]
public class SceneChangeEvent : IGameEvent
{
    [SerializeField] string _nextSceneName;

    public async UniTask ReleaseEvent()
    {
        var async = EventManager.Instance.SceneChanger.ReturnAsyncOperation(_nextSceneName);
        async.allowSceneActivation = false;

        await EventManager.Instance.FadeController.FadeOut();
        Debug.Log("Tipsを表示します");

        await EventManager.Instance.LoadingTips.OnOpen();

        async.allowSceneActivation = true;

        Debug.Log($"{_nextSceneName}に遷移します");
    }
}

[Serializable]
public class Quest01EndEvent : IGameEvent
{
    public async UniTask ReleaseEvent()
    {
        await Quest01Manager.Instance.QuestEnd();
        
        await UniTask.CompletedTask;
    }
}

[Serializable]
public class Quest02StartEvent : IGameEvent
{
    public async UniTask ReleaseEvent()
    {
        await EventManager.Instance.FadeController.FadeOut();
        Quest02Manager.Instance.OnQuest02Start();
        await EventManager.Instance.FadeController.FadeIn();
        
        await UniTask.CompletedTask;
    }
}

[Serializable]
public class Quest02EndEvent : IGameEvent
{
    public async UniTask ReleaseEvent()
    {
        await Quest02Manager.Instance.OnQuest02End();
        
        await UniTask.CompletedTask;
    }
}

[Serializable]
public class Quest02DetectionRacoon : IGameEvent
{
    public async UniTask ReleaseEvent()
    {
        Quest02Manager.Instance.DetectionRacoon();
        return;
    }
}

[Serializable]
public class Field02ClearEvent : IGameEvent
{
    public async UniTask ReleaseEvent()
    {
        await EventManager.Instance.FadeController.FadeOut();
        await Field02Manager.Instance.EventGameClear();
        await EventManager.Instance.FadeController.FadeIn();

        await UniTask.CompletedTask;
        return;
    }
}

[Serializable]
public class Field02GameOverEvent : IGameEvent
{
    public async UniTask ReleaseEvent()
    {
        Field02Manager.Instance.EventGameOver();
        await UniTask.CompletedTask;
        return;
    }
}

[Serializable]
public class Field02GameEnd : IGameEvent
{
    [SerializeField] private string _sceneName;
    public async UniTask ReleaseEvent()
    {
        PauseManager.Instance.ExecutePause();
        Field02Manager.Instance.GameEnd(_sceneName).Forget();
        await UniTask.CompletedTask;
        return;
    }
}

[Serializable]
public class Quest03DestroyBoar : IGameEvent
{
    public async UniTask ReleaseEvent()
    {
        await EventManager.Instance.FadeController.FadeOut();
        Quest03Manager.Instance.BoarDestroy();
        await EventManager.Instance.FadeController.FadeIn();
        await UniTask.CompletedTask;
        return;
    }
}

[Serializable]
public class Quest03WallDissolve : IGameEvent
{
    public async UniTask ReleaseEvent()
    {
        await Quest03Manager.Instance.WallDissolve();
        await UniTask.CompletedTask;
        return;
    }
}