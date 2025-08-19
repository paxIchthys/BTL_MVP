using Godot;
using System;

using com.escapinghats.btl.logic.view;

namespace com.escapinghats.btl.logic.view.managers
{
	public interface IViewManager
	{
		void UnloadView();
		event EventHandler<ViewEventArgs> OnViewEvent;
	}
	

}
 
