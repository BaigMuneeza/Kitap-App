using KitapCache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using KitapUIElements.Views;
using KitapClientMananger;

namespace KitapUIElements
{
    public class GUIHandler
    {
        private static GUIHandler _instance;
        private CacheManager _cacheManager;
        public ClientManager _clientManager;
        private Dictionary<Type, object> m_dicViewInstances;

        public static GUIHandler Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GUIHandler();
                }
                return _instance;
            }
        }
        public ClientManager ClientManager
        {
            get { return _clientManager; }
            set { _clientManager = value; }
        }

        public CacheManager CacheManager
        {
            get { return _cacheManager; }
            set { _cacheManager = value; }
        }

        public Window CurrentWindow { get; private set; }

        public void SetView<T>(Window window) where T : new()
		{
			Type viewType = typeof(T);
			if (m_dicViewInstances == null)
			{
				m_dicViewInstances = new Dictionary<Type, object>();
			}

			if (!m_dicViewInstances.ContainsKey(viewType))
			{
				m_dicViewInstances[viewType] = new T();
			}
			FrameworkElement view = (FrameworkElement)m_dicViewInstances[viewType];
			window.Content = view;
			window.SizeToContent = SizeToContent.WidthAndHeight;
		}

        public void SetCurrentWindow<T>(T window) where T : Window
        {
            CurrentWindow = window;
        }

        public TWindow CreateNewWindow<TWindow>() where TWindow : Window, new()
        {
            return new TWindow();
        }
    }
}