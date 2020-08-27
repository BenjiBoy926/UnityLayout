using UnityEngine;

public class LayoutChild
{
    public LayoutSize width;
    public LayoutSize height;
    public LayoutChildAlignment crossAlign;
    public Rect rect;

    public LayoutChild(LayoutSize w, LayoutSize h) : this(w, h, LayoutChildAlignment.Default()) { }
    public LayoutChild(LayoutSize w, LayoutSize h, LayoutChildAlignment cA)
    {
        width = w;
        height = h;
        crossAlign = cA;
        rect = new Rect();
    }

    // Delegates
    // Width
    public float CompileWidth(float totalOrRemainder)
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
    public float CompileHeight(float totalOrRemainder)
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
}
