public class LayoutDirection
{
    public LayoutDirectionType type;

    public LayoutDirection(LayoutDirectionType t)
    {
        type = t;
    }

    public LayoutDirectionCategory Category()
    {
        if (type == LayoutDirectionType.LeftToRight || type == LayoutDirectionType.RightToLeft)
        {
            return LayoutDirectionCategory.Horizontal;
        }
        else return LayoutDirectionCategory.Vertical;
    }
}
