using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FantasyRobbery.Scripts.Ui
{
    [RequireComponent(typeof(Canvas))]
    public class UiService : MonoBehaviour
    {
        private static UiService s_instance;

        [SerializeField] private Camera mainMenuCamera;
        
        private Dictionary<Type, Screen> _cachedScreens = new();
        private Dictionary<Type, Screen> _showedScreens = new();

        public void Init()
        {
            if (s_instance == null)
            {
                s_instance = this;
                DontDestroyOnLoad(this);
            }
        }

        private TScreen Add<TScreen>(Transform parent = null) where TScreen : Screen
        {
            //TODO : VM : Add content manager and addressables
            var type = typeof(TScreen);
            var screen = Resources.Load<TScreen>($"Screens/{type.Name}");
            if (screen == null)
            {
                Debug.LogError($"You're trying to show {type.Name}, but there is no resource for it!");
                return null;
            }

            if (_cachedScreens.ContainsKey(type))
            {
                Debug.LogError($"You're trying to show {type.Name} twice!");
                return null;
            }
            
            var instance = Instantiate(screen, parent == null ? transform : parent);
            _cachedScreens.Add(type, instance);
            _showedScreens.Add(type, instance);
            return instance;
        }

        private void Delete(Type type)
        {
            if (_cachedScreens.Remove(type, out var screen))
            {
                _showedScreens.Remove(type);
                Destroy(screen.gameObject);
            }
        }

        private void Delete<TScreen>() where TScreen : Screen
        {
            Delete(typeof(TScreen));
        }

        public static TScreen Show<TScreen>(bool hideOtherScreens = true, Transform parent = null, bool fresh = false) where TScreen : Screen
        {
            if (hideOtherScreens)
            {
                var showedScreens = s_instance._showedScreens.Keys.ToList();
                foreach (var screenType in showedScreens)
                    Hide(screenType);
            }
            
            var type = typeof(TScreen);
            if (!s_instance._cachedScreens.TryGetValue(type, out var screen))
            {
                return s_instance.Add<TScreen>(parent);
            }

            if (!fresh)
            {
                screen.gameObject.SetActive(true);
                if (!s_instance._showedScreens.TryAdd(type, screen))
                    Debug.LogWarning($"You're trying to show already showed screen : {type.Name}");
                return (TScreen)screen;
            }
            
            s_instance.Delete<TScreen>();
            return s_instance.Add<TScreen>(parent);
        }

        private static void Hide(Type screenType)
        {
            if (!s_instance._cachedScreens.TryGetValue(screenType, out var screen))
            {
                Debug.LogWarning($"You're trying to hide {screenType.Name}, but it wasn't added");
                return;
            }
            
            screen.gameObject.SetActive(false);
            s_instance._showedScreens.Remove(screenType);
        }

        public static void Hide<TScreen>() where TScreen : Screen
        {
            Hide(typeof(TScreen));
        }

        public static void ToggleMainMenuCamera(bool value)
        {
            s_instance.mainMenuCamera.gameObject.SetActive(value);
        }
    }
}