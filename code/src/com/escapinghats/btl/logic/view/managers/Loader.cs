namespace com.escapinghats.btl.logic.view.managers
{
    using System;
    using System.Collections.Generic;
    using Godot;
    using com.escapinghats.btl.config;
    using com.escapinghats.btl.logic.model;
    using com.escapinghats.btl.logic.services;

    /// <summary>
    /// Manages the loading of game resources and scenes, providing progress tracking and event notifications.
    /// Implements the singleton pattern to ensure only one instance exists.
    /// </summary>
    public class Loader : IViewManager
    {
        /// <summary>
        /// Occurs when a view-related event is raised by the loader.
        /// </summary>
        public event EventHandler<ViewEventArgs> OnViewEvent;

        private static ContextLoader _contextLoader;
        private Node _currentScene;
        private static Loader _instance;
        private string _loadingLog;
        private static readonly object _lock = new object();
        private static Queue<LoaderStep> _steps;
        private int _stepsCompleted;
        private int _totalSteps;

        /// <summary>
        /// Gets the current loading progress as a percentage.
        /// </summary>
        /// <value>A float value between 0 and 100 representing the loading progress.</value>
        public float Progress
        {
            get
            {
                if (_totalSteps == 0) return 0;
                return ((float)_stepsCompleted / _totalSteps) * 100;
            }
        }

        private Loader()
        {
        }

        /// <summary>
        /// Gets the singleton instance of the Loader class.
        /// </summary>
        /// <value>The singleton instance of the Loader.</value>
        public static Loader Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new Loader();
                        _steps = new Queue<LoaderStep>();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Initializes the loader with the specified scene and view data.
        /// </summary>
        /// <param name="scene">The scene node to attach the loader to.</param>
        /// <param name="viewData">The view data containing initialization parameters.</param>
        public void Initialize(Node scene, ViewData viewData)
        {
            Debug.Instance.Log("Loader initializing");

            if (_currentScene == null)
            {
                _currentScene = scene;
            }
            
            _loadingLog = "Loading...\n";

			HttpRequest httpRequest = _currentScene.GetNode<HttpRequest>("HTTPRequest");
			
			if(httpRequest == null){
				Debug.Instance.Error("HttpRequest not found. Context Loading paused");
				return;
			}

			if(_contextLoader == null){
				_contextLoader = new ContextLoader();
				_contextLoader.Initialize(httpRequest);
				_contextLoader.LoaderEvent += HandleContextLoaderEvent;
			}

			Debug.Instance.Log("Loader initialized" + _currentScene + " " + httpRequest);
		}


        /// <summary>
        /// Gets the event handler for view events.
        /// </summary>
        /// <returns>The event handler for view events.</returns>
        public EventHandler<ViewEventArgs> GetEvent()
        {
            return OnViewEvent;
        }

		public void HandleContextLoaderEvent(object sender, LoaderEventArgs e){
			Debug.Instance.Log($"ContextLoaderEvent: {e.Message}");

			if(e.Type == LoaderEventArgs.LoaderEventTypes.ContextLoaded){
				Load();
			}else if(e.Type == LoaderEventArgs.LoaderEventTypes.DataLoaded){
				LoadData(e.FileContents, e.FileName);

				_stepsCompleted++;
				//AddLog("|---" + StepsCompleted + " of " + TotalSteps + "--" + Progress + "%");
				_currentScene.CallDeferred("set_progress", Progress);	
				RunNextStep();

			}else if(e.Type == LoaderEventArgs.LoaderEventTypes.Error){
				Debug.Instance.Error($"Loader Error: File Load Failed {e.Message} {e.FileName}");
				if(e.FileName == ContextLoader.CONTEXT_FILE_NAME){
					_currentScene.CallDeferred("set_text", "CRITICAL ERROR: Failed to load game context.");
					_currentScene.CallDeferred("set_progress", 0);
				}
			}
		}	

        /// <summary>
        /// Loads data into the model.
        /// </summary>
        /// <param name="dataContents">The contents of the data to load.</param>
        /// <param name="fileName">The name of the file being loaded.</param>
        public void LoadData(string dataContents, string fileName)
        {
            // Give to model
            Model.Instance.LoadData(dataContents, fileName);
        }

        /// <summary>
        /// Adds a new loading step to the queue.
        /// </summary>
        /// <param name="step">The loading step to add.</param>
        /// <returns>True if the step was added successfully; otherwise, false.</returns>
        public bool AddStep(LoaderStep step)
        {
            if (step != null)
            {
                lock (_lock)
                {
                    _steps.Enqueue(step);
                    _totalSteps++;
                    return true;
                }
            }
            return false;
        }

        private void UpdateLog()
        {
            _currentScene.CallDeferred("set_text", _loadingLog);
        }

        private void AddLog(string newLine)
        {
            _loadingLog += newLine + "\n";
            UpdateLog();
        }

        /// <summary>
        /// Starts the loading process by enqueuing all context files for loading.
        /// </summary>
        public void Load()
        {
            UpdateLog();

            Debug.Instance.Log("Loader is working...");
            AddLog(tab(1) + "Context Files...");

            foreach (var item in _contextLoader.ContextDef.PreloadList.Data)
            {
                var step = new LoaderStep { Name = item };
                _steps.Enqueue(step);
                _totalSteps++;
            }

            AddLog(tab(2) + "(" + _totalSteps + ") context files to load");
            RunNextStep();
        }

		private static string tab(int count)
		{
			return new string(' ', count * 2);
		}

        /// <summary>
        /// Processes the next loading step in the queue.
        /// </summary>
        /// <returns>True if there are more steps to process; otherwise, false.</returns>
        public bool RunNextStep()
        {
            LoaderStep step;
            
            lock (_lock)
            {
                if (_steps.Count == 0)
                {
                    Debug.Instance.Log("All steps completed!");
                    _currentScene.CallDeferred("set_progress", 100);
                    OnViewEvent?.Invoke(this, new ViewEventArgs(ViewEventTypes.ViewDone, "Loading complete"));
                    return false;
                }

                step = _steps.Dequeue();
            }

            if (step == null)
            {
                Debug.Instance.Error("Step is null");
                return false;
            }

            AddLog(tab(2) + step.Name);
            _contextLoader.LoadFile(Config.CONTEXT_ROOT + step.Name, ContextLoader.LoadTypes.Data, step.Name);
            return true;
        }

        /// <summary>
        /// Unloads the current view and releases resources.
        /// </summary>
        public void UnloadView()
        {
            // Implementation to unload resources can be added here
        }
		
	}

    /// <summary>
    /// Represents a single step in the loading process.
    /// </summary>
    public class LoaderStep
    {
        /// <summary>
        /// Gets or sets the name of the loading step.
        /// </summary>
        public string Name { get; set; }
    }
}
