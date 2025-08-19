namespace com.escapinghats.btl.logic.model.text
{
    public class Text
    {
        public int id { get; set; }
        public string name { get; set; }
        public string content { get; set; }

        public Text(int id, string name, string content)
        {
            this.id = id;
            this.name = name;
            this.content = content;
        }
    }
}
