
using Godot;
using System;

using com.escapinghats.btl.config;
using com.escapinghats.btl.logic.controller;
using com.escapinghats.btl.logic.model;
using com.escapinghats.btl.logic.model.state;
using com.escapinghats.btl.logic.services;
using com.escapinghats.btl.logic.view.managers;
using com.escapinghats.btl.logic.view;

namespace com.escapinghats.btl.logic{

	public partial class GameLogic : Node2D
	{
		// Accessible by GDScript
		public string Version { get; set; } = "MVP.001";	
		
		// Public properties
		public Node2D Stage { get => _stage; set => HandleStageSet(value); }
		
		// Private fields
		private static Node2D _stage;

		public override void _Ready(){
			Debug.Instance.Log("GameLogic Ready");
			Model.State.StateEvent += OnStateEvent;
		}


		private void HandleStageSet(Node2D value){
			Debug.Instance.Log("Stage set to: " + value);
			_stage = value; 
			Debug.Instance.Log("View: " + View.Instance);
			View.Instance.Initialize(_stage);
			Debug.Instance.Log("Stage set to: " + value);

			Controller.Instance.Initialize(this);
		}

		private void OnStateEvent(object sender, StateEventArgs e){
			Debug.Instance.Log("StateEvent: " + e.Type);
			if(e.Type == StateEventArgs.StateEventTypes.STATE_CHANGED){
				View.Instance.LoadPerspective(Model.State.Type);
			}
		}

	}
}
