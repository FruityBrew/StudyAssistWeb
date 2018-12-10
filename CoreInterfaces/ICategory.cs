using System;
using System.Collections.Generic;
using System.Text;

namespace StudyAssist.Core.Interfaces
{
    /// <summary>
    /// Категория - контейнер для тем.
    /// </summary>
    public interface ICategory
    {
        #region Properties

        /// <summary>
        /// Имя категории.
        /// </summary>
        String Name { get; set; }

        /// <summary>
        /// Коллекция тем.
        /// </summary>
        IEnumerable<ITheme> Themes { get; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Удаляет категорию с обучения.
        /// </summary>
        void RemoveFromStudy();

        /// <summary>
        /// Добавляет тему в коллекцию тем.
        /// </summary>
        /// <param name="addedItem">Добавляемая тема.</param>
        void AddTheme(ITheme addedItem);

        #endregion Methods
    }
}
