using System;
using OpenTK;
using Rendering;
using Utilities;
using MathSupport;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MichalPocatko
{
  [Serializable]
  public class BlackHole : Sphere
  {
    double radius = 0.1;
    public BlackHole (Vector3d center)
    {
      SetAttribute(PropertyName.COLOR, new double[] { 0.0, 0.0, 0.0 });
      SetAttribute(PropertyName.MATERIAL, new PhongMaterial(new double[] { 1.0, 0.8, 0.1 }, 0.1, 0.1, 0.01, 5));


    RecursionFunction del = (Intersection i, Vector3d dir, double importance, out RayRecursion rr) =>
    {
      Vector3d iPos = i.CoordWorld;
      double dist = LinePointDistance(iPos, dir, center);
      double direct = 0.5 - i.TextureCoord.X;
      if(dist < radius)
      {
        Util.ColorCopy(new double[] {0.0, 0.0, 0.0 }, i.SurfaceColor);
      }
      else
      {
        double angle = (1 - dist)*(1-dist) * Math.PI/2;
        dir = Vector3d.Transform(dir, Matrix4d.Rotate(center, angle));
      }
      rr = new RayRecursion(
      Util.ColorClone(i.SurfaceColor, direct),
      new RayRecursion.RayContribution(i, dir, importance));
      return 144L;
    };
    SetAttribute(PropertyName.RECURSION, del);
  }
    private double LinePointDistance (Vector3d a, Vector3d b, Vector3d p)
    {
      Vector3d d = (b - a) / Vector3d.Distance(b, a);
      Vector3d v = p - a;
      double t = Vector3d.Dot(v, d);
      Vector3d P = a + t * d;
      return Vector3d.Distance(P, p);
    }
  }

}
