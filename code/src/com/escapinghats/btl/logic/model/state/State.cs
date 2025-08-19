using System;
using com.escapinghats.btl.logic.services;

namespace com.escapinghats.btl.logic.model.state
{
    public class State // MOVE TO CONTROLLER AND REPLACE WITH SINGLE VALUE CURRENT STATE ON MODEL
    {
       public Types Type { get { return _type;} set{ NewState(value); } }
       public event EventHandler<StateEventArgs> StateEvent;

       private Types _type; 

        public enum Types 
        { 
            UNDEFINED,
            LOADING,
            TITLE           
        }

        public State()
        {
            _type = Types.UNDEFINED;
        }

        private void NewState(Types type){
            _type = type;
            StateEvent?.Invoke(this, new StateEventArgs(StateEventArgs.StateEventTypes.STATE_CHANGED));
            Debug.Instance.Log("State::New: " + type);
        }
    }

    public class StateEventArgs : EventArgs
    {
        public enum StateEventTypes 
        { 
            STATE_CHANGED
        }  
        public StateEventTypes Type;

        public StateEventArgs(StateEventTypes type){
            Type = type;
        }
    }
}
