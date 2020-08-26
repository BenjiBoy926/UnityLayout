public class LayoutChild
{
    public LayoutSize width;
    public LayoutSize height;
    public LayoutChildAlignment crossAlign;

    public LayoutChild(LayoutSize w, LayoutSize h) : this(w, h, LayoutChildAlignment.Default()) { }
    public LayoutChild(LayoutSize w, LayoutSize h, LayoutChildAlignment cA)
    {
        width = w;
        height = h;
        crossAlign = cA;
    }
}
