namespace com.escapinghats.btl.config
{
    public class Config
    {
        public enum ResourceRootTypes { Local, Remote };

        public const string GAME_DATA = "http://192.168.86.29:3000/btl.json";
        public const string CONTEXT_ROOT = "http://192.168.86.29:3000/data/";
        public const string ASSETS_ROOT = "res://assets/";
        public const string NODES_ROOT = "res://";
        
        public const string RESOURCE_ROOT_LOCAL = "res://";
        public const string RESOURCE_ROOT_REMOTE = "https://btl-builder.herokuapp.com/";

        public static ResourceRootTypes ResourceRootType { get; set; } = ResourceRootTypes.Remote;
    }
}
