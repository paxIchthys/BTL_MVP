using System.Collections.Generic;
using com.escapinghats.btl.logic.services;
using com.escapinghats.btl.logic.schemas.context.definitions;

namespace com.escapinghats.btl.logic.model.characters
{
	public class Characters
	{
		private Dictionary<int, Character> _characters = new Dictionary<int, Character>();
		public CharacterListSchema CharacterList;

		public Characters()
		{
			CharacterList = null;
		}

		public void Load(ModelLoader modelLoader)
		{
			CharacterListSchema characterList = modelLoader.LoadData<CharacterListSchema>();
			Debug.Instance.Log("Characters::Loading 1st character schema: " + characterList.characters[0].name);

			for (int i = 0; i < characterList.characters.Count; i++)
			{
				_characters.Add(characterList.characters[i].id, new Character(characterList.characters[i].id, characterList.characters[i].name));
			}
			
			Debug.Instance.Log("Characters::Loading 1st character: " + this.GetById(0));
		}

		public Character GetById(int id)
		{
			if (_characters.ContainsKey(id))
			{
				return _characters[id];
			}

			return null;
		}
	}

}
