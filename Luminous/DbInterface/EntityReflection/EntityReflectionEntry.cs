namespace LangM.AspNetCore.DbInterface
{
    public class EntityReflectionEntry
    {
        public EntityReflectionEntry(string tableName, EntityPropertyMetadata[] propertyMetadata, EntityPropertyFlags entityProperties)
        {
            TableName = tableName;
            PropertyMetadata = propertyMetadata;
            EntityProperties = entityProperties;
        }

        public string TableName { get; set; }
        public EntityPropertyMetadata[] PropertyMetadata { get; set; }
        public EntityPropertyFlags EntityProperties { get; set; }
    }
}
