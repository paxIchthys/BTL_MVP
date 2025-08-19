namespace com.escapinghats.btl.logic.view
{
    using System;
    using Godot;
    using com.escapinghats.btl.config;
    using com.escapinghats.btl.logic.controller;
    using com.escapinghats.btl.logic.model;
    using com.escapinghats.btl.logic.model.state;
    using com.escapinghats.btl.logic.view.managers;
    using com.escapinghats.btl.logic.services;

    public class View
    {
        private const string ErrorScene = "nodes/views/error.tscn";
        private const string LoaderScene = "nodes/views/loader.tscn";
        
        public PackedScene CurrentScene { get; private set; }
        private Node _currentViewNode;
        private static Node2D _stage;
        private static View _instance;
        private IViewManager _viewManager;

        public static View Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new View();
                }
                return _instance;
            }
        }

        private View()
        {
        }

        public void Initialize(Node2D stage)
        {
            _stage = stage;
        }

        public void LoadPerspective(State.Types type)
        {
            Debug.Instance.Log($"Loading perspective: {type}");
            
            switch (type)
            {
                case State.Types.LOADING:
                    var viewData = new ViewData();
                    LoadView(Config.NODES_ROOT + LoaderScene, Loader.Instance.Initialize, Loader.Instance, viewData);
                    break;
                default:
                    var errorViewData = new ErrorViewData
                    {
                        Title = "Fatal Error: Unrecognized State",
                        Description = $"State had entered an unhandled type: {type}"
                    };
                    LoadView(Config.NODES_ROOT + ErrorScene, ErrorView.Instance.Initialize, ErrorView.Instance, errorViewData);
                    break;                
			}	
		}

		private Node LoadView<T>(string packedSceneLocation, Action<Node, T> action, IViewManager viewManager, T viewData) where T : ViewData {
			Debug.Instance.Log("Loading scene: " + packedSceneLocation);
			CurrentScene = GD.Load<PackedScene>(packedSceneLocation);
			if (CurrentScene == null) {
				Debug.Instance.Error("Failed to load scene: " + packedSceneLocation);
				return null;
			}else{
				Node instance = CurrentScene.Instantiate();
				_stage.AddChild(instance);
				_currentViewNode = instance;

				Debug.Instance.Log("Scene loaded successfully: " + packedSceneLocation);
				_viewManager = viewManager;
				_viewManager.OnViewEvent += HandleViewEvent;

				action(instance, viewData);
				
				Debug.Instance.Log("View manager: " + _viewManager);
				return instance;
			}		
		}

		public void UnloadView(){
			if(_currentViewNode != null){
				_stage.RemoveChild(_currentViewNode);
				_currentViewNode = null;
			}
			if(_viewManager != null){
				_viewManager.OnViewEvent -= HandleViewEvent;
				_viewManager.UnloadView();
				_viewManager = null;
			}
		}

		private void HandleViewEvent(object sender, ViewEventArgs e){
            Debug.Instance.Log($"ViewEvent: {e.Type}");
            if (e.Type == ViewEventTypes.ViewDone)
            {
                UnloadView();
                Controller.Instance.NextState();
            }
		}


	}

    public class ViewData
    {
    }
}
