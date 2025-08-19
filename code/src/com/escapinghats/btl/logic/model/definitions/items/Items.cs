using System.Collections.Generic;
using com.escapinghats.btl.logic.services;
using com.escapinghats.btl.logic.schemas.context.definitions;

namespace com.escapinghats.btl.logic.model.definitions.items
{
	public class Items
		{
			private Dictionary<int, Item> _items;
			private Dictionary<int, ItemType> _itemTypes;

		public Items()
		{
			_items = new Dictionary<int, Item>();
			_itemTypes = new Dictionary<int, ItemType>();
		}

		public bool Load(ModelLoader modelLoader)
		{

			if(modelLoader.FileType == "items"){
				string itemType = modelLoader.FileRemaining[0].Split('.')[0]; 
				Debug.Instance.Log("Items::Loading type" + itemType);

				switch(itemType){
				case "items":
					LoadItem(modelLoader);
					break;
				case "itemTypes":
					LoadItemType(modelLoader);
					break;
				default:
					return false;
				}
			}else{
				return false;
			}

			return true;
		}


		private void LoadItem(ModelLoader modelLoader)
		{
			ItemListSchema itemList = modelLoader.LoadData<ItemListSchema>();

			foreach (var item in itemList.items)
			{
				_items.Add(item.id, new Item(item.id, item.name, item.typeId));
			}

			Debug.Instance.Log("Items::First item: " + GetItemById(0)?.Name);
		}


		private void LoadItemType(ModelLoader modelLoader)
		{
			ItemTypeListSchema itemTypeList = modelLoader.LoadData<ItemTypeListSchema>();

			foreach (var itemType in itemTypeList.types)
			{
				_itemTypes.Add(itemType.id, new ItemType(itemType.id, itemType.name, itemType.parent));
			}

			Debug.Instance.Log("Items::First item type: " + GetItemTypeById(1)?.Name);
		}

		public ItemType GetItemTypeById(int id)
		{
			if(_itemTypes.ContainsKey(id)){
				return _itemTypes[id];
			}
			return null;
		}

		public Item GetItemById(int id)
		{
			if(_items.ContainsKey(id)){
				return _items[id];
			}
			return null;
		}
	}
}
