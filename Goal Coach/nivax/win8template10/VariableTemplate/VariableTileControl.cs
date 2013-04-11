using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthAndFitness.Data;
using Windows.UI.Xaml.Controls;

namespace HealthAndFitness.VariableTemplate
{
    public class VariableTileControl : GridView
    {
        protected override void PrepareContainerForItemOverride(Windows.UI.Xaml.DependencyObject element, object item)
        {
            if (item.GetType() == typeof(SampleDataItem))
            {
                var viewModel = item as SampleDataItem;
                element.SetValue(VariableSizedWrapGrid.ColumnSpanProperty, viewModel.ColSpan);
                element.SetValue(VariableSizedWrapGrid.RowSpanProperty, viewModel.RowSpan);
                base.PrepareContainerForItemOverride(element, item);
            }
            else if (item.GetType() == typeof(RecipesDataItem))
            {
                var viewModel = item as RecipesDataItem;
                element.SetValue(VariableSizedWrapGrid.ColumnSpanProperty, viewModel.ColSpan);
                element.SetValue(VariableSizedWrapGrid.RowSpanProperty, viewModel.RowSpan);
                base.PrepareContainerForItemOverride(element, item);
            }

            else if (item.GetType() == typeof(WorkOutDataItem))
            {
                var viewModel = item as WorkOutDataItem;
                element.SetValue(VariableSizedWrapGrid.ColumnSpanProperty, viewModel.ColSpan);
                element.SetValue(VariableSizedWrapGrid.RowSpanProperty, viewModel.RowSpan);
                base.PrepareContainerForItemOverride(element, item);
            }
        }
    }
}