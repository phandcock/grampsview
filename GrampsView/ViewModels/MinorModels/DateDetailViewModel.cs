﻿namespace GrampsView.ViewModels
{
    using GrampsView.Common;
    using GrampsView.Data.Model;

    /// <summary>
    /// ViewModel for the Address Detail page.
    /// </summary>
    public class DateDetailViewModel : ViewModelBase
    {
        public DateDetailViewModel(ICommonLogging iocCommonLogging)
            : base(iocCommonLogging)
        {
            BaseTitle = "Date Detail";
            BaseTitleIcon = CommonConstants.IconDDefault;
        }

        /// <summary>
        /// Gets or sets the View Current Person.
        /// </summary>
        /// <value>
        /// The current person ViewModel.
        /// </value>
        public DateObjectModel DateObject
        {
            get; set;
        }

        /// <summary>
        /// Populates the view ViewModel.
        /// </summary>
        /// <returns>
        /// </returns>
        public override void BaseHandleLoadEvent()
        {
            BaseCL.RoutineEntry("DateDetailViewModel");

            HLinkDateModel HLinkObject = CommonRoutines.GetHLinkParameter<HLinkDateModel>(BaseParamsHLink);

            BaseTitle = HLinkObject.Title;

            DateObject = HLinkObject.DeRef;

            if (DateObject.Valid)
            {
                BaseTitle = DateObject.GetDefaultText;

                /*
                 * General Details
                 */

                // Get the Date Details
                BaseDetail.Add(new CardListLineCollection("Date")
                {
                    new CardListLine("Short Date:", DateObject.ShortDate),
                    new CardListLine("Long Date:", DateObject.LongDate),
                    new CardListLine("Age:", DateObject.GetAge),
                    new CardListLine("Valid:", DateObject.Valid),
                });

                BaseDetail.Add(new CardListLineCollection("Date Detail")
                {
                    new CardListLine("Decade:", DateObject.GetDecade),
                    new CardListLine("Month Day:", $"{DateObject.GetMonthDay:MM dd}"),
                    new CardListLine("Year:", DateObject.GetYear),
                });

                BaseDetail.Add(new CardListLineCollection("Date Internal")
                {
                    new CardListLine("Default Date:", DateObject.GetDefaultText),
                    new CardListLine("Notional Date:", DateObject.NotionalDate.ToString()),
                    new CardListLine("Single Date:", DateObject.SingleDate.ToString()),
                    new CardListLine("Sort Date:", DateObject.SortDate.ToString()),
                });

                /*
                 * Date Model specific details
                 */

                switch (DateObject)
                {
                    case DateObjectModelRange i:
                        {
                            // TODO Finish this
                            break;
                        }

                    case DateObjectModelSpan i:
                        {
                            // TODO Finish this
                            break;
                        }

                    case DateObjectModelStr i:
                        {
                            // TODO Finish this
                            break;
                        }

                    case DateObjectModelVal i:
                        {
                            BaseDetail.Add(new CardListLineCollection("Date Val Type")
                            {
                                new CardListLine("CFormat:", (DateObject as IDateObjectModelVal).GCformat),
                                new CardListLine("Dual Dated:", (DateObject as IDateObjectModelVal).GDualdated),
                                new CardListLine("New Year:",  (DateObject as IDateObjectModelVal).GNewYear),
                                new CardListLine("Quality:",  (DateObject as IDateObjectModelVal).GQuality.ToString()),
                                new CardListLine("Val:",  (DateObject as IDateObjectModelVal).GVal),
                            });

                            break;
                        }

                    default:
                        {
                            // TODO Finish this
                            break;
                        }
                }
            }

            return;
        }
    }
}