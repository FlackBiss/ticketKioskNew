using System.Collections;
using System.Collections.ObjectModel;
using Microsoft.Xaml.Behaviors;
using System.Collections.Specialized;
using System.Windows.Controls;
using System.Windows;
using Lastik.Models.Geometry.Entities.New;
using Lastik.Models.Schedules.Entities;
using Lastik.Utilities;
using Lastik.ViewModels.HallGeometry;
using ListBox = System.Windows.Controls.ListBox;

namespace Lastik.Helpers;

public class ListBoxMultiSelectionBehavior<T> : Behavior<ListBox>
{
    public static readonly DependencyProperty SelectedItemsProperty =
        DependencyProperty.Register(
            nameof(SelectedItems),
            typeof(ObservableCollection<T>),
            typeof(ListBoxMultiSelectionBehavior<T>),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnSelectedItemsChanged
            )
        );

    private static void OnSelectedItemsChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
    {
        var behavior = (ListBoxMultiSelectionBehavior<T>)sender;
        if (behavior._modelHandled) return;

        if (behavior.AssociatedObject is null)
            return;

        if (behavior.SelectedItems is null) return;
        behavior.SelectedItems.CollectionChanged += behavior.SelectedItems_CollectionChanged;

        behavior._modelHandled = true;
        behavior.SelectItems();
        behavior._modelHandled = false;
    }

    private bool _viewHandled;
    private bool _modelHandled;

    public ObservableCollection<T>? SelectedItems
    {
        get => (ObservableCollection<T>)GetValue(SelectedItemsProperty);
        set => SetValue(SelectedItemsProperty, value);
    }

    // Propagate selected items from model to view
    private void SelectItems()
    {
        _viewHandled = true;
        AssociatedObject.SelectedItems.Clear();
        if (SelectedItems is not null)
        {
            foreach (var item in SelectedItems)
                AssociatedObject.SelectedItems.Add(item); 
        }
        
        _viewHandled = false;
    }

    // Propagate selected items from view to model
    private void OnListBoxSelectionChanged(object sender, SelectionChangedEventArgs args)
    {
        if (_viewHandled) return;
        if (AssociatedObject.Items.SourceCollection is null) return;
        if(SelectedItems is null) return;

        args.AddedItems.ForEach<T>(SelectedItems.Add);
        args.RemovedItems.ForEach<T>(i=>SelectedItems.Remove(i));
    }

    // Re-select items when the set of items changes
    private void OnListBoxItemsChanged(object? sender, NotifyCollectionChangedEventArgs args)
    {
        if (_viewHandled) return;
        if (AssociatedObject.Items.SourceCollection is null) return;

        SelectItems();
    }

    protected override void OnAttached()
    {
        base.OnAttached();
        AssociatedObject.SelectionChanged += OnListBoxSelectionChanged;
        ((INotifyCollectionChanged)AssociatedObject.Items).CollectionChanged += OnListBoxItemsChanged;
    }

    private void SelectedItems_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action != NotifyCollectionChangedAction.Remove) return;
        e.OldItems?.ForEach<T>(i=>AssociatedObject.SelectedItems.Remove(i));
    }

    /// <inheritdoc />
    protected override void OnDetaching()
    {
        base.OnDetaching();

        if (AssociatedObject is null) return;
        AssociatedObject.SelectionChanged -= OnListBoxSelectionChanged;
        ((INotifyCollectionChanged)AssociatedObject.Items).CollectionChanged -= OnListBoxItemsChanged;

        if (SelectedItems is null) return;
        SelectedItems.CollectionChanged -= SelectedItems_CollectionChanged;
    }
}

public class TagListBoxMultiSelectionBehavior:ListBoxMultiSelectionBehavior<Hall>;

public class PlaceListBoxMultiSelectionBehavior : ListBoxMultiSelectionBehavior<SchemeDataJson>;