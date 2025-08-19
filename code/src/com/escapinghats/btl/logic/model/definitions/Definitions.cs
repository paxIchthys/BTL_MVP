using System.Collections.Generic;
using com.escapinghats.btl.logic.services;
using com.escapinghats.btl.logic.model.characters;
using com.escapinghats.btl.logic.model.logicBlocks;
using com.escapinghats.btl.logic.model.text;
using com.escapinghats.btl.logic.model.definitions. items;

namespace com.escapinghats.btl.logic.model.definitions
{
	[System.Serializable]
	public class Definitions
	{
		public string name;
		public int id;
		public List<Definition> DefinitionList = new List<Definition>();

		public Characters CharacterList = new Characters();
		public List<LogicBlocks> LogicBlockList = new List<LogicBlocks>();
		public List<Text> TextList = new List<Text>();
		public Items ItemList = new Items();
		

		public Definitions(string name, int id)
		{
			this.name = name;
			this.id = id;
		}

		public bool LoadDefinition(ModelLoader modelLoader)
		{
			ModelLoader nextModelLoader = new ModelLoader(modelLoader.DataContents, modelLoader.FileRemaining);
			string fileType = nextModelLoader.FileType;
			
			Debug.Instance.Log("Definitions::Adding definition: " + fileType + " " + nextModelLoader.FileRemaining[0]);
			

			switch(fileType){
				case "characters":
					LoadCharacters(nextModelLoader);
					break;
				case "items":
					LoadItems(nextModelLoader);
					break;
				case "logicBlocks":
					LoadLogicBlocks(nextModelLoader);
					break;
				case "text":
					LoadText(nextModelLoader);
					break;
				default:
					Debug.Instance.Error("Definitions::Adding UNKNOWN definition: " + fileType + " " + nextModelLoader.FileRemaining[0]);
					return false;
			}

			return true;
		}

		public void LoadCharacters(ModelLoader modelLoader)
		{
			if(CharacterList == null){
				CharacterList = new Characters();
			}
			CharacterList.Load(modelLoader);
		}

		public void LoadItems(ModelLoader modelLoader)
		{
			if(ItemList == null){
				ItemList = new Items();
			}
			ItemList.Load(modelLoader);
		}

		public void LoadLogicBlocks(ModelLoader modelLoader)
		{
			//LogicBlockList.Add(new LogicBlocks(modelLoader.FileRemaining[0], modelLoader.FileRemaining[1]));
			/*
			string fileName = logicBlock.name.Split('/')[0];
			if(fileName == "logicBlocks")
			{
				Debug.Instance.Log("Adding logic block: " + logicBlock.name);
				LogicBlockList.Add(logicBlock);
			}
			*/
		}

		public void LoadText(ModelLoader modelLoader)
		{
			//TextList.Add(new Text(modelLoader.FileRemaining[0], modelLoader.FileRemaining[1]));
			/*
			string fileName = text.name.Split('/')[0];
			if(fileName == "text")
			{
				Debug.Instance.Log("Adding text: " + text.name);
				TextList.Add(text);
			}
			*/
		}
	}


	[System.Serializable]
	public class Definition
	{
		public string name;
		public int id;
		public string content;

		public Definition(string name, int id, string content)
		{
			this.name = name;
			this.id = id;
			this.content = content;
		}
	}
}
