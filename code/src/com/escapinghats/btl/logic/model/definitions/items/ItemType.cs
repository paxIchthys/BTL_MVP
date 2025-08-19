namespace com.escapinghats.btl.logic.model.definitions.items
{
    public class ItemType
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public int ParentId { get; private set; }

        public ItemType(int id, string name, int parentId)
        {
            Id = id;
            Name = name;
            ParentId = parentId;
        }
    }
}
