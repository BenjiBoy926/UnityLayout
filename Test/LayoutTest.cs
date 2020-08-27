using UnityEngine;

public class LayoutTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Layout.Builder builder = new Layout.Builder();
        builder.PushChild(new LayoutChild(LayoutSize.Exact(50), LayoutSize.RatioOfTotal(1)));
        builder.PushChild(new LayoutChild(LayoutSize.RatioOfTotal(0.25f), LayoutSize.RatioOfTotal(1)));
        builder.PushChild(new LayoutChild(LayoutSize.RatioOfRemainder(0.5f), LayoutSize.RatioOfTotal(1)));
        Layout layout = builder.Compile(new Rect(0, 0, 100, 50));
    }
}
