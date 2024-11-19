using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Formats.Nrbf;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using StudyAssist.Model;
using StudyAssistInterfaces;
using StudyAssistModel;
using Utilities.Interfaces;

namespace StudyAssist.KnowledgeDataConverter.Api
{
    internal static class FromFileToDbConvertor
    {
        private static string _knowledgeDirectoryPath = 
            @"D:\OneDrive\_Код\StudyAssistRelease_Actual_V_3\CategoriesStorage";

        internal static async Task Convert()
        {
            List<int> catalogsIds = [15];

            IEnumerable<FileInfo> files = _GetDataFiles();
            IEnumerable<ICategory> oldModels = _ConvertFileDataToOldModel(files);
            IEnumerable<Catalog> actualModels = _ConvertOldModelToModel(oldModels)
                .ToList();

            IKnowledgeDataProvider dataProvider = new KnowledgeDataProvider();
            await dataProvider.SaveCatalog(actualModels.First());
        }

        private static IEnumerable<FileInfo> _GetDataFiles()
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

        private static IEnumerable<ICategory> _ConvertFileDataToOldModel(
            IEnumerable<FileInfo> files)
        {
            List<ICategory> categories = new List<ICategory>();

            foreach(FileInfo file in files.Take(2))
            {
                ICategory xCategory = new XCategory();
#pragma warning disable SYSLIB5005 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
                var binaryCategory = NrbfDecoder.DecodeClassRecord(file.OpenRead());
                var b = binaryCategory.MemberNames;
                string? categoryName = binaryCategory?.GetString("_name");
                
                xCategory.Name = categoryName;

                var binaryThemesRecords = binaryCategory
                    .GetClassRecord("_themes")
                    .GetClassRecord("Collection`1+items")
                    .GetArrayRecord("_items");
                
                var binaryThemesArray = binaryThemesRecords.GetArray(typeof(ITheme[]));

                foreach(ClassRecord theme in binaryThemesArray)
                {
                    if(theme == null) continue;

                    string themeName = theme.GetString("_name");

                    ITheme xTheme = new XTheme();
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

                        IProblem xProblem = new XProblem();
                        xProblem.Question = problemQuestion;
                        xProblem.Answer = promlemAnswer; 
                        
                        xTheme.Problems.Add(xProblem);
                    }

                    xCategory.Themes.Add(xTheme);
                }

                var y = b.ToString();
#pragma warning restore SYSLIB5005 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

                categories.Add(xCategory);
            }


            return categories;
        }

        private static IEnumerable<Catalog> _ConvertOldModelToModel(
            IEnumerable<ICategory> source)
        {
            foreach (ICategory category in source)
                yield return _ConvertToCatalog(category);
        }

        private static Catalog _ConvertToCatalog(ICategory source)
        {
            Catalog target = new()
            {
                Name = source.Name,
                Themes = source.Themes
                    .Select(_ConvertToTheme)
                    .ToList(),
            };

            return target;
        }

        private static Theme _ConvertToTheme(ITheme source)
        {
            Theme target = new()
            {
                Name = source.Name,
                Issues = source.Problems
                    .Select(_ConvertToIssue)
                    .ToList()
            };

            return target;
        }

        private static Issue _ConvertToIssue(IProblem source)
        {
            Issue target = new()
            {
                Question = source.Question,
                Answer = source.Answer,
            };

            return target;
        }
    }
}
