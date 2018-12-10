using System;
using System.Collections.Generic;
using System.Text;
using StudyAssist.Core.Interfaces;

namespace StudyAssist.Core
{
    public class Category : ICategory
    {
        #region Fields

        String _name;

        private List<ITheme> _themes;

        #endregion

        #region ctors

        public Category()
        {
            _themes = new List<ITheme>();
        }

        #endregion

        #region properties

        /// <summary>
        /// Название категории.
        /// </summary>
        public string Name
        {
            get { return _name; }

            set
            {
                if (String.IsNullOrWhiteSpace(value))
                    throw new ArgumentException(
                        "Название категории не может быть пустым");

                _name = value;
            }
        }

        /// <summary>
        /// Список тем. //TODO переделать на список?
        /// </summary>
        public IEnumerable<ITheme> Themes
        {
            get { return _themes; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// TODO
        /// </summary>
        public void RemoveFromStudy()
        {
            throw new NotImplementedException();
        }

        public void AddTheme(ITheme addedItem)
        {
            _themes?.Add(addedItem);
        }

        #endregion Methods
    }
}
