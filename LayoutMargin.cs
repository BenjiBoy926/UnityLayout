﻿using UnityEngine;
using UnityEditor;

public class LayoutMargin
{
    public float top;
    public float bottom;
    public float left;
    public float right;

    public Vector2 horizontal
    {
        get
        {
            return new Vector2(left, right);
        }
        set
        {
            left = value.x;
            right = value.y;
        }
    }
    public Vector2 vertical
    {
        get
        {
            return new Vector2(top, bottom);
        }
        set
        {
            top = value.x;
            bottom = value.y;
        }
    }

    public float horizontalSpace
    {
        get
        {
            return left + right;
        }
    }
    public float verticalSpace
    {
        get
        {
            return top + bottom;
        }
    }

    public LayoutMargin() : this(0, 0, 0, 0) { }
    public LayoutMargin(Vector2 horizontal, Vector2 vertical) : this(vertical.x, vertical.y, horizontal.x, horizontal.y) { }
    public LayoutMargin(float t, float b, float l, float r)
    {
        top = t;
        bottom = b;
        left = l;
        right = r;
    }

    // Factory methods
    public static LayoutMargin Top(float t)
    {
        return new LayoutMargin(t, 0, 0, 0);
    }
    public static LayoutMargin Top()
    {
        return new LayoutMargin(EditorGUIUtility.standardVerticalSpacing, 0, 0, 0);
    }

    public static LayoutMargin Bottom(float b)
    {
        return new LayoutMargin(0, b, 0, 0);
    }
    public static LayoutMargin Bottom()
    {
        return new LayoutMargin(0, EditorGUIUtility.standardVerticalSpacing, 0, 0);
    }

    public static LayoutMargin Left(float l)
    {
        return new LayoutMargin(0, 0, l, 0);
    }
    public static LayoutMargin Right(float r)
    {
        return new LayoutMargin(0, 0, 0, r);
    }

    public static LayoutMargin Horizontal(Vector2 horizontal)
    {
        return new LayoutMargin(horizontal, Vector2.zero);
    }

    public static LayoutMargin Vertical(Vector2 vertical)
    {
        return new LayoutMargin(Vector2.zero, vertical);
    }
    public static LayoutMargin Vertical()
    {
        return new LayoutMargin(Vector2.zero, new Vector2(EditorGUIUtility.standardVerticalSpacing, EditorGUIUtility.standardVerticalSpacing));
    }
}
