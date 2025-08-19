using com.escapinghats.btl.logic.model.characters;
using com.escapinghats.btl.logic.model.definitions;
using com.escapinghats.btl.logic.model.state;
using com.escapinghats.btl.logic.services;

using System.Linq;
using System.Text.Json;

namespace com.escapinghats.btl.logic.model
{
    public sealed class Model
    {
        private static readonly Model _instance = new Model();
        private static readonly object _lock = new object();

        public static State State;
        public static Definitions Definitions;
        public static Characters Characters;

        private Model() { Initialize(); }

        public static Model Instance
        {
            get
            {
                lock (_lock)
                {
                    return _instance;
                }
            }
        }

        private void Initialize()
        {
            Definitions = new Definitions("Definitions", 1);
            Characters = new Characters();
            State = new State();
        }

        public void LoadData(string dataContents, string fileName)
        {
            Debug.Instance.Log("Model::Loading data: " + fileName);

            ModelLoader modelLoader = new ModelLoader(dataContents, fileName);

            Debug.Instance.Log("Model::Loading rest of data: " + modelLoader.FileType);

            if(modelLoader.FileType == "definitions"){
                ModelLoader nextModelLoader = new ModelLoader(modelLoader.DataContents, modelLoader.FileRemaining);

                Debug.Instance.Log("Model::Loading rest of data NEXT: " + nextModelLoader.FileType);
                
                Definitions.LoadDefinition(modelLoader);
            }
        }
    }

    public class ModelLoader
    {
        public string[] FileParts;
        public string FileType;
        public string[] FileRemaining;
        public string DataContents;

        public ModelLoader(string dataContents, string fileName)
        {
            FileParts = fileName.Split('/');
            FileType = FileParts[0];
            FileRemaining = FileParts.Skip(1).ToArray();
            DataContents = dataContents;
        }

        public ModelLoader(string dataContents, string[] fileRemaining){
            DataContents = dataContents;
            FileRemaining = fileRemaining.Skip(1).ToArray();
            FileType = fileRemaining[0];
        }

        public T LoadData<T>(){
            return JsonSerializer.Deserialize<T>(DataContents);
        }
    }
}
