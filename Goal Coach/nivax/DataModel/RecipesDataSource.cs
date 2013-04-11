using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.ApplicationModel.Resources.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using System.Collections.Specialized;

// The data model defined by this file serves as a representative example of a strongly-typed
// model that supports notification when members are added, removed, or modified.  The property
// names chosen coincide with data bindings in the standard item templates.
//
// Applications may use this model as a starting point and build on it, or discard it entirely and
// replace it with something appropriate to their needs.

namespace HealthAndFitness.Data
{
    /// <summary>
    /// Base class for <see cref="RecipesDataItem"/> and <see cref="RecipesDataGroup"/> that
    /// defines properties common to both.
    /// </summary>
    [Windows.Foundation.Metadata.WebHostHidden]
    public abstract class RecipesDataCommon : HealthAndFitness.Common.BindableBase
    {
        private static Uri _baseUri = new Uri("ms-appx:///");
      
        public RecipesDataCommon(String uniqueId, String title, String subtitle, String imagePath, String description)
        {
            this._uniqueId = uniqueId;
            this._title = title;
            this._subtitle = subtitle;
            this._description = description;
            this._imagePath = imagePath;
        }

        private string _uniqueId = string.Empty;
        public string UniqueId
        {
            get { return this._uniqueId; }
            set { this.SetProperty(ref this._uniqueId, value); }
        }

        private string _title = string.Empty;
        public string Title
        {
            get { return this._title; }
            set { this.SetProperty(ref this._title, value); }
        }

        private string _subtitle = string.Empty;
        public string Subtitle
        {
            get { return this._subtitle; }
            set { this.SetProperty(ref this._subtitle, value); }
        }

        private string _description = string.Empty;
        public string Description
        {
            get { return this._description; }
            set { this.SetProperty(ref this._description, value); }
        }

        private ImageSource _image = null;
        private String _imagePath = null;
        public ImageSource Image
        {
            get
            {
                if (this._image == null && this._imagePath != null)
                {
                    this._image = new BitmapImage(new Uri(RecipesDataCommon._baseUri, this._imagePath));
                }
                return this._image;
            }

            set
            {
                this._imagePath = null;
                this.SetProperty(ref this._image, value);
            }
        }

        public void SetImage(String path)
        {
            this._image = null;
            this._imagePath = path;
            this.OnPropertyChanged("Image");
        }

        public override string ToString()
        {
            return this.Title;
        }
    }

    /// <summary>
    /// Generic item data model.
    /// </summary>
    public class RecipesDataItem : RecipesDataCommon
    {
        public RecipesDataItem(String uniqueId, String title, String subtitle, String imagePath, String description, String content, int colSpan, int rowSpan, RecipesDataGroup group)
            : base(uniqueId, title, subtitle, imagePath, description)
        {
            this._colSpan = colSpan;
            this._rowSpan = rowSpan;
            this._content = content;
            this._group = group;
        }

        private string _content = string.Empty;
        public string Content
        {
            get { return this._content; }
            set { this.SetProperty(ref this._content, value); }
        }

        private int _rowSpan = 1;
        public int RowSpan
        {
            get { return this._rowSpan; }
            set { this.SetProperty(ref this._rowSpan, value); }
        }

        private int _colSpan = 1;
        public int ColSpan
        {
            get { return this._colSpan; }
            set { this.SetProperty(ref this._colSpan, value); }
        }


        private RecipesDataGroup _group;
        public RecipesDataGroup Group
        {
            get { return this._group; }
            set { this.SetProperty(ref this._group, value); }
        }
    }

    /// <summary>
    /// Generic group data model.
    /// </summary>
    public class RecipesDataGroup : RecipesDataCommon
    {
        public RecipesDataGroup(String uniqueId, String title, String subtitle, String imagePath, String description)
            : base(uniqueId, title, subtitle, imagePath, description)
        {
            Items.CollectionChanged += ItemsCollectionChanged;
        }

        private void ItemsCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            // Provides a subset of the full items collection to bind to from a GroupedItemsPage
            // for two reasons: GridView will not virtualize large items collections, and it
            // improves the user experience when browsing through groups with large numbers of
            // items.
            //
            // A maximum of 12 items are displayed because it results in filled grid columns
            // whether there are 1, 2, 3, 4, or 6 rows displayed

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewStartingIndex < 12)
                    {
                        TopItems.Insert(e.NewStartingIndex, Items[e.NewStartingIndex]);
                        if (TopItems.Count > 12)
                        {
                            TopItems.RemoveAt(12);
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Move:
                    if (e.OldStartingIndex < 12 && e.NewStartingIndex < 12)
                    {
                        TopItems.Move(e.OldStartingIndex, e.NewStartingIndex);
                    }
                    else if (e.OldStartingIndex < 12)
                    {
                        TopItems.RemoveAt(e.OldStartingIndex);
                        TopItems.Add(Items[11]);
                    }
                    else if (e.NewStartingIndex < 12)
                    {
                        TopItems.Insert(e.NewStartingIndex, Items[e.NewStartingIndex]);
                        TopItems.RemoveAt(12);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    if (e.OldStartingIndex < 12)
                    {
                        TopItems.RemoveAt(e.OldStartingIndex);
                        if (Items.Count >= 12)
                        {
                            TopItems.Add(Items[11]);
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    if (e.OldStartingIndex < 12)
                    {
                        TopItems[e.OldStartingIndex] = Items[e.OldStartingIndex];
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    TopItems.Clear();
                    while (TopItems.Count < Items.Count && TopItems.Count < 12)
                    {
                        TopItems.Add(Items[TopItems.Count]);
                    }
                    break;
            }
        }

        private ObservableCollection<RecipesDataItem> _items = new ObservableCollection<RecipesDataItem>();
        public ObservableCollection<RecipesDataItem> Items
        {
            get { return this._items; }
        }

        private ObservableCollection<RecipesDataItem> _topItem = new ObservableCollection<RecipesDataItem>();
        public ObservableCollection<RecipesDataItem> TopItems
        {
            get { return this._topItem; }
        }
    }

    /// <summary>
    /// Creates a collection of groups and items with hard-coded content.
    /// 
    /// RecipesDataSource initializes with placeholder data rather than live production
    /// data so that sample data is provided at both design-time and run-time.
    /// </summary>
    public sealed class RecipesDataSource
    {
        private static RecipesDataSource _RecipesDataSource = new RecipesDataSource();

        private ObservableCollection<RecipesDataGroup> _allGroups = new ObservableCollection<RecipesDataGroup>();
        public ObservableCollection<RecipesDataGroup> AllGroups
        {
            get { return this._allGroups; }
        }

        public static IEnumerable<RecipesDataGroup> GetGroups(string uniqueId)
        {
            if (!uniqueId.Equals("AllGroups")) throw new ArgumentException("Only 'AllGroups' is supported as a collection of groups");

            return _RecipesDataSource.AllGroups;
        }

        public static RecipesDataGroup GetGroup(string uniqueId)
        {
            // Simple linear search is acceptable for small data sets
            var matches = _RecipesDataSource.AllGroups.Where((group) => group.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public static RecipesDataItem GetItem(string uniqueId)
        {
            // Simple linear search is acceptable for small data sets
            var matches = _RecipesDataSource.AllGroups.SelectMany(group => group.Items).Where((item) => item.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }


        public RecipesDataSource()
        {
            String ITEM_CONTENT = String.Format("Item Content: {0}\n\n{0}\n\n{0}\n\n{0}\n\n{0}\n\n{0}\n\n{0}",
                        "nigw nwg44r naveed");

            
            var group1 = new RecipesDataGroup("Group-1",
                "Services",
                "Group Subtitle: 2",
                "Assets/DarkGray.png",
                "Group Description: Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus tempor scelerisque lorem in vehicula. Aliquam tincidunt, lacus ut sagittis tristique, turpis massa volutpat augue, eu rutrum ligula ante a ante");

           
            group1.Items.Add(new RecipesDataItem("RecipesBig-Group-1-Item1",
                "Pomegranete",
                " Lettuce Salad",
                "Assets/HubPageImages/HubPageImage11.png",
                "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                ITEM_CONTENT,
                54,
                104,
                group1));
            group1.Items.Add(new RecipesDataItem("RecipesSmall-Group-1-Item2",
               "Very Berry",
               "shake",
               "Assets/HubPageImages/HubPageImage12.png",
               "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
               ITEM_CONTENT,
               49,
               35,
               group1));
            group1.Items.Add(new RecipesDataItem("RecipesSmall-Group-1-Item3",
               "Radish",
               "Cream salad",
               "Assets/HubPageImages/HubPageImage13.png",
               "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
               ITEM_CONTENT,
               49,
               34,
               group1));
            group1.Items.Add(new RecipesDataItem("RecipesSmall-Group-1-Item4",
               "Pasta",
               "-Vegetable Soup",
               "",
               "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
               ITEM_CONTENT,
               49,
               35,
               group1));
            group1.Items.Add(new RecipesDataItem("RecipesSmall-Group-1-Item5",
               "Brocolli",
               "Stir Fry",
               "Assets/HubPageImages/HubPageImage15.png",
               "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
               ITEM_CONTENT,
               49,
               35,
               group1));
            group1.Items.Add(new RecipesDataItem("RecipesMedium-Group-1-Item6",
               "Chilled",
               "Cucumber Mocktail",
               "Assets/HubPageImages/HubPageImage16.png",
               "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
               ITEM_CONTENT,
               49,
               69,
               group1));
            group1.Items.Add(new RecipesDataItem("RecipesSmall-Group-1-Item7",
               "Brocolli",
               "Stir Fry",
               "Assets/RecipesImages/RecipesImage1.png",
               "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
               ITEM_CONTENT,
               49,
               35,
               group1));
            group1.Items.Add(new RecipesDataItem("RecipesSmall-Group-1-Item8",
               "Mix Vegetable",
               "Broth",
               "",
               "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
               ITEM_CONTENT,
               49,
               34,
               group1));
            group1.Items.Add(new RecipesDataItem("RecipesSmall-Group-1-Item9",
               "Honey",
               "Grape Salad",
               "Assets/RecipesImages/RecipesImage2.png",
               "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
               ITEM_CONTENT,
               49,
               35,
               group1));
            group1.Items.Add(new RecipesDataItem("RecipesSmall-Group-1-Item10",
               "Pasta",
               "-Vegetable Soup",
               "",
               "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
               ITEM_CONTENT,
               51,
               35,
               group1));
            group1.Items.Add(new RecipesDataItem("BigP-Group-1-Item11",
               "Smoked",
               "Salmon",
               "Assets/RecipesImages/RecipesImage3.png",
               "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
               ITEM_CONTENT,
               100,
               69,
               group1));

            group1.Items.Add(new RecipesDataItem("RecipesSmall-Group-1-Item12",
              "Pasta",
              "-Vegetable Soup",
               "Assets/RecipesImages/RecipesImage4.png",
              "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
              ITEM_CONTENT,
              49,
              35,
              group1));
            group1.Items.Add(new RecipesDataItem("RecipesMedium-Group-1-Item13",
              "Egg",
              "Crouton salad",
               "Assets/RecipesImages/RecipesImage5.png",
              "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
              ITEM_CONTENT,
              49,
              69,
              group1));
            group1.Items.Add(new RecipesDataItem("RecipesSmall-Group-1-Item14",
               "Steamed",
               "Dumplings",
               "",
               "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
               ITEM_CONTENT,
               49,
               35,
               group1));


            this.AllGroups.Add(group1);

            




        }
    }
}
