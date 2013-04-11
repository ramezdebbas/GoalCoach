using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthAndFitness.Data;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace HealthAndFitness.VariableTemplate
{
    public class VariableTiles : DataTemplateSelector
    {

        public DataTemplate BigLTemplate { get; set; }
        public DataTemplate ServiceLTemplate { get; set; }
        public DataTemplate ServicePTemplate { get; set; }
        public DataTemplate GearBigTemplate { get; set; }
        public DataTemplate GearSmallTemplate { get; set; }
        public DataTemplate LatestNewsTemplate { get; set; }
        public DataTemplate RecipesBigTemplate { get; set; }
        public DataTemplate RecipesMediumTemplate { get; set; }
        public DataTemplate RecipesSmallTemplate { get; set; }
        public DataTemplate InterviewTemplate { get; set; }
        public DataTemplate BigPTemplate { get; set; }

        

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            FrameworkElement element = container as FrameworkElement;

            if (element != null && item != null)
            {
                if (item.GetType() == typeof(SampleDataItem))
                {

                    if ((item as SampleDataItem).UniqueId.StartsWith("BigL"))
                        return BigLTemplate;
                    if ((item as SampleDataItem).UniqueId.StartsWith("ServiceL"))
                        return ServiceLTemplate;
                    if ((item as SampleDataItem).UniqueId.StartsWith("ServiceP"))
                        return ServicePTemplate;
                    if ((item as SampleDataItem).UniqueId.StartsWith("GearBig"))
                        return GearBigTemplate;
                    if ((item as SampleDataItem).UniqueId.StartsWith("GearSmall"))
                        return GearSmallTemplate;
                    if ((item as SampleDataItem).UniqueId.StartsWith("LatestNews"))
                        return LatestNewsTemplate;
                    if ((item as SampleDataItem).UniqueId.StartsWith("RecipesBig"))
                        return RecipesBigTemplate;
                    if ((item as SampleDataItem).UniqueId.StartsWith("RecipesMedium"))
                        return RecipesMediumTemplate;
                    if ((item as SampleDataItem).UniqueId.StartsWith("RecipesSmall"))
                        return RecipesSmallTemplate;
                    if ((item as SampleDataItem).UniqueId.StartsWith("Interview"))
                        return InterviewTemplate;


                }

                else if (item.GetType() == typeof(RecipesDataItem))
                {
                    if ((item as RecipesDataItem).UniqueId.StartsWith("RecipesBig"))
                        return RecipesBigTemplate;
                    if ((item as RecipesDataItem).UniqueId.StartsWith("RecipesMedium"))
                        return RecipesMediumTemplate;
                    if ((item as RecipesDataItem).UniqueId.StartsWith("RecipesSmall"))
                        return RecipesSmallTemplate;
                    if ((item as RecipesDataItem).UniqueId.StartsWith("BigP"))
                        return BigPTemplate;
                }


                else if (item.GetType() == typeof(WorkOutDataItem))
                {
                    if ((item as WorkOutDataItem).UniqueId.StartsWith("RecipesBig"))
                        return RecipesBigTemplate;
                    if ((item as WorkOutDataItem).UniqueId.StartsWith("RecipesMedium"))
                        return RecipesMediumTemplate;
                    if ((item as WorkOutDataItem).UniqueId.StartsWith("RecipesSmall"))
                        return RecipesSmallTemplate;
                    if ((item as WorkOutDataItem).UniqueId.StartsWith("BigP"))
                        return BigPTemplate;
                }

            }
            return base.SelectTemplateCore(item, container);
        }

    }
}
