using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderBufferRing : MonoBehaviour
{
    static RenderTexture[] ring = null;
    static int position = 0;

    public static void ClearBuffer()
    {
        if (Exists())
        {
            foreach (RenderTexture texture in ring)
            {
                texture.Release();
            }
        }
        ring = null;
    }

    public static void CreateBuffer(int size, RenderTexture template)
    {
        ClearBuffer();
        ring = new RenderTexture[size];
        for (int index = 0; index < size; ++index)
        {
            ring[index] = new RenderTexture(template);
        }
    }

    public static RenderTexture GetTexture(int index)
    {
        Debug.Assert(Exists());
        return ring[(position + index) % ring.Length];
    }

    public static RenderTexture GetFront()
    {
        Debug.Assert(Exists());
        return ring[position];
    }

    public static void CycleRing()
    {
        Debug.Assert(Exists());
        position++;
        if (position >= ring.Length)
        {
            position = 0;
        }
    }

    public static bool Exists()
    {
        return ring != null;
    }

    public static int Size()
    {
        Debug.Assert(Exists());
        return ring.Length;
    }
}
