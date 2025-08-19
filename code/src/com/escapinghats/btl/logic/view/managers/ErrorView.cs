using Godot;
using System;
using System.Collections.Generic;

using com.escapinghats.btl.logic.model;
using com.escapinghats.btl.logic.services;
using com.escapinghats.btl.logic.view;

namespace com.escapinghats.btl.logic.view.managers
{
    public class ErrorView : IViewManager
    {
        public event EventHandler<ViewEventArgs> OnViewEvent;

        private Node _currentScene;
        private static ErrorView _instance;
        private static readonly object _lock = new object();

        private ErrorView() 
        { 
        }

        public static ErrorView Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new ErrorView();
                    }
                    return _instance;
                }
            }
        }

        public void Initialize(Node scene, ErrorViewData data)
        {
            _currentScene = scene;
            Debug.Instance.Log("ErrorView initialized");
            SetTitle(data.Title);
            SetDescription(data.Description);
        }

        private void SetTitle(string title)
        {
            _currentScene.CallDeferred("set_title", title);
        }

        private void SetDescription(string description)
        {
            _currentScene.CallDeferred("set_description", description);
        }

        public EventHandler<ViewEventArgs> GetEvent()
        {
            return OnViewEvent;
        }

        public void UnloadView()
        {
            _currentScene = null;
        }
    }

    public class ErrorViewData : ViewData
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
