namespace KnowledgeDataAccessApi.Constants
{
    public static class MessageTemplates
    {
        public static string DB_ENTITYID_RULE = "Id must be set by database";

        public static string QUESTION_TEXT_CONSTRAINT_ERR = 
            "Question should not be empty and length less than 200 characters"; //todo

        public static string JSON_UPD_PATCH_ERR = "JSonUpdatePatch is incorrect";
    }
}
