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
    /// Base class for <see cref="SampleDataItem"/> and <see cref="SampleDataGroup"/> that
    /// defines properties common to both.
    /// </summary>
    [Windows.Foundation.Metadata.WebHostHidden]
    public abstract class SampleDataCommon : HealthAndFitness.Common.BindableBase
    {
        private static Uri _baseUri = new Uri("ms-appx:///");
      
        public SampleDataCommon(String uniqueId, String title, String subtitle, String imagePath, String description)
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
                    this._image = new BitmapImage(new Uri(SampleDataCommon._baseUri, this._imagePath));
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
    public class SampleDataItem : SampleDataCommon
    {
        public SampleDataItem(String uniqueId, String title, String subtitle, String imagePath, String description, String content, int colSpan, int rowSpan, SampleDataGroup group)
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


        private SampleDataGroup _group;
        public SampleDataGroup Group
        {
            get { return this._group; }
            set { this.SetProperty(ref this._group, value); }
        }
    }

    /// <summary>
    /// Generic group data model.
    /// </summary>
    public class SampleDataGroup : SampleDataCommon
    {
        public SampleDataGroup(String uniqueId, String title, String subtitle, String imagePath, String description)
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

        private ObservableCollection<SampleDataItem> _items = new ObservableCollection<SampleDataItem>();
        public ObservableCollection<SampleDataItem> Items
        {
            get { return this._items; }
        }

        private ObservableCollection<SampleDataItem> _topItem = new ObservableCollection<SampleDataItem>();
        public ObservableCollection<SampleDataItem> TopItems
        {
            get { return this._topItem; }
        }
    }

    /// <summary>
    /// Creates a collection of groups and items with hard-coded content.
    /// 
    /// SampleDataSource initializes with placeholder data rather than live production
    /// data so that sample data is provided at both design-time and run-time.
    /// </summary>
    public sealed class SampleDataSource
    {
        private static SampleDataSource _sampleDataSource = new SampleDataSource();

        private ObservableCollection<SampleDataGroup> _allGroups = new ObservableCollection<SampleDataGroup>();
        public ObservableCollection<SampleDataGroup> AllGroups
        {
            get { return this._allGroups; }
        }

        public static IEnumerable<SampleDataGroup> GetGroups(string uniqueId)
        {
            if (!uniqueId.Equals("AllGroups")) throw new ArgumentException("Only 'AllGroups' is supported as a collection of groups");

            return _sampleDataSource.AllGroups;
        }

        public static SampleDataGroup GetGroup(string uniqueId)
        {
            // Simple linear search is acceptable for small data sets
            var matches = _sampleDataSource.AllGroups.Where((group) => group.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public static SampleDataItem GetItem(string uniqueId)
        {
            // Simple linear search is acceptable for small data sets
            var matches = _sampleDataSource.AllGroups.SelectMany(group => group.Items).Where((item) => item.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }


        public SampleDataSource()
        {
            String ITEM_CONTENT = String.Format("Item Content: {0}\n\n{0}\n\n{0}\n\n{0}\n\n{0}\n\n{0}\n\n{0}",
                        "Nivax Sample Data Source");

            var group1 = new SampleDataGroup("Group-1",
                 "Introduction",
                 "Group Subtitle: 1",
                 "Assets/DarkGray.png",
                 "Group Description: Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus tempor scelerisque lorem in vehicula. Aliquam tincidunt, lacus ut sagittis tristique, turpis massa volutpat augue, eu rutrum ligula ante a ante");

            group1.Items.Add(new SampleDataItem("BigL-Group-1-Item1",
                 "The Way",
                 "",
                 "Assets/HubPageImages/HubPageImage1.png",
                 "Every person in the world devotes countless hours to thinking of their future and their present situation in life. Almost everyone wishes that there was something  that they could change in their life.",
                 "\n\nVery person in the world devotes countless hours to thinking of their future and their present situation in life. Almost everyone wishes that there was something  that they could change in their life. Whether it is their family life; their friendships; relationships or finances, everyone wants to change something. The first step to doing that is to set goals.\n\nHowever, many of us are great at trying to set goals; most of us are practically incapable of following through with them. Think about it. How many times have you decided on a course of action and simply didn’t follow through with it? \n\nThat is pretty much the norm for most people. Sometimes even setting goals at all is the harder part of accomplishing any.  The easiest way of looking at this is to think of each and every New Year. The largest part of New Year’s celebrations are not the parties and the gettogethers; it is actually in the resolutions. As much as we all like to attend and talk about New Years Eve parties, the most common source of conversation is the resolutions for the new year.",
                 67,
                 104,
                 group1));
            this.AllGroups.Add(group1);

            var group2 = new SampleDataGroup("Group-2",
                "Focus on your goal",
                "Group Subtitle: 2",
                "Assets/DarkGray.png",
                "Group Description: Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus tempor scelerisque lorem in vehicula. Aliquam tincidunt, lacus ut sagittis tristique, turpis massa volutpat augue, eu rutrum ligula ante a ante");

            group2.Items.Add(new SampleDataItem("ServiceL-Group-2-Item1",
                "How to Choose the Right Goals to Focus?",
                "",
                "Assets/HubPageImages/HubPageImage2.png",
                "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                "\n\n\n\n\n\n\n\n\n\nSometimes setting goals alone is not the only problem that you must face.Sometimes, choosing the right goals to begin with is harder. Basically, you can choose to work any goal that you feel is necessary for your health, stability and happiness. Goal setting is nothing more than a formal process for personal planning. By setting goals on a routine basis you decide what you want to achieve, and then move in a step-by-step manner towards the achievement of these goals. The process of setting goals and targets allows you to choose where you want to go in life. By  knowing exactly what you want to achieve, you know what you have to concentrate on to do it. You also know what nothing more than a distraction is.",
                43,
                26,
                group2));
            group2.Items.Add(new SampleDataItem("ServiceL-Group-2-Item2",
               "Goals Plan Setting",
               "",
               "Assets/HubPageImages/HubPageImage3.png",
               "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
               "\n\n\n\n\n\n\n\n\n\nThis section explains how to set personal goals. It starts with your lifetime goals, and then works through a series of lower level plans culminating in a daily to -do list. By setting up  this structure of plans you can break even the biggest life goal down into a number of small tasks that you need to do each day to reach the lifetime goals.\n\n The first step in setting personal goals is to consider what you want to achieve in your lifetime, as setting lifetime goals that gives you the overall perspective that shapes  all other aspects of your decision making process.",
               43,
               26,
               group2));
            group2.Items.Add(new SampleDataItem("ServiceP-Group-2-Item3",
               "Begin to achieve your Goals",
               "",
               "Assets/HubPageImages/HubPageImage4.png",
               "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
               "\n\n\n\n\n\n\n\n\n\nOnce you have set your lifetime goals, the best thing that you can do is set a 25 year plan of smaller goals that you should complete if you are to reach your lifetime plan. From there you can just shorten your overall goal spans for example, you set a 5 year plan, 1 year plan, 6 month plan, and 1 month plan of progressively smaller goals that you should reach to achieve your lifetime goals. \n\nEach of these should be based on the previous plan. It is the best way to begin to achieve a lifetime that is filled with and results in a life without any failed wishes. It results in a life without regret. You see, by starting out slowly, you are giving yourself the chance to realize and work on achieving the goals that you set out to. ",
               26,
               52,
               group2));
            group2.Items.Add(new SampleDataItem("ServiceL-Group-2-Item4",
               "Setting your Goals Effectively",
               "",
               "Assets/HubPageImages/HubPageImage5.png",
               "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
               "\n\n\n\n\n\n\n\n\n\nThere is a difference in setting your goals and setting them effectively. Anyone can set a goal, but doing it effectively means that it will actually get done. \n\nThere are so many things that you can do to better your life, but if you don’t know how to go about it you are stuck.\n\nThe following guidelines will help you to set effective goals and help you manage your time in an efficient manner that will cause those goals to become reality.",
               43,
               26,
               group2));
            group2.Items.Add(new SampleDataItem("ServiceL-Group-2-Item5",
               "State Each Goal",
               "",
               "Assets/HubPageImages/HubPageImage6.png",
               "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
               "\n\n\n\n\n\n\n\n\n\nExpress your goals in a positive way. That is a key component to setting goals that you can attain. How often have you been excited to accomplish a goal that didn’t even sound good when you brought it up? If you are not comfortable or happy with the goals that you have set, the likelihood of you succeeding is pretty low. \n\nIf you want to express your goals in a positive way, you simply have to first think  of a goal that puts a smile on your face when you imagine it completed. Why would you want to set a goal that made you frown, cringe or cry? \n\nWhen you are beginning to set your goals it helps when you are talking about them to others in a manner that states your actions as positives because it will have others seeing it as a positive as well. That will garner you a great deal more support. In the end, don’t we all need a little support when we are trying to do something positive in our lives? ",
               43,
               26,
               group2));
            group2.Items.Add(new SampleDataItem("ServiceP-Group-2-Item6",
               "Be Precise",
               "",
               "Assets/HubPageImages/HubPageImage7.png",
               "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
               "\n\n\n\n\n\n\n\n\n\nSet a precise goal that includes  starting dates, times and amounts so that you can properly measure your achievement. If you do this, you will know exactly when you have achieved the goal, and can take complete satisfaction from having achieved it.  \n\nBeing precise in setting your goals is no more than setting them with exact details. It is easier this way because then you can follow a step-by-step format. That’s all there is to it. ",
               26,
               52,
               group2));

            this.AllGroups.Add(group2);

            var group3 = new SampleDataGroup("Group-3",
                "Set Priorities",
                "Group Subtitle: 2",
                "Assets/DarkGray.png",
                "Group Description: Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus tempor scelerisque lorem in vehicula. Aliquam tincidunt, lacus ut sagittis tristique, turpis massa volutpat augue, eu rutrum ligula ante a ante");

            group3.Items.Add(new SampleDataItem("GearSmall-Group-3-Item1",
                "Performance",
                "",
                "Assets/HubPageImages/HubPageImage8.png",
                "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                "\n\n\n\n\n\n\n\n\n\nYou should take care to set goals over which you have as much control as possible. There is nothing more dispiriting than failing to achieve a personal goal for reasons that are beyond your control. These could be bad business environments, poor judging, bad weather, injury, or just plain bad luck. If you base your goals on personal your performance, then you can keep control over the achievement of your goals and get satisfaction from achieving them. ",
                42,
                52,
                group3));
			group3.Items.Add(new SampleDataItem("GearBig-Group-3-Item2",
                "Set Realistic Goals",
                "",
                "Assets/HubPageImages/HubPageImage9.png",
                "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                "\n\n\n\n\n\n\n\n\n\nIt is important to set goals that you can achieve. All sorts of people (parents, media, and society) can set unrealistic goals for you which is almost a guarantee of failure. They will often do this in ignorance of your own desires and ambitions or flat out disinterest.\n\nAlternatively you may be naïve in setting very high goals. You might not appreciate either the obstacles in the way, or understand quite how many skills you must master  to achieve a particular level of performance. By being realistic you are increasing your chances of success.",
                84,
                52,
                group3));
			group3.Items.Add(new SampleDataItem("GearSmall-Group-3-Item3",
                "Level of Goal",
                "",
                "Assets/HubPageImages/HubPageImage10.png",
                "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                "\n\n\n\n\n\n\n\n\n\nJust as it is important not to set goals unrealistically high; do not set them too low. People tend to do this where they are afraid of failure or where they simply don’t want to do anything. You should set goals so that they are slightly out of your immediate grasp, but not so far that there is no hope of achieving them. No one will put serious effort into achieving a goal that they believe is unattainable.\n\nHowever, remember that your belief that a goal is unrealistic may be incorrect. If this could be the case, you can to change this belief by using imagery effectively. ",
                42,
                52,
                group3));
            this.AllGroups.Add(group3);


            var group4 = new SampleDataGroup("Group-4",
                "Planning",
                "Group Subtitle: 2",
                "Assets/DarkGray.png",
                "Group Description: Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus tempor scelerisque lorem in vehicula. Aliquam tincidunt, lacus ut sagittis tristique, turpis massa volutpat augue, eu rutrum ligula ante a ante");

            group4.Items.Add(new SampleDataItem("LatestNews-Group-4-Item1",
                "Key Points",
                "",
                "",
                "Goal setting is an important method of accomplishing any lifetime achievement. However, there are some key points that you should consider before setting your goals.",
                "Goal setting is an important method of accomplishing any lifetime achievement. However, there are some key points that you should consider before setting your goals. Let’s take a look at what those are. •    Deciding what is important for you to achieve in your life and making your choices based on this knowledge \n•    Separating what is important from what is irrelevant so that your focus is in the right place \n•   Motivating yourself to achievement to ensure their accomplishment \n•   Building your self-confidence based on the measured achievement of goals \n•   Ensuring that your goals are your own and no one else’s \n\nYou should allow yourself to enjoy the achievement of goals and reward yourself appropriately. You must draw lessons where they are appropriate, and feed these back into future performances. In learning from mistakes and errors, you are guaranteeing future success. ",
                79,
                21,
                group4));
			
			group4.Items.Add(new SampleDataItem("LatestNews-Group-4-Item2",
                "How Goal Setting Go Wrong",
                "",
                "",
                "Goal setting can go wrong for a number of reasons. When these things happen, it can be a great bit devastating to the self esteem and can make the idea of setting any new goals mute.",
                "Goal setting can go wrong for a number of reasons. When these things happen, it can be a great bit devastating to the self esteem and can make the idea of setting any new goals mute. Before we can look into what we can do about solving these goals setting problems, let’s see what the problems can be. This section is really no more than a more detailed explanation of the above section, but I felt that it needed a section of its own to help you set your sights solely on how setting your goals can go wrong. If it seems repetitious, it is because it is! But it is very necessary for this guide especially for quick referencing later. ",
                79,
                21,
                group4));
			
			group4.Items.Add(new SampleDataItem("LatestNews-Group-4-Item3",
                "Quantum Leap",
                "",
                "",
                "One approach to goal setting for yourself and other people is the Quantum Leap approach. This tries to force intense activity by setting a goal that will need a quantum leap in activity to achieve it.",
                "One approach to goal setting for yourself and other people is the Quantum Leap approach. This tries to force intense activity by setting a goal that will need a quantum leap in activity to achieve it. This is a dangerous technique that should be used with a great deal of care. It is very easy for the whole process of goal setting to fall into problems where quantum leap goals are not met.\n\nSimilarly if you are really not convinced that a goal is attainable, you will not put effort into achieving it. Doesn’t all of this sound repetitious! Business Managers using this approach should take care that they are not shot down by someone that is firmly requesting information on how a quantum leap goal should be achieved because then everybody suffers. ",
                79,
                21,
                group4));
			
			group4.Items.Add(new SampleDataItem("LatestNews-Group-4-Item4",
                "Career Goals",
                "",
                "",
                "One of the toughest issues in making a good career choice and career goal setting  is identifying what it is that you want. Even when it seems that you know what you want, you may still have doubts on if your career choice is the right one for you.",
                "One of the toughest issues in making a good career choice and career goal setting  is identifying what it is that you want. Even when it seems that you know what you want, you may still have doubts on if your career choice is the right one for you. Reaching clarity in those issues may be the most important thing you can do in your career planning and goal setting. Here are a few career goal setting guidelines  that can help. Most people, even very successful ones, have some periods in their career path when they seem unsure about their career choice and goals. It is totally human to feel  that way. ",
                79,
                21,
                group4));
			
			group4.Items.Add(new SampleDataItem("LatestNews-Group-4-Item5",
                "Plan Backwards",
                "",
                "",
                "One of the best ways to move forward is to plan backwards. Start by asking yourself if you can accomplish your goal today. If you can’t why do you think that is? What do you have to do first? Is there something you have to do before that?",
                "One of the best ways to move forward is to plan backwards. Start by asking yourself if you can accomplish your goal today. If you can’t why do you think that is? What do you have to do first? Is there something you have to do before that? Keep thinking backwards like this until you arrive at tasks you could do today. This will help you to attain the goal’s starting point. \n\nFor example, if your goal is to take a two-year business administration program, could you start today? No, you have to be accepted to the program first. Could you be accepted today? No, you have to apply first. Could you apply today? No, you have to decide which post-secondary institutions to apply to. Could you decide today? No, you have to do some research first and so on. I could do this all day but you get the point. ",
                79,
                21,
                group4));
            this.AllGroups.Add(group4);
			
			
			var group5 = new SampleDataGroup("Group-5",
                "Expectations of Goal",
                "Group Subtitle: 2",
                "Assets/DarkGray.png",
                "Group Description: Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus tempor scelerisque lorem in vehicula. Aliquam tincidunt, lacus ut sagittis tristique, turpis massa volutpat augue, eu rutrum ligula ante a ante");

            group5.Items.Add(new SampleDataItem("RecipesBig-Group-5-Item1",
                "Put your plan into action",
                "",
                "Assets/HubPageImages/HubPageImage11.png",
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas sollicitudin, lectus dictum accumsan convallis, tellus sagittis nulla, in tempus magna libero et libero.",
                "\n\n\n\n\n\n\n\n\n\nBy this stage, you  probably have more than one list of things to do and, if it is necessary, some plans for avoiding or dealing with potential problems. Now you need to put them all together into one comprehensive plan. You must list tasks in the order in which you must complete them and set deadlines for the completion of any major plans. \n\nSuccessful career planners keep themselves on track using a variety of methods, such as:\n\n•   marking tasks on a monthly calendar  (noting important dates such as\napplication deadlines or action plans)\n•    making weekly or daily lists of things to do and cross off tasks as they are completed\n •    using a computer program to create timeline charts which give you your time limits for task completion \n•    Using a commercial appointment book or a notebook; even a palm pilot with a new page for each day or week. ",
                54,
                104,
                group5));

            group5.Items.Add(new SampleDataItem("RecipesSmall-Group-5-Item2",
                "Set yourself up for Success",
                "and Trends",
                "Assets/HubPageImages/HubPageImage12.png",
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas sollicitudin, lectus dictum accumsan convallis, tellus sagittis nulla, in tempus magna libero et libero.",
                "\n\n\n\n\n\n\n\n\n\nIt's important not to undermine yourself with goals that are too long-term or impossible to attain. For example; I want to lose all my extra weight before summer is too unrealistic; particularly if you have a great deal of weight to lose and summer is 3 months away. \n\nToo often goals are an end result of  whatever program we choose, and not a part of it. You have to make goals an active part of your life by creating goals that  lead to the next goal works best.",
                49,
                35,
                group5));

            group5.Items.Add(new SampleDataItem("RecipesSmall-Group-5-Item3",
                "Keep a Record",
                "and Trends",
                "Assets/HubPageImages/HubPageImage13.png",
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas sollicitudin, lectus dictum accumsan convallis, tellus sagittis nulla, in tempus magna libero et libero.",
                "\n\n\n\n\n\n\n\n\n\nWrite your goals down so you have something to look forward to as well as back on. To begin, map out no more than eight weeks of activities towards your first fitness goal. Working within your lifestyle, decide on a regular program. It's not necessary to work out every day, especially when beginning a new program as your body is not used to the stress. Our bodies become tired if expected to do hard work seven days a week. \n\nExercising every other day is a safe and realistic goal. Keep track of how much time you want to spend doing an activity, followed by how much time you will actually spend on it. \n\nNot everyone is looking at fitness and health to lose weight. Perhaps you are just looking to better your health. ",
                49,
                34,
                group5));

            group5.Items.Add(new SampleDataItem("RecipesSmall-Group-5-Item4",
                "Setting Relationship Goals",
                "",
                "Assets/HubPageImages/HubPageImage14.png",
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas sollicitudin, lectus dictum accumsan convallis, tellus sagittis nulla, in tempus magna libero et libero.",
                "\n\n\n\n\n\n\n\n\n\nIn order for a relationship to be satisfying those involved in it must set clear goals for it. Most people go into relationships with a vague idea of what they want out of it. When pressed, they often are unable to specify their goals for the relationship in the long term. \n\nGoals can be stated or written, but they should be agreed upon by the partners at the beginning of the relationship. Relationship goals sometimes are dictated by behavior. However, for a relationship to work, the goals stated should be only those on which both partners can agree. ",
                49,
                35,
                group5));

            group5.Items.Add(new SampleDataItem("RecipesSmall-Group-5-Item5",
                "Setting Financial Goals",
                "",
                "Assets/HubPageImages/HubPageImage15.png",
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas sollicitudin, lectus dictum accumsan convallis, tellus sagittis nulla, in tempus magna libero et libero.",
                "\n\n\n\n\n\n\n\n\n\nThe first step in personal financial planning is learning to control your day-to-day financial affairs to enable  you to do the things that bring you satisfaction and enjoyment. This is achieved by planning and following a budget. \n\nThe second step in personal financial planning, and the topic of this section, is choosing and following a course toward achieving your long-term financial goals. \n\nAs with anything else in life, without financial goals and specific plans for meeting them, you will just drift along and leave our future to chance. A wise man once said: Most people don't plan to fail they just fail to plan.",
                 49,
                35,
                group5));

            group5.Items.Add(new SampleDataItem("RecipesMedium-Group-5-Item6",
                "Setting Family Goals",
                "",
                "Assets/HubPageImages/HubPageImage16.png",
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas sollicitudin, lectus dictum accumsan convallis, tellus sagittis nulla, in tempus magna libero et libero.",
                "\n\n\n\n\n\n\n\n\n\nEvery family could use a little bit of help in setting family goals. Perhaps your family wants to take a really fantastic vacation together this year; That three day romantic getaway just for the parents that never seems to happen the home improvement project no one ever seems to have time for. Most families live in a certain budget. Living within budget can help your family pay off those credit cards once and for all and realize your child’s dreams of going to college or your dream of getting a bigger and better house. Perhaps your family’s goal is to see your kids go from a C average to a B or have more quality family time together. Maybe you want to start your own homebased business so that you can spend more time with your family. Yet,  each and every one of these worthy goals can be easily achieved in a somewhat remarkable and FUN way.",
                49,
                69,
                group5));
            this.AllGroups.Add(group5);



            var group6 = new SampleDataGroup("Group-6",
                "Directions",
                "Group Subtitle: 2",
                "Assets/DarkGray.png",
                "Group Description: Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus tempor scelerisque lorem in vehicula. Aliquam tincidunt, lacus ut sagittis tristique, turpis massa volutpat augue, eu rutrum ligula ante a ante");

            group6.Items.Add(new SampleDataItem("Interview-Group-6-Item1",
                "Dream the Goal",
                "",
                "Assets/HubPageImages/HubPageImage17.png",
                "Make a list of everything that you each think you want... all the goals you think you want to achieve. They may involve money, or material things, or better relationships, or a special vacation, or a change in your personal attitudes or habits.",
                "\n\n\n\n\n\n\n\n\n\nMake a list of everything that you each think you want... all the goals you think you want to achieve. They may involve money, or material things, or better relationships, or a special vacation, or a change in your personal attitudes or habits. Get some paper and a pen and go somewhere where you can be completely alone and uninterrupted. Write down everything that comes to mind, being careful not to judge or dismiss any of your ideas.",
                93,
                35,
                group6));

            group6.Items.Add(new SampleDataItem("Interview-Group-6-Item2",
                "Identift the Obstacles",
                "",
                "Assets/HubPageImages/HubPageImage18.png",
                "After you've set your goal, make a list of things that may threaten the successful achievement of the goal and what you can do to remove those threats. For example, are you and your spouse or child fighting over some of these goals?",
                "\n\n\n\n\n\n\n\n\n\nAfter you've set your goal, make a list of things that may threaten the successful achievement of the goal and what you can do to remove those threats. For example, are you and your spouse or child fighting over some of these goals? Write down ALL the  obstacles that you feel may prevent you from reaching your goal. \n\nThis is a particularly magical part of goal setting because it takes all of the obstacles that seemed so huge before and reduces them to little letter that form words on a piece of paper. Once the obstacles are clearly defined, they are more often than not, easily solved. ",
                93,
                35,
                group6));

            group6.Items.Add(new SampleDataItem("Interview-Group-6-Item3",
                "Review",
                "",
                "Assets/HubPageImages/HubPageImage19.png",
                "Once you have your goal and the date in writing, make more reminders of your goal. Put these reminders all around your house, your car, your bathroom.",
                "\n\n\n\n\n\n\n\n\n\nOnce you have your goal and the date in writing, make more reminders of your goal. Put these reminders all around your house, your car, your bathroom. They will remind you of your goal and the date that the goal will be achieved by, and each time you see this information you will be programming your mind to take action toward your goal.",
                93,
                34,
                group6));
            this.AllGroups.Add(group6);


        }
    }
}
