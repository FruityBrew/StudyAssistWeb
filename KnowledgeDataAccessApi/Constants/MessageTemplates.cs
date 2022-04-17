namespace KnowledgeDataAccessApi.Constants
{
    public static class MessageTemplates
    {
        public const string DB_ENTITYID_RULE = "Id must be set by database";

        public const string QUESTION_TEXT_CONSTRAINT_ERR = 
            "Question should not be empty and length less than 200 characters"; //todo

        public const string JSON_UPD_PATCH_ERR = "JSonUpdatePatch is incorrect";

        public const string STUDY_LEVEL_CONSTRAINT_ERR = "Study level must be non negative";

        public const string REPEATE_DATE_CONSTRAINT_ERR = "Repeate date must not be past";

    }
}
