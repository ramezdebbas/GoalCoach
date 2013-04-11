using HealthAndFitness.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Grouped Items Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234231

namespace HealthAndFitness
{
    /// <summary>
    /// A page that displays a grouped collection of items.
    /// </summary>
    public sealed partial class WorkOutPage : HealthAndFitness.Common.LayoutAwarePage
    {
        public WorkOutPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            // TODO: Create an appropriate data model for your problem domain to replace the sample data
            var WorkOutDataGroups = WorkOutDataSource.GetGroups("AllGroups");
            this.DefaultViewModel["Groups"] = WorkOutDataGroups;
            EnableLiveTile.CreateLiveTile.ShowliveTile(false, "Health & Fitness");
         //   if (navigationParameter is String)
                //pageTitle.Text = navigationParameter.ToString();
           // else if (navigationParameter is SampleDataGroup)
           //     pageTitle.Text = (navigationParameter as SampleDataGroup).Title;
        }

        /// <summary>
        /// Invoked when a group header is clicked.
        /// </summary>
        /// <param name="sender">The Button used as a group header for the selected group.</param>
        /// <param name="e">Event data that describes how the click was initiated.</param>
        void Header_Click(object sender, RoutedEventArgs e)
        {
            // Determine what group the Button instance represents
            // var group = (sender as FrameworkElement).DataContext;

            // Navigate to the appropriate destination page, configuring the new page
            // by passing required information as a navigation parameter
            //  this.Frame.Navigate(typeof(PartyItemsPage), group);
        }

        /// <summary>
        /// Invoked when an item within a group is clicked.
        /// </summary>
        /// <param name="sender">The GridView (or ListView when the application is snapped)
        /// displaying the item clicked.</param>
        /// <param name="e">Event data that describes the item clicked.</param>
        void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Navigate to the appropriate destination page, configuring the new page
            // by passing required information as a navigation parameter
            var itemId = ((WorkOutDataItem)e.ClickedItem);
            this.Frame.Navigate(typeof(ItemDetailPage), itemId);
        }




         private void btnService_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
        	 this.Frame.Navigate(typeof(RecipesPage), (sender as Button).Tag);
		
        }

        private void btnGear_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
        	this.Frame.Navigate(typeof(WorkOutPage), "AllGroups");
        }

        private void btnNews_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
        	 this.Frame.Navigate(typeof(RecipesPage), (sender as Button).Tag);
        }

        private void btnRecipes_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
        	 this.Frame.Navigate(typeof(RecipesPage), (sender as Button).Tag);
        }

        private void btnInterviews_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
        	 this.Frame.Navigate(typeof(RecipesPage), (sender as Button).Tag);
        }

        private void btnHome_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
        	this.Frame.Navigate(typeof(GroupedItemsPage), "AllGroups");
        }
    }
}

