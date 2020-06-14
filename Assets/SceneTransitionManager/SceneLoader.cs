// https://github.com/TORISOUP/MegTataki/blob/master/Assets/MegTataki/Scripts/Common/TrasitionComponent.cs

using System;
using UniRx;
using UnityEngine;
using Zenject;

namespace SceneTransitionManager
{
    public static class SceneLoader
    {
        private static TransitionComponent _transitionComponent;

        private static TransitionComponent TransitionComponent
        {
            get
            {
                if (_transitionComponent != null) return _transitionComponent;
                var path = Resources.Load("SceneTransition");
                var go = GameObject.Instantiate(path) as GameObject;
                _transitionComponent = go.GetComponent<TransitionComponent>();
                return _transitionComponent;
            }
        }

        /// <summary>
        /// シーン遷移が完了した
        /// </summary>
        public static IObservable<Unit> OnTransitionFinished
        {
            get
            {
                if (!TransitionComponent.IsTransition.Value)
                {
                    return Observable.Return(Unit.Default);
                }
                else
                {
                    return TransitionComponent
                        .IsTransition
                        .FirstOrDefault(x => !x).AsUnitObservable();
                }
            }
        }

        /// <summary>
        /// シーン遷移する
        /// </summary>
        /// <param name="nextScene"></param>
        /// <param name="action"></param>
        public static void LoadScene(string nextScene, Action<DiContainer> action)
        {
            TransitionComponent.Transition(nextScene, action);
        }

    }
}