public class LayoutDirection
{
    public LayoutDirectionType type;

    public LayoutDirection(LayoutDirectionType t)
    {
        type = t;
    }

    // Getters
    public LayoutOrientation Orientation()
    {
        if (type == LayoutDirectionType.LeftToRight || type == LayoutDirectionType.RightToLeft)
        {
            return LayoutOrientation.Horizontal;
        }
        else return LayoutOrientation.Vertical;
    }
    public LayoutMovement Movement()
    {
        if (type == LayoutDirectionType.LeftToRight || type == LayoutDirectionType.UpToDown)
        {
            return LayoutMovement.Forwards;
        }
        else return LayoutMovement.Backwards;
    }
}
