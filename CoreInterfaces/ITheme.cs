using System;
using System.Collections.Generic;

namespace StudyAssist.Core.Interfaces
{
    /// <summary>
    /// Интерфейс темы (подкатегории).
    /// </summary>
    public interface ITheme
    {
        #region Properties

        Int32 ThemeId { get; set; }

        /// <summary>
        /// Название темы.
        /// </summary>
        String Name { get; set; }

        /// <summary>
        /// Признак того, находится ли тема на изучении.
        /// </summary>
        Boolean IsStudy { get; set; }

        /// <summary>
        /// Коллекция вопросов темы.
        /// </summary>
        IEnumerable<IProblem> Problems { get; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Удаляет тему с обучения.
        /// </summary>
        void RemoveFromStudy();

        /// <summary>
        /// Добавляет проблему в коллекцию проблем.
        /// </summary>
        /// <param name="addedItem"></param>
        void AddProblem(IProblem addedItem);

        #endregion Methods
    }
}
