using UnityEngine;
using System.Collections.Generic;

public class Layout
{
    public class Builder
    {
        private LayoutDirection direction;
        private LayoutWrap wrap;
        private LayoutAlignment mainAlign;
        private LayoutAlignment crossAlign;
        private List<LayoutChild> children;

        public Builder() : this(
            LayoutDirection.LeftToRight,
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
        public Layout Compile()
        {
            List<Rect> rects = new List<Rect>();



            return new Layout(rects);
        }
    }

    private List<Rect> items;
    private int cursor;

    private Layout(List<Rect> rects)
    {
        items = rects;
        cursor = 0;
    }

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
