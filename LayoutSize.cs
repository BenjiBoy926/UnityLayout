public class LayoutSize
{
    public LayoutSizeType type;
    public float size;
    public float min;
    public float max;

    private LayoutSize(LayoutSizeType t, float s) : this(t, s, 0, float.PositiveInfinity) { }
    private LayoutSize(LayoutSizeType t, float s, float mn, float mx)
    {
        type = t;
        size = s;
        min = mn;
        max = mx;
    }

    // Factory methods
    // Exact
    public static LayoutSize Exact(float size)
    {
        return new LayoutSize(LayoutSizeType.Exact, size);
    }
    // Ratio of total
    public static LayoutSize RatioOfTotal(float ratio)
    {
        return RatioOfTotal(ratio, 0);
    }
    public static LayoutSize RatioOfTotal(float ratio, float min)
    {
        return RatioOfTotal(ratio, min, float.PositiveInfinity);
    }
    public static LayoutSize RatioOfTotal(float ratio, float min, float max)
    {
        return new LayoutSize(LayoutSizeType.RatioOfTotal, ratio, min, max);
    }
    // Ratio of remainder
    public static LayoutSize RatioOfRemainder(float ratio)
    {
        return RatioOfRemainder(ratio, 0);
    }
    public static LayoutSize RatioOfRemainder(float ratio, float min)
    {
        return RatioOfRemainder(ratio, min, float.PositiveInfinity);
    }
    public static LayoutSize RatioOfRemainder(float ratio, float min, float max)
    {
        return new LayoutSize(LayoutSizeType.RatioOfRemainder, ratio, min, max);
    }
}
