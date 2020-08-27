using UnityEngine;
using System;
using System.Collections.Generic;

public class Layout
{
    // TYPEDEFS

    // Builder class for a layout.
    // Build a set of rectangles for each EdiorGUI control using abstract concepts
    // of direction, alignment, and wrapping rather than computing
    // all dimensions and coordinates manually.  

    // Loosely based off of Flexbox concepts in CSS
    public class Builder
    {
        private LayoutDirection direction;
        private LayoutWrap wrap;
        private LayoutAlignment mainAlign;
        private LayoutAlignment crossAlign;
        private List<LayoutChild> children;

        public Builder() : this(
            new LayoutDirection(LayoutDirectionType.LeftToRight),
            LayoutWrap.NoWrap,
            LayoutAlignment.Start,
            LayoutAlignment.Start)
        { }

        private Builder(Builder other) : this(
            other.direction,
            other.wrap,
            other.mainAlign,
            other.crossAlign)
        { }

        private Builder(LayoutDirection d, LayoutWrap w, LayoutAlignment mA, LayoutAlignment cA)
        {
            direction = d;
            wrap = w;
            mainAlign = mA;
            crossAlign = cA;
            children = new List<LayoutChild>();
        }

        // FIELD SET
        // Methods to set the fields of a new builder
        public Builder Direction(LayoutDirection d)
        {
            direction = d;
            return this;
        }
        public Builder Wrap(LayoutWrap w)
        {
            wrap = w;
            return this;
        }
        public Builder Align(LayoutAlignment align, LayoutAlignmentType type)
        {
            if (type == LayoutAlignmentType.Main) return MainAlign(align);
            else return CrossAlign(align);
        }
        public Builder MainAlign(LayoutAlignment mA)
        {
            mainAlign = mA;
            return this;
        }
        public Builder CrossAlign(LayoutAlignment cA)
        {
            crossAlign = cA;
            return this;
        }

        // ADD CHILDREN
        public Builder PushChild(LayoutChild child)
        {
            children.Add(child);
            return this;
        }
        public Builder PopChild()
        {
            return RemoveChild(children.Count - 1);
        }
        // Insert
        public Builder InsertChild(int index, LayoutChild child)
        {
            children.Insert(index, child);
            return this;
        }
        // Remove
        public Builder RemoveChild(int index)
        {
            children.RemoveAt(index);
            return this;
        }
        public Builder RemoveChildren(int start, int count)
        {
            children.RemoveRange(start, count);
            return this;
        }
        public Builder RemoveChildren(System.Predicate<LayoutChild> predicate)
        {
            children.RemoveAll(predicate);
            return this;
        }
        // Sorting
        public Builder SortChildren(System.Comparison<LayoutChild> comparison)
        {
            children.Sort(comparison);
            return this;
        }
        public Builder SortChildren(IComparer<LayoutChild> comparer)
        {
            children.Sort(comparer);
            return this;
        }
        public Builder SortChildren(int start, int count, IComparer<LayoutChild> comparer)
        {
            children.Sort(start, count, comparer);
            return this;
        }
        // Reordering
        public Builder MoveChildForward(int index, int count)
        {
            LayoutChild reorder = children[index];
            for(int i = index; i < index + count; i++)
            {
                children[index] = children[index + 1];
            }
            children[index + count] = reorder;
            return this;
        }
        public Builder MoveChildBack(int index, int count)
        {
            LayoutChild reorder = children[index];
            for(int i = index; i >= index - count; i--)
            {
                children[index] = children[index - 1];
            }
            children[index - count] = reorder;
            return this;
        }
        public Builder SwapChildren(int index1, int index2)
        {
            LayoutChild swapper = children[index1];
            children[index1] = children[index2];
            children[index2] = swapper;
            return this;
        }

        // COMPILE
        public Layout Compile(Rect container)
        {
            // Initialize child rects
            List<Rect> rects = new List<Rect>();
            for (int i = 0; i < children.Count; i++)
            {
                rects.Add(new Rect());
            }

            CompileChildrenSizes(container.size, rects);

            for(int i = 0; i < rects.Count; i++)
            {
                Debug.Log(rects[i]);
            }

            return new Layout(rects);
        }
        // Compile Sizes
        private void CompileChildrenSizes(Vector2 containerSize, List<Rect> rects)
        {
            CompileConstantSizes(containerSize, rects);
            CompileVariableSizes(containerSize - AggregateSizes(rects), rects);
        }
        private void CompileConstantSizes(Vector2 total, List<Rect> rects)
        {
            CompileChildrenSizeUtil(total, true, rects);
        }
        private void CompileVariableSizes(Vector2 remainder, List<Rect> rects)
        {
            CompileChildrenSizeUtil(remainder, false, rects);
        }
        private void CompileChildrenSizeUtil(Vector2 totalOrRemainder, bool constant, List<Rect> rects)
        { 
            // Used to build the rects in the list
            Vector2 temp = new Vector2();

            for (int i = 0; i < rects.Count; i++)
            {
                temp = rects[i].size;

                // If the main size is a fixed type, get the exact size
                if (children[i].WidthIsConstant() == constant)
                {
                    temp.x = children[i].CompileWidth(totalOrRemainder.x);
                }
                // If the cross size is a fixed type, get the exact size
                if (children[i].HeightIsConstant() == constant)
                {
                    temp.y = children[i].CompileHeight(totalOrRemainder.y);
                }

                rects[i] = new Rect(rects[i].position, temp);
            }
        }
        // Compile positions
        private void CompilePositions(Rect container, List<Rect> rects)
        {
            Vector2 currentPosition = Vector2.zero;
            LayoutAlignment trueCrossAlign;

            for(int i = 0; i < rects.Count; i++)
            {
                trueCrossAlign = children[i].CompileCrossAlignment(crossAlign);

                if(direction.Category() == LayoutDirectionCategory.Horizontal)
                {

                }
                else
                {

                }
            }
        }

        // UTIL
        private Vector2 AggregateSizes(List<Rect> rects)
        {
            Vector2 size = Vector2.zero;
            foreach (Rect r in rects)
            {
                size += r.size;
            }
            return size;
        }
        private float LeftoverSpace(Rect container, List<Rect> rects)
        {
            if (direction.Category() == LayoutDirectionCategory.Horizontal)
            {
                return container.width - AggregateSizes(rects).x;
            }
            else return container.height - AggregateSizes(rects).y;
        }
        private float CenterSpace(Rect container, List<Rect> rects)
        {
            return LeftoverSpace(container, rects) / 2f;
        }
        private float JustifySpace(Rect container, List<Rect> rects)
        {
            return LeftoverSpace(container, rects) / (rects.Count + 1);
        }
    }

    private List<Rect> items;
    private int cursor;

    private Layout(List<Rect> rects)
    {
        items = rects;
        cursor = 0;
    }

    // Member functions
    public void Start()
    {
        cursor = 0;
    }
    public Rect Next()
    {
        if(!AtEnd()) return items[cursor++];
        else return items[items.Count - 1];
    }
    public bool AtEnd()
    {
        return cursor >= items.Count;
    }
}
