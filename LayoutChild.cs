using UnityEngine;
using UnityEditor;

public class LayoutChild
{
    // FIELDS
    public LayoutSize width;
    public LayoutSize height;
    public LayoutChildAlignment crossAlign;
    public LayoutMargin margin;
    public Rect rect;

    // PROPERTIES
    // Sizes
    public float totalWidth
    {
        get
        {
            return rect.width + margin.left + margin.right;
        }
    }
    public float totalHeight
    {
        get
        {
            return rect.height + margin.top + margin.bottom;
        }
    }
    public Vector2 totalSize
    {
        get
        {
            return new Vector2(totalWidth, totalHeight);
        }
    }
    // Size types
    public bool constantWidth
    {
        get
        {
            return width.constant;
        }
    }
    public bool variableWidth
    {
        get
        {
            return width.variable;
        }
    }
    public bool constantHeight
    {
        get
        {
            return height.constant;
        }
    }
    public bool variableHeight
    {
        get
        {
            return height.variable;
        }
    }

    // Conclassors with no size parameters, assuming the width is the full width and the height is EditorGUIUtility.singleLineHeight
    public LayoutChild() : this(new LayoutMargin()) { }
    public LayoutChild(LayoutMargin m) : this(LayoutChildAlignment.Default(), m) { }
    public LayoutChild(LayoutChildAlignment cA) : this(cA, new LayoutMargin()) { }
    public LayoutChild(LayoutChildAlignment cA, LayoutMargin m) : this(LayoutSize.RatioOfTotal(), cA, m) { }

    // Conclassors where you give the width, assuming the height is EditorGUIUtility.singleLineHeight
    public LayoutChild(LayoutSize w) : this(w, LayoutChildAlignment.Default()) { }
    public LayoutChild(LayoutSize w, LayoutMargin m) : this(w, LayoutChildAlignment.Default(), m) { }
    public LayoutChild(LayoutSize w, LayoutChildAlignment cA) : this(w, cA, new LayoutMargin()) { }
    public LayoutChild(LayoutSize w, LayoutChildAlignment cA, LayoutMargin m) : this(w, LayoutSize.Exact(EditorGUIUtility.singleLineHeight), cA, m) { }

    // Conclassors where you give width and height
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
    // Height
    public float CompileContentHeight(float totalOrRemainder)
    {
        return height.Compile(totalOrRemainder);
    }
    // Alignment
    public LayoutAlignment CompileCrossAlignment(LayoutAlignment parentAlignment)
    {
        return crossAlign.CompileAlignment(parentAlignment);
    }
    // Margin
    public float OrientationStartMargin(LayoutOrientation orientation)
    {
        return margin.OrienationStartMargin(orientation);
    }
    public float OrientationEndMargin(LayoutOrientation orientation)
    {
        return margin.OrienationEndMargin(orientation);
    }
}
