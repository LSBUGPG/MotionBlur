using UnityEngine;

public class MotionBlur : MonoBehaviour
{
    public float rate = 0.02f;
    public Material material;
    RenderTexture [] frames;
    float time;

    private void OnDestroy()
    {
        if (frames != null)
        {
            foreach (var frame in frames)
            {
                frame.Release();
            }
        }
    }

    private void Update()
    {
        time += Time.deltaTime;
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (frames == null)
        {
            frames = new RenderTexture[2];
            for (int i = 0; i < frames.Length; ++i)
            {
                frames[i] = new RenderTexture(source);
            }
        }


        if (time > rate)
        {
            time = 0;
            RenderTexture t = frames[0];
            frames[0] = frames[1];
            frames[1] = t;
            Graphics.CopyTexture(source, frames[1]);
        }
        material.SetFloat("_Level", Mathf.InverseLerp(0.0f, rate, time));
        material.SetTexture("_BackTex", frames[0]);
        Graphics.Blit(frames[1], destination, material);
    }
}
