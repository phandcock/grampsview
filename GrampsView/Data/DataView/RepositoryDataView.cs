using GrampsView.Common;
using GrampsView.Common.CustomClasses;
using GrampsView.Data.Collections;
using GrampsView.Data.Model;
using GrampsView.Data.Repository;

using System.Collections;
using System.Globalization;

namespace GrampsView.Data.DataView
{
    /// <summary>
    ///   <br />
    /// </summary>
    /// <seealso cref="IRepositoryDataView" />
    public class RepositoryDataView : DataViewBase<RepositoryModel, HLinkRepositoryModel, HLinkRepositoryModelCollection>, IRepositoryDataView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryDataView"/> class.
        /// </summary>
        public RepositoryDataView()
        {
        }

        public override IReadOnlyList<RepositoryModel> DataDefaultSort => DataViewData.OrderBy(RepositoryModel => RepositoryModel.GRName).ToList();

        /// <summary>
        /// Gets the local repository data.
        /// </summary>
        /// <summary>
        /// Gets or sets the data view data.
        /// </summary>
        /// <value>
        /// The data view data.
        /// </value>
        public override IReadOnlyList<RepositoryModel> DataViewData => RepositoryData.Values.ToList();

        public override HLinkRepositoryModelCollection GetLatestChanges
        {
            get
            {
                DateTime lastSixtyDays = DateTime.Now.Subtract(new TimeSpan(60, 0, 0, 0, 0));

                IEnumerable tt = DataViewData.OrderByDescending(GetLatestChangest => GetLatestChangest.Change).Where(GetLatestChangestt => GetLatestChangestt.Change > lastSixtyDays).Take(3);

                HLinkRepositoryModelCollection returnCardGroup = new();

                foreach (RepositoryModel item in tt)
                {
                    returnCardGroup.Add(item.HLink);
                }

                returnCardGroup.Title = "Latest Repository Changes";

                return returnCardGroup;
            }
        }

        /// <summary>
        /// Gets or sets the person data.
        /// </summary>
        /// <value>
        /// The person data.
        /// </value>

        public RepositoryModelDictionary<RepositoryModel, HLinkRepositoryModel> RepositoryData => DataStore.Instance.DS.RepositoryData;

        // TODO cleanup up this code
        public override HLinkRepositoryModelCollection GetAllAsCardGroupBase()
        {
            HLinkRepositoryModelCollection t = new();

            foreach (RepositoryModel item in DataDefaultSort)
            {
                t.Add(item.HLink);
            }

            // Sort TODO Sort t = HLinkCollectionSort(t);

            return t;
        }

        public override Group<HLinkRepositoryModelCollection> GetAllAsGroupedCardGroup()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets all as hlink.
        /// </summary>
        /// <returns>
        /// </returns>
        public HLinkRepositoryModelCollection GetAllAsHLink()
        {
            HLinkRepositoryModelCollection t = new();

            foreach (RepositoryModel item in DataDefaultSort)
            {
                t.Add(item.HLink);
            }

            return t;
        }

        public override RepositoryModel GetModelFromHLinkKey(HLinkKey argHLinkKey)
        {
            return RepositoryData[argHLinkKey.Value];
        }

        public override RepositoryModel GetModelFromId(string argId)
        {
            return DataViewData.Where(X => X.Id == argId).FirstOrDefault();
        }

        /// <summary>
        /// hes the link collection sort.
        /// </summary>
        /// <param name="collectionArg">
        /// The collection argument.
        /// </param>
        /// <returns>
        /// Sorted hlink collection.
        /// </returns>
        public override HLinkRepositoryModelCollection HLinkCollectionSort(HLinkRepositoryModelCollection collectionArg)
        {
            if (collectionArg == null)
            {
                return null;
            }

            IOrderedEnumerable<HLinkRepositoryModel> t = collectionArg.OrderBy(HLinkRepositoryModel => HLinkRepositoryModel.DeRef.GRName);

            HLinkRepositoryModelCollection tt = new();

            foreach (HLinkRepositoryModel item in t)
            {
                tt.Add(item);
            }

            return tt;
        }

        public override HLinkRepositoryModelCollection Search(string argQuery)
        {
            HLinkRepositoryModelCollection itemsFound = new()
            {
                Title = "Repositories"
            };

            if (string.IsNullOrEmpty(argQuery))
            {
                return itemsFound;
            }

            IOrderedEnumerable<RepositoryModel> temp = DataViewData.Where(x => x.ToString().ToLower(CultureInfo.CurrentCulture).Contains(argQuery)).OrderBy(y => y.ToString());

            foreach (RepositoryModel tempMO in temp)
            {
                itemsFound.Add(tempMO.HLink);
            }

            return itemsFound;
        }
    }
}