using UnityEngine;
using UnityEditor;

public class LayoutChild
{
    public LayoutSize width;
    public LayoutSize height;
    public LayoutChildAlignment crossAlign;
    public LayoutMargin margin;
    public Rect rect;

    public LayoutChild() : this(new LayoutMargin()) { }
    public LayoutChild(LayoutMargin m) : this(LayoutChildAlignment.Default(), m) { }
    public LayoutChild(LayoutChildAlignment cA) : this(cA, new LayoutMargin()) { }
    public LayoutChild(LayoutChildAlignment cA, LayoutMargin m) : this(LayoutSize.RatioOfTotal(), cA, m) { }

    // Constructors where you give the width, assuming the height is EditorGUIUtility.singleLineHeight
    public LayoutChild(LayoutSize w) : this(w, LayoutChildAlignment.Default()) { }
    public LayoutChild(LayoutSize w, LayoutMargin m) : this(w, LayoutChildAlignment.Default(), m) { }
    public LayoutChild(LayoutSize w, LayoutChildAlignment cA) : this(w, cA, new LayoutMargin()) { }
    public LayoutChild(LayoutSize w, LayoutChildAlignment cA, LayoutMargin m) : this(w, LayoutSize.Exact(EditorGUIUtility.singleLineHeight), cA, m) { }

    // Constructors where you give width and height
    public LayoutChild(LayoutSize w, LayoutSize h) : this(w, h, LayoutChildAlignment.Default()) { }
    public LayoutChild(LayoutSize w, LayoutSize h, LayoutChildAlignment cA) : this(w, h, cA, new LayoutMargin()) { }
    public LayoutChild(LayoutSize w, LayoutSize h, LayoutMargin m) : this(w, h, LayoutChildAlignment.Default(), m) { }
    public LayoutChild(LayoutSize w, LayoutSize h, LayoutChildAlignment cA, LayoutMargin m)
    {
        width = w;
        height = h;
        crossAlign = cA;
        margin = m;
        rect = new Rect();
    }

    // Delegates
    // Width
    public float CompileContentWidth(float totalOrRemainder)
    {
        return width.Compile(totalOrRemainder);
    }
    public bool WidthIsConstant()
    {
        return width.IsConstant();
    }
    public bool WidthIsVariable()
    {
        return width.IsVariable();
    }
    // Height
    public float CompileContentHeight(float totalOrRemainder)
    {
        return height.Compile(totalOrRemainder);
    }
    public bool HeightIsConstant()
    {
        return height.IsConstant();
    }
    public bool HeightIsVariable()
    {
        return height.IsVariable();
    }
    // Alignment
    public LayoutAlignment CompileCrossAlignment(LayoutAlignment parentAlignment)
    {
        return crossAlign.CompileAlignment(parentAlignment);
    }
    // Margin
    public float CompileMainAxisStartMargin(LayoutOrientation orientation)
    {
        if (orientation == LayoutOrientation.Horizontal)
        {
            return margin.left;
        }
        else return margin.top;
    }
    public float CompileMainAxisEndMargin(LayoutOrientation orientation)
    {
        if (orientation == LayoutOrientation.Horizontal)
        {
            return margin.right;
        }
        else return margin.bottom;
    }
    public float CompileCrossAxisStartMargin(LayoutOrientation orientation)
    {
        if (orientation == LayoutOrientation.Horizontal)
        {
            return margin.top;
        }
        else return margin.left;
    }
}
