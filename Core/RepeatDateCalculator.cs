using System;
using StudyAssist.Core.Interfaces;

namespace StudyAssist.Core
{
    public class RepeatDateCalculator : IRepeatCalculator
    {
        #region Fields

        /// <summary>
        /// Период первого повтора.
        /// </summary>
        public int FirstRepeatPeriod { get; } = 1;

        public int SecondRepeatPeriod { get; } = 3;

        public int ThirdRepeatPeriod { get; } = 7;

        public int FourthRepeatPeriod { get; } = 14;

        public int FifthRepeatPeriod { get; } = 30;

        public int DefaultRepeatPeriod { get; } = 45;

        #endregion Fields

        #region Methods

        public DateTime GetRepeatDate(
            Int32 studyLevel, DateTime? oldRepeateDate)
        {
            if (oldRepeateDate.HasValue == false)
                throw new ArgumentNullException(nameof(oldRepeateDate));

            DateTime? repeateDate = DateTime.Today;

            switch (studyLevel)
            {
                case 1:
                    if (oldRepeateDate > DateTime.Today)
                        break;
                    repeateDate = oldRepeateDate?.AddDays(FirstRepeatPeriod);
                    break;
                case 2:
                    if (oldRepeateDate > DateTime.Today)
                        break;
                    repeateDate = oldRepeateDate?.AddDays(SecondRepeatPeriod);
                    break;
                case 3:
                    if (oldRepeateDate > DateTime.Today)
                        break;
                    repeateDate = oldRepeateDate?.AddDays(ThirdRepeatPeriod);
                    break;
                case 4:
                    if (oldRepeateDate > DateTime.Today)
                        break;
                    repeateDate = oldRepeateDate?.AddDays(FourthRepeatPeriod);
                    break;
                case 5:
                    if (oldRepeateDate > DateTime.Today)
                        break;
                    repeateDate = oldRepeateDate?.AddDays(FifthRepeatPeriod);
                    break;
                //case 6:
                //    _repeateDate = _repeateDate.AddDays(45);
                //break;
                default:
                    repeateDate = oldRepeateDate?.AddDays(DefaultRepeatPeriod);
                    break;
            }

            return repeateDate.Value;
        }

        #endregion Methods
    }
}
