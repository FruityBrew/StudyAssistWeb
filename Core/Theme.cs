using StudyAssist.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudyAssist.Core
{
    public class Theme : ITheme
    {
        #region fields   

        private String _name;

        private Boolean _isStudy;

        private List<IProblem> _problems;

        #endregion

        #region properties

        /// <summary>
        /// Показывает находится ли тема на изучении.
        /// </summary>
        public bool IsStudy
        {
            get { return _isStudy; }
            set { _isStudy = value; }
        }

        /// <summary>
        /// Название темы.
        /// </summary>
        public string Name
        {
            get { return _name; }

            set
            {
                if (String.IsNullOrEmpty(value))
                    throw new ArgumentException(
                        "Название темы на может быть пустым");
                else
                    _name = value;
            }
        }

        /// <summary>
        /// Коллекция проблем.
        /// </summary>
        public IEnumerable<IProblem> Problems
        {
            get { return _problems; }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Конструктор.
        /// </summary>
        public Theme()
        {
            _problems = new List<IProblem>();
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Удаляет тему и все проблемы темы. 
        /// </summary>
        public void RemoveFromStudy()
        {
            IsStudy = false;

            if (_problems == null)
                return;

            Parallel.ForEach(_problems, (a) => a.IsStudy = false);
        }

        /// <summary>
        /// Добавляет проблему к списку.
        /// </summary>
        /// <param name="addedItem"></param>
        public void AddProblem(IProblem addedItem)
        {
            if(_problems != null)
                _problems.Add(addedItem);
        }

        #endregion Methods
    }
}
