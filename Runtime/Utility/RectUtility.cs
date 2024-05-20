﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Tactile.Utility
{
    /// <summary>
    /// Represents a manner in which a rect can be laid out.
    /// </summary>
    public abstract record RectLayout
    {
        public static float DefaultGap = 4f;
        
        private interface IFlexibleRect
        {
            float FlexSize { get; }
        }

        private interface IFixedSizeRect
        {
            float FixedSize { get; }
        }

        private interface IContentRect
        {
        }

        public record FlexLayout(float FlexSize = 1f) : RectLayout, IFlexibleRect, IContentRect;

        public record SizeLayout(float FixedSize) : RectLayout, IFixedSizeRect, IContentRect;

        public record FlexibleSpaceLayout(float FlexSize = 1f) : RectLayout, IFlexibleRect;

        public record SpaceLayout(float FixedSize) : RectLayout, IFixedSizeRect;

        public record RepeatLayout(int RepeatTimes, params RectLayout[] RepeatedLayout) : RectLayout;

        public static FlexLayout Flex(float flex = 1f) => new(flex);
        public static SizeLayout Size(float size) => new(size);
        public static SpaceLayout Space(float size) => new(size);
        public static FlexibleSpaceLayout FlexibleSpace(float flex = 1f) => new(flex);
        public static RepeatLayout Repeat(int repeatTimes, params RectLayout[] layouts) => new(repeatTimes, layouts);

        #if UNITY_EDITOR
        public static readonly SizeLayout SingleLineHeight = Size(EditorGUIUtility.singleLineHeight);
        #endif
        
        public enum LayoutDirection
        {
            Vertical,
            Horizontal
        }

        public static Rect[] Layout(Rect rect, LayoutDirection direction, float gap, params RectLayout[] layouts)
        {
            var flattenedLayouts = FlattenLayouts(layouts).ToArray();
            var (numRects, totalFlex, totalFixed) = SummarizeLayouts(flattenedLayouts);
            
            var rects = new Rect[numRects];
            var startPos = direction == LayoutDirection.Horizontal ? rect.x : rect.y;
            var rectSize = direction == LayoutDirection.Horizontal ? rect.width : rect.height;
            var contentSize = rectSize - gap * (flattenedLayouts.Length - 1) - totalFixed;
            var i = 0;
            
            foreach (var layout in flattenedLayouts)
            {
                var size = layout switch
                {
                    IFlexibleRect flexibleRect => flexibleRect.FlexSize / totalFlex * contentSize,
                    IFixedSizeRect fixedSizeRect => fixedSizeRect.FixedSize,
                    _ => 0f
                };
                
                // Only create a rect for content (e.g. don't create a rect for spaces).
                if (layout is IContentRect)
                {
                    rects[i++] = direction switch
                    {
                        LayoutDirection.Horizontal => new Rect(startPos, rect.y, size, rect.height),
                        LayoutDirection.Vertical => new Rect(rect.x, startPos, rect.width, size),
                        _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
                    };
                };

                startPos += size + gap;
            }

            return rects;
        }

        private static IEnumerable<RectLayout> FlattenLayouts(IEnumerable<RectLayout> layouts) => layouts.SelectMany(
            l => l switch
            {
                RepeatLayout repeatLayout => Enumerable
                    .Repeat(FlattenLayouts(repeatLayout.RepeatedLayout), repeatLayout.RepeatTimes).SelectMany(i => i),
                _ => Yield(l),
            });

        private static (int numRects, float totalFlex, float totalFixed) SummarizeLayouts(IEnumerable<RectLayout> layouts) =>
            layouts.Aggregate((0, 0f, 0f),
                (a, l) => (a.Item1 + (l is IContentRect ? 1 : 0),
                    a.Item2 + (l is IFlexibleRect flexibleRect ? flexibleRect.FlexSize : 0),
                    a.Item3 + (l is IFixedSizeRect fixedSizeRect ? fixedSizeRect.FixedSize : 0)));

        private static IEnumerable<RectLayout> Yield(RectLayout layout)
        {
            yield return layout;
        }
    }

    public static class RectUtility
    {
        /// <summary>
        /// Adds a specified amount of padding to the rectangle
        /// </summary>
        /// <param name="rect">The rectangle to modify</param>
        /// <param name="top">How much padding to apply on top</param>
        /// <param name="bottom">How much padding to apply on the bottom</param>
        /// <param name="left">How much padding to apply on the left</param>
        /// <param name="right">How much padding to apply on the right</param>
        /// <returns>A new rectangle with padding</returns>
        public static Rect Padding(this Rect rect, float top, float bottom, float left, float right)
        {
            var width = rect.width - (left + right);
            var height = rect.height - (top + bottom);
            var x = rect.x + left;
            var y = rect.y + top;
            var newRect = new Rect(x, y, width, height);

            return newRect;
        }

        public static Rect[] Layout(this Rect rect, RectLayout.LayoutDirection direction, float gap,
            params RectLayout[] layouts) =>
            RectLayout.Layout(rect, direction, gap, layouts);
        
        public static Rect[] Layout(this Rect rect, RectLayout.LayoutDirection direction,
            params RectLayout[] layouts) =>
            RectLayout.Layout(rect, direction, RectLayout.DefaultGap, layouts);

        public static Rect[] HorizontalLayout(this Rect rect, float gap,
            params RectLayout[] layouts) =>
            RectLayout.Layout(rect, RectLayout.LayoutDirection.Horizontal, gap, layouts);
        
        public static Rect[] HorizontalLayout(this Rect rect,
            params RectLayout[] layouts) =>
            RectLayout.Layout(rect, RectLayout.LayoutDirection.Horizontal, RectLayout.DefaultGap, layouts);

        public static Rect[] VerticalLayout(this Rect rect, float gap,
            params RectLayout[] layouts) =>
            RectLayout.Layout(rect, RectLayout.LayoutDirection.Vertical, gap, layouts);
        
        public static Rect[] VerticalLayout(this Rect rect,
            params RectLayout[] layouts) =>
            RectLayout.Layout(rect, RectLayout.LayoutDirection.Vertical, RectLayout.DefaultGap, layouts);

        /// <summary>
        /// Applies padding uniformly throughout the rectangle.
        /// </summary>
        /// <param name="rect">The rectangle to add padding to</param>
        /// <param name="padding">The amount of padding to add throughout the rectangle.</param>
        /// <returns>A new rectangle with padding</returns>
        public static Rect Padding(this Rect rect, float padding) => Padding(rect, padding, padding, padding, padding);
        public static Rect PaddingTop(this Rect rect, float padding) => Padding(rect, padding, 0, 0, 0);
        public static Rect PaddingBottom(this Rect rect, float padding) => Padding(rect, 0, padding, 0, 0);
        public static Rect PaddingLeft(this Rect rect, float padding) => Padding(rect, 0, 0, padding, 0);
        public static Rect PaddingRight(this Rect rect, float padding) => Padding(rect, 0, 0, 0, padding);
        public static Rect PaddingVertical(this Rect rect, float padding) => Padding(rect, padding, padding, 0, 0);
        public static Rect PaddingHorizontal(this Rect rect, float padding) => Padding(rect, 0, 0, padding, padding);

        public static float GetArea(this Rect rect) => rect.width * rect.height;
        public static float GetPerimeter(this Rect rect) => 2 * (rect.width + rect.height);
    }
}