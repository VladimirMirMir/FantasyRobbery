using System;
using System.Collections.Generic;
using UnityEngine;

namespace FantasyRobbery.Scripts.Ui
{
    [RequireComponent(typeof(Canvas))]
    public class UiService : MonoBehaviour
    {
        private static UiService s_instance;
        private Dictionary<Type, Screen> _screens = new();

        private void Awake()
        {
            if (s_instance != null)
            {
                Destroy(this);
                return;
            }

            s_instance = this;
        }

        private void Start()
        {
            Show<MainMenuScreen>();
        }

        private void Add<TScreen>() where TScreen : Screen
        {
            //TODO : VM : Add content manager and addressables
            var type = typeof(TScreen);
            var screen = Resources.Load<TScreen>($"Screens/{type.Name}");
            if (screen == null)
                throw new Exception($"You're trying to show {type.Name}, but there is no resource for it!");

            if (_screens.ContainsKey(type))
                throw new Exception($"You're trying to show {type.Name} twice!");
            
            var instance = Instantiate(screen, transform);
            _screens.Add(type, instance);
        }

        private void Delete<TScreen>() where TScreen : Screen
        {
            var type = typeof(TScreen);
            if (_screens.Remove(type, out var screen))
                Destroy(screen.gameObject);
        }

        public static void Show<TScreen>(bool fresh = false) where TScreen : Screen
        {
            var type = typeof(TScreen);
            if (!s_instance._screens.TryGetValue(type, out var screen))
            {
                s_instance.Add<TScreen>();
                return;
            }

            if (!fresh)
            {
                screen.gameObject.SetActive(true);
                return;
            }
            
            s_instance.Delete<TScreen>();
            s_instance.Add<TScreen>();
        }

        public static void Hide<TScreen>() where TScreen : Screen
        {
            var type = typeof(TScreen);
            if (!s_instance._screens.TryGetValue(type, out var screen))
                Debug.LogWarning($"You're trying to hide {type.Name}, but it wasn't added");
            else
                screen.gameObject.SetActive(false);
        }
    }
}