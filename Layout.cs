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
        private LayoutOrientation orientation;
        private LayoutWrap wrap;
        private LayoutAlignment mainAlign;
        private LayoutAlignment crossAlign;
        private List<LayoutChild> children;

        public Builder() : this(
            LayoutOrientation.Horizontal,
            LayoutWrap.NoWrap,
            LayoutAlignment.Start,
            LayoutAlignment.Start)
        { }

        private Builder(Builder other) : this(
            other.orientation,
            other.wrap,
            other.mainAlign,
            other.crossAlign)
        { }

        private Builder(LayoutOrientation o, LayoutWrap w, LayoutAlignment mA, LayoutAlignment cA)
        {
            orientation = o;
            wrap = w;
            mainAlign = mA;
            crossAlign = cA;
            children = new List<LayoutChild>();
        }

        // FIELD SET
        // Methods to set the fields of a new builder
        public Builder Orientation(LayoutOrientation o)
        {
            orientation = o;
            return this;
        }
        public Builder Wrap(LayoutWrap w)
        {
            wrap = w;
            return this;
        }
        public Builder Align(LayoutAlignment align, LayoutAxis type)
        {
            if (type == LayoutAxis.Main) return MainAlign(align);
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
            CompileChildrenSizes(container.size);
            CompileChildrenPositions(container);

            for(int i = 0; i < children.Count; i++)
            {
                Debug.Log(children[i].rect);
            }

            return new Layout(GetChildrenRects());
        }
        // Compile Sizes
        private void CompileChildrenSizes(Vector2 containerSize)
        {
            CompileConstantSizes(containerSize);
            CompileVariableSizes(containerSize - TotalChildSize());
        }
        private void CompileConstantSizes(Vector2 total)
        {
            CompileChildrenSizeUtil(total, true);
        }
        private void CompileVariableSizes(Vector2 remainder)
        {
            CompileChildrenSizeUtil(remainder, false);
        }
        private void CompileChildrenSizeUtil(Vector2 totalOrRemainder, bool constant)
        { 
            // Used to build the rects in the list
            Vector2 temp = new Vector2();

            for (int i = 0; i < children.Count; i++)
            {
                temp = children[i].rect.size;

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

                children[i].rect.size = temp;
            }
        }
        // Compile positions
        private void CompileChildrenPositions(Rect container)
        {
            for(int i = 0; i < children.Count; i++)
            {
                children[i].rect.position = GetLayoutChildCoordinate(container, i);
            }
        }

        // UTIL
        // Layout children list
        private Vector2 TotalChildSize()
        {
            Vector2 size = Vector2.zero;
            foreach (LayoutChild child in children)
            {
                size += child.rect.size;
            }
            return size;
        }
        private List<Rect> GetChildrenRects()
        {
            List<Rect> rects = new List<Rect>();
            foreach (LayoutChild child in children)
            {
                rects.Add(child.rect);
            }
            return rects;
        }
        // Leftover space in the container
        private float LeftoverSpace(Rect container)
        {
            if (orientation == LayoutOrientation.Horizontal)
            {
                return container.width - TotalChildSize().x;
            }
            else return container.height - TotalChildSize().y;
        }
        private float CenterSpace(Rect container)
        {
            return LeftoverSpace(container) / 2f;
        }
        private float JustifySpace(Rect container)
        {
            return LeftoverSpace(container) / (children.Count + 1);
        }
        // Separate main axis components of the rect based on the axis of the layout direction
        private float MainAxisCoordinate(Rect rect)
        {
            return AxisCoordinate(rect, LayoutAxis.Main);
        }
        private float MainAxisSize(Rect rect)
        {
            return AxisSize(rect, LayoutAxis.Main);
        }
        // Separeate cross axis component of the rect based on axis of the layout direction
        private float CrossAxisCoordinate(Rect rect)
        {
            return AxisCoordinate(rect, LayoutAxis.Cross);
        }
        private float CrossAxisSize(Rect rect)
        {
            return AxisSize(rect, LayoutAxis.Cross);
        }
        // Get the coordinate of the rect along the given layout axis
        private float AxisCoordinate(Rect rect, LayoutAxis axis)
        {
            if(axis == LayoutAxis.Main)
            {
                if (orientation == LayoutOrientation.Horizontal)
                {
                    return rect.x;
                }
                else return rect.y;
            }
            else
            {
                if (orientation == LayoutOrientation.Horizontal)
                {
                    return rect.y;
                }
                else return rect.x;
            }
        }
        private float AxisSize(Rect rect, LayoutAxis axis)
        {
            if (axis == LayoutAxis.Main)
            {
                if (orientation == LayoutOrientation.Horizontal)
                {
                    return rect.width;
                }
                else return rect.height;
            }
            else
            {
                if (orientation == LayoutOrientation.Horizontal)
                {
                    return rect.height;
                }
                else return rect.width;
            }
        }

        // Get coordinates
        private Vector2 GetStartCoordinate(Rect container)
        {
            float mainAxisCoordinate;

            // Compute the main axis coordinate based on the main axis alignment of the layout children
            if (mainAlign == LayoutAlignment.Start)
            {
                mainAxisCoordinate = MainAxisCoordinate(container);
            }
            else if (mainAlign == LayoutAlignment.Center)
            {
                mainAxisCoordinate = MainAxisCoordinate(container) + CenterSpace(container);
            }
            else if (mainAlign == LayoutAlignment.End)
            {
                mainAxisCoordinate = MainAxisCoordinate(container) + LeftoverSpace(container);
            }
            else
            {
                mainAxisCoordinate = MainAxisCoordinate(container) + JustifySpace(container);
            }

            // Determine if the main axis coordinate is the x-value or y-value
            if (orientation == LayoutOrientation.Horizontal)
            {
                return new Vector2(mainAxisCoordinate, container.y);
            }
            else return new Vector2(container.x, mainAxisCoordinate);
        }
        private Vector2 GetLayoutChildCoordinate(Rect container, int index)
        {
            Vector2 position = GetStartCoordinate(container);

            float mainAxisShift = 0;
            float crossAxisShift = 0;

            // Compute the cross axis shift
            LayoutAlignment trueCrossAlign = children[index].CompileCrossAlignment(crossAlign);
            if(trueCrossAlign == LayoutAlignment.Center || trueCrossAlign == LayoutAlignment.Justify)
            {
                crossAxisShift = (CrossAxisSize(container) - CrossAxisSize(children[index].rect)) / 2f;
            }
            else if(trueCrossAlign == LayoutAlignment.End)
            {
                crossAxisShift = CrossAxisSize(container) - CrossAxisSize(children[index].rect);
            }

            // Set the shift of the child along the main axis
            // By adding the sizes along the main axis 
            // of all the children before this child
            for(int i = 0; i < index; i++)
            {
                mainAxisShift += MainAxisSize(children[i].rect);

                if (mainAlign == LayoutAlignment.Justify)
                {
                    mainAxisShift += JustifySpace(container);
                }
            }

            if (orientation == LayoutOrientation.Horizontal)
            {
                return new Vector2(position.x + mainAxisShift, position.y + crossAxisShift);
            }
            else return new Vector2(position.x + crossAxisShift, position.y + mainAxisShift);
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
