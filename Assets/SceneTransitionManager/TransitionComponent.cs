// https://github.com/TORISOUP/MegTataki/blob/master/Assets/MegTataki/Scripts/Common/TrasitionComponent.cs
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace SceneTransitionManager
{
    public class TransitionComponent : MonoBehaviour
    {
        [SerializeField] private float _transitionSeconds = 1f;
        [SerializeField] private Image _coverImage;
        [Inject] private ZenjectSceneLoader _zenjectSceneLoader;

        BoolReactiveProperty _isTransition = new BoolReactiveProperty(false);
        public IReadOnlyReactiveProperty<bool> IsTransition => _isTransition;

        public void Transition(string nextScene, Action<DiContainer> bindAction)
        {
            _isTransition.Value = false;
            Transition(nextScene, bindAction, this.GetCancellationTokenOnDestroy()).Forget();
        }

        private async UniTaskVoid Transition(string nextScene, Action<DiContainer> bindAction, CancellationToken token)
        {
            // 0 -> 1
            var startTime = Time.time;
            while (Time.time - startTime < _transitionSeconds)
            {
                var rate = (Time.time - startTime) / _transitionSeconds;
                //_coverImage.color.SetA(rate);
                await UniTask.Yield();
            }

            await _zenjectSceneLoader.LoadSceneAsync(nextScene, LoadSceneMode.Single, bindAction);
            //await SceneManager.LoadSceneAsync(nextScene);

            // 1 -> 0
            startTime = Time.time;
            while (Time.time - startTime < _transitionSeconds)
            {
                var rate = 1 - (Time.time - startTime) / _transitionSeconds;
                await UniTask.Yield();
            }

            _isTransition.Value = true;
        }
    }
}