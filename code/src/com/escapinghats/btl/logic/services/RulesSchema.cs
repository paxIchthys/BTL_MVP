using System;

namespace com.escapinghats.btl.logic.services
{
    [Serializable]
    public class RulesSchema
    {
        public string Name { get; set; }
        
        public static readonly string[] FILES = 
        {
            "definitions/characters/characterList.json", 
            "definitions/items/items.json",
            "definitions/items/itemTypes.json",
            "logicBlocks/rules.json"
        };

        public static readonly string[] LOCALIZED_FILES = 
        {
            "text/en_us/dialogue.json",
            "text/en_us/labels.json"
        };    
    }
}