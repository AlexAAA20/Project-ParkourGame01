using UnityEngine;

public class TintEffect : MonoBehaviour
{
    public DaytimeCycle cycle;
    public AnimationCurve R = AnimationCurve.Linear(0, 0, 1, 1);
    public AnimationCurve G = AnimationCurve.Linear(0, 0, 1, 1);
    public AnimationCurve B = AnimationCurve.Linear(0, 0, 1, 1);
    Color baseColor;
    SpriteRenderer renderer;

    public void Start ( )
    {
        renderer = GetComponent<SpriteRenderer>();
        baseColor = renderer.color;
    }

    public void Update ( )
    {
        Color toSet = baseColor;

        Color toAdd = new(0, 0, 0);
        Color gradient = cycle.currCycle.Gradient(cycle.fufilness);
        toAdd.r = R.Evaluate( gradient.r );
        toAdd.g = G.Evaluate( gradient.g );
        toAdd.b = B.Evaluate( gradient.b );

        toSet += toAdd;

        renderer.color = toSet;
    }
}
