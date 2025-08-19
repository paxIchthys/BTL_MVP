namespace com.escapinghats.btl.logic.model.characters
{
    public class Character
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Character(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
