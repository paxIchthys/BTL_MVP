using com.escapinghats.btl.logic.model;
using com.escapinghats.btl.logic.services;
using com.escapinghats.btl.logic.model.state;

namespace com.escapinghats.btl.logic.controller
{
    public class Controller
    {
        private static Controller _instance;
        public static Controller Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Controller();
                }
                return _instance;
            }
        }

        public void NextState(){
            State.Types newType = State.Types.UNDEFINED;
            
            switch(Model.State.Type){
                case State.Types.UNDEFINED:
                    newType = State.Types.LOADING;
                    break;
                case State.Types.LOADING:
                    newType = State.Types.TITLE;
                    break;
            }

            Debug.Instance.Log("State::Next: " + newType);

            Model.State.Type = newType;

        }

        private Controller()
        {
        }

        public void Initialize(GameLogic gameLogic){
            NextState();
        }

    }
}
