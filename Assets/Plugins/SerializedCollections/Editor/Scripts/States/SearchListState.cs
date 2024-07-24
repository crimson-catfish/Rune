using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections.Editor.Search;
using UnityEditor;
using UnityEngine;

namespace AYellowpaper.SerializedCollections.Editor.States
{
    internal class SearchListState : ListState
    {
        private readonly List<SearchResultEntry> _searchResults = new();
        private          HashSet<string>         _foundProperties;

        private string _lastSearch = string.Empty;
        private Color  _previousColor;

        public SearchListState(SerializedDictionaryInstanceDrawer serializedDictionaryDrawer) : base(
            serializedDictionaryDrawer)
        {
        }

        public override int    ListSize               => _searchResults.Count;
        public override string NoElementsText         => "No Results";
        public          bool   OnlyShowMatchingValues { get; set; }

        public override void DrawElement(Rect rect, SerializedProperty property, DisplayType displayType)
        {
            SerializedDictionaryInstanceDrawer.DrawElement(rect, property, displayType, BeforeDrawingProperty,
                AfterDrawingProperty);
        }

        public override void OnEnter()
        {
            this.Drawer.ReorderableList.draggable = false;
            UpdateSearch();
        }

        public override void OnExit()
        {
        }

        public override ListState OnUpdate()
        {
            if (this.Drawer.SearchText.Length == 0)
                return this.Drawer.DefaultState;

            UpdateSearch();

            return this;
        }

        public void PerformSearch(string searchString)
        {
            var query = new SearchQuery(Matchers.RegisteredMatchers);
            query.SearchString = searchString;
            _searchResults.Clear();
            _searchResults.AddRange(query.ApplyToArrayProperty(this.Drawer.ListProperty));

            _foundProperties = _searchResults.SelectMany(x => x.MatchingResults, (x, y) => y.Property.propertyPath)
                .ToHashSet();
        }

        public override SerializedProperty GetPropertyAtIndex(int index)
        {
            return _searchResults[index].Property;
        }

        public override float GetHeightAtIndex(int index, bool drawKeyAsList, bool drawValueAsList)
        {
            return base.GetHeightAtIndex(index, drawKeyAsList, drawValueAsList);
        }

        public override void RemoveElementAt(int index)
        {
            var indexToDelete = _searchResults[index].Index;
            this.Drawer.ListProperty.DeleteArrayElementAtIndex(indexToDelete);
            PerformSearch(_lastSearch);
        }

        public override void InserElementAt(int index)
        {
            var indexToAdd = _searchResults[index].Index;
            this.Drawer.ListProperty.InsertArrayElementAtIndex(indexToAdd);
            PerformSearch(_lastSearch);
        }

        private void BeforeDrawingProperty(SerializedProperty obj)
        {
            _previousColor = GUI.backgroundColor;

            if (_foundProperties.Contains(obj.propertyPath))
            {
                GUI.backgroundColor = Color.blue;
            }
        }

        private void AfterDrawingProperty(SerializedProperty obj)
        {
            GUI.backgroundColor = _previousColor;
        }

        private void UpdateSearch()
        {
            if (_lastSearch != this.Drawer.SearchText)
            {
                _lastSearch = this.Drawer.SearchText;
                PerformSearch(this.Drawer.SearchText);
            }
        }
    }
}