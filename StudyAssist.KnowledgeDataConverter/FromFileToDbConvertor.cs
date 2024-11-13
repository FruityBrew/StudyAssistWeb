using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Formats.Nrbf;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using StudyAssistInterfaces;
using StudyAssistModel;

namespace StudyAssist.KnowledgeDataConverter
{
    internal static class FromFileToDbConvertor
    {
        private static string _knowledgeDirectoryPath = 
            @"D:\OneDrive\_Код\StudyAssistRelease_Actual_V_3\CategoriesStorage";

        internal static void Convert()
        {
            List<int> catalogsIds = [15];

            var res = ConvertFileDataToOldModel(
                GetDataFiles());

                

        }

        private static IEnumerable<FileInfo> GetDataFiles()
        {
            try
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(_knowledgeDirectoryPath);
                return directoryInfo.GetFiles();
            }
            catch(Exception ex) 
            {
                Console.WriteLine(
                    $"Не могу получить файлы из {_knowledgeDirectoryPath}: {ex.Message}");

                return [];
            }
        }

        private static IEnumerable<XCategory> ConvertFileDataToOldModel(
            IEnumerable<FileInfo> files)
        {
            List<XCategory> categories = new List<XCategory>();

            foreach(FileInfo file in files.Take(2))
            {
                XCategory xCategory = new ();
#pragma warning disable SYSLIB5005 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
                var binaryCategory = NrbfDecoder.DecodeClassRecord(file.OpenRead());
                var b = binaryCategory.MemberNames;
                string? categoryName = binaryCategory?.GetString("_name");
                
                xCategory.Name = categoryName;

                //object? x = d.GetRawValue("_themes");
                //var ddd = x as ObservableCollection<ITheme>;

                //var binaryThemes = binaryCategory.GetClassRecord("_themes");
                var binaryThemesRecords = binaryCategory
                    .GetClassRecord("_themes")
                    .GetClassRecord("Collection`1+items")
                    .GetArrayRecord("_items");
                
                var binaryThemesArray = binaryThemesRecords.GetArray(typeof(ITheme[]));

                foreach(ClassRecord theme in binaryThemesArray)
                {
                    if(theme == null) continue;

                    string themeName = theme.GetString("_name");

                    XTheme xTheme = new XTheme();
                    xTheme.Name = themeName;

                    var binaryProblemRecords = theme
                      .GetClassRecord("_problems")
                      .GetClassRecord("Collection`1+items")
                      .GetArrayRecord("_items");

                    var binaryProblemArray = binaryProblemRecords.GetArray(typeof(IProblem[]));

                    foreach(ClassRecord problem in binaryProblemArray)
                    {
                        if(problem == null) continue;

                        string problemQuestion = problem.GetString("_question");
                        string promlemAnswer = problem.GetString("_answer");

                        XProblem xProblem = new XProblem();
                        xProblem.Question = problemQuestion;
                        xProblem.Answer = promlemAnswer; 
                        
                        xTheme.Problems.Add(xProblem);
                    }

                    xCategory.Themes.Add(xTheme);
                }

                //for(int i = 0; i < binaryThemesArray.Length; i++)
                //{
                //    var d = binaryThemesArray.
                //}
                //var u = th.GetArrayRecord("_themes");

                var y = b.ToString();
#pragma warning restore SYSLIB5005 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

                categories.Add(xCategory);
            }


            return categories;
        }
    }
}
