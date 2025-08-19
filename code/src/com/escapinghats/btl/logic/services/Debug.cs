namespace com.escapinghats.btl.logic.services
{
    public class Debug
    {
        private static Debug _instance;

        private Debug()
        {
        }

        public static Debug Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Debug();
                }
                return _instance;
            }
        }

        public void Log(string message)
        {
            Godot.GD.Print(message);
        }

        public void Error(string message)
        {
            Godot.GD.PrintErr(message);
        }
    }
}
