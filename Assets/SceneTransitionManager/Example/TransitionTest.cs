using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace SceneTransitionManager.Example
{
    /// <summary>
    /// ボタンを押すと次のシーンへ移動するサンプル
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class TransitionTest : MonoBehaviour
    {
        [SerializeField] private GameScenes _nextScene;

        void Start()
        {
            var button = GetComponent<Button>();
            button.OnClickAsObservable().Subscribe(_ => SceneLoader.LoadScene(_nextScene.ToString(), __ => { }));
        }
    }
}