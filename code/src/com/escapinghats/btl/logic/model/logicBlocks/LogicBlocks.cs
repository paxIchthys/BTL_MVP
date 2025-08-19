using System.Collections.Generic;

namespace com.escapinghats.btl.logic.model.logicBlocks
{
	public class LogicBlocks
	{
		private Dictionary<int, LogicBlock> _logicBlocks = new Dictionary<int, LogicBlock>();

		public void AddLogicBlock(ModelLoader modelLoader)
		{
			//_logicBlocks.Add(logicBlock.Id, logicBlock);
		}

		public LogicBlock GetLogicBlock(int id)
		{
			return _logicBlocks[id];
		}
	}

    public class LogicBlock
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }

        public LogicBlock(int id, string name, string content)
        {
            Id = id;
            Name = name;
            Content = content;
        }
    }
}
