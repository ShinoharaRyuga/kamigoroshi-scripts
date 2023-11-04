using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace BehaviorTreeNodeGraphEditor
{
    /// <summary>
    /// 行動ツリーを実行するためのランナークラス
    /// </summary>
    public class BehaviorTreeRunner : MonoBehaviour, IPause
    {
        [Tooltip("メインの行動ツリーアセット")] public BehaviorTree _behaviorTree;

        // ゲームオブジェクトのサブシステムを保持するコンテキストオブジェクト
        private Context _context;
        private PlayerManager _playerController;
        private PauseManager _pauseManager;
        private bool _isPause = false;

        void Start()
        {
            _context = CreateBehaviourTreeContext();
            _playerController = FindObjectOfType<PlayerManager>();
            _context.playerManager = (_playerController != null) ? _playerController : null;
            _behaviorTree = _behaviorTree.Clone();
            _behaviorTree.Bind(_context);

            _pauseManager = PauseManager.Instance;
            _pauseManager.AddPauseObject(this);
        }

        void Update()
        {
            if (_behaviorTree)
            {
                if (!_isPause)
                {
                    _behaviorTree.Update();
                }
            }

            if (_context.enemyController.CurrentHP.Value <= 0)
            {
                _pauseManager.RemovePauseObject(this);
            }
        }

        /// <summary>
        /// 行動ツリー用のコンテキストを作成
        /// </summary>
        /// <returns></returns>
        Context CreateBehaviourTreeContext()
        {
            return Context.CreateFromGameObject(gameObject);
        }

        /// <summary>
        /// 選択時に呼び出されるメソッド
        /// </summary>
        private void OnDrawGizmosSelected()
        {
            if (!_behaviorTree)
            {
                return;
            }

            BehaviorTree.Traverse(_behaviorTree.rootNode, (n) =>
            {
                if (n.drawGizmos)
                {
                    n.OnDrawGizmos();
                }
            });
        }

        public void Pause()
        {
            _isPause = true;
        }

        public void Resume()
        {
            _isPause = false;
        }
    }
}