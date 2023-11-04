using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace EventCharacterIcon
{
    public abstract class EventCharacterData
    {
        protected static readonly string Path = "EventTalkIcon/";

        public string Name { get; protected set; }
        public Sprite Icon { get; protected set; }

        public abstract void SetupData();
    }

    [Serializable]
    public class Yashima : EventCharacterData
    {
        [SerializeField] Faical _faical = Faical.Normal;

        enum Faical
        {
            None = -1,
            Normal = 0,
            Cry = 1,
            Smile = 2,
            Angry = 3,
        }

        public override void SetupData()
        {
            var path = new StringBuilder("Yajima_Douji/");

            switch (_faical)
            {
                case Faical.None:
                    path.Append("Normal");
                    break;
                case Faical.Normal:
                    path.Append("Normal");
                    break;
                case Faical.Cry:
                    path.Append("Cry");
                    break;
                case Faical.Smile:
                    path.Append("Smile");
                    break;
                case Faical.Angry:
                    path.Append("Angry");
                    break;
                default:
                    break;
            }

            var icon = Resources.Load<Sprite>(Path + path);

            if (!icon) Debug.LogError($"ロードに失敗しました：Path[{Path + path}]");

            Icon = icon;
            Name = "八島童子";

            Resources.UnloadUnusedAssets();
        }
    }

    [Serializable]
    public class Miyashima : EventCharacterData
    {
        [SerializeField] Faical _faical = Faical.Normal;

        enum Faical
        {
            None = -1,
            Normal = 0,
            Angry = 1,
        }

        public override void SetupData()
        {
            var path = new StringBuilder("Miyashima/");

            switch (_faical)
            {
                case Faical.None:
                    path.Append("Normal");
                    break;
                case Faical.Normal:
                    path.Append("Normal");
                    break;
                case Faical.Angry:
                    path.Append("Angry");
                    break;
                default:
                    break;
            }

            var icon = Resources.Load<Sprite>(Path + path);

            if (!icon) Debug.LogError($"ロードに失敗しました：Path[{Path + path}]");

            Icon = icon;
            Name = "ミヤシマ";

            Resources.UnloadUnusedAssets();
        }
    }

    [Serializable]
    public class Pig : EventCharacterData
    {
        [SerializeField] Faical _faical = Faical.Normal;

        enum Faical
        {
            None = -1,
            Normal = 0,
        }

        public override void SetupData()
        {
            var path = new StringBuilder("NPC_Pig/");

            switch (_faical)
            {
                case Faical.None:
                    path.Append("Normal");
                    break;
                case Faical.Normal:
                    path.Append("Normal");
                    break;
                default:
                    break;
            }

            var icon = Resources.Load<Sprite>(Path + path);

            if (!icon) Debug.LogError($"ロードに失敗しました：Path[{Path + path}]");

            Icon = icon;
            Name = "猪";

            Resources.UnloadUnusedAssets();
        }
    }

    [Serializable]
    public class RaccoonDog : EventCharacterData
    {
        [SerializeField] Faical _faical = Faical.Normal;

        enum Faical
        {
            None = -1,
            Normal = 0,
        }

        public override void SetupData()
        {
            var path = new StringBuilder("NPC_RaccoonDog/");

            switch (_faical)
            {
                case Faical.None:
                    path.Append("Normal");
                    break;
                case Faical.Normal:
                    path.Append("Normal");
                    break;
                default:
                    break;
            }

            var icon = Resources.Load<Sprite>(Path + path);

            if (!icon) Debug.LogError($"ロードに失敗しました：Path[{Path + path}]");

            Icon = icon;
            Name = "化け狸";

            Resources.UnloadUnusedAssets();
        }
    }
}