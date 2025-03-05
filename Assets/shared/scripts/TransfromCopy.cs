using UnityEngine;

public static class TransformExtensions
{
    public static Transform CopyTransform(this Transform source)
    {
        GameObject copy = new GameObject("CopiedTransform");
        Transform copyTransform = copy.transform;

        copyTransform.position = source.position;
        copyTransform.rotation = source.rotation;
        copyTransform.localScale = source.localScale;

        return copyTransform;
    }
}

