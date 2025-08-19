namespace com.escapinghats.btl.logic.model.definitions.items
{
	public class Item
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int TypeId { get; set; }

		public Item(int id, string name, int typeId)
		{
			Id = id;
			Name = name;
			TypeId = typeId;
		}
	}
}
