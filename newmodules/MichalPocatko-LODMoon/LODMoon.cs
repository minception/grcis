using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using MathSupport;
using OpenTK;
using Utilities;
using Rendering;
using System.IO;

namespace MichalPocatko
{
  /// <summary>
  /// A level of detail texture
  /// </summary>
  [Serializable]
  public class LODMoon : ITexture
  {
    KeyObjectMaps<int,Bitmap> Textures;
    Vector3d cameraPos;
    string mipmapsPath;
    public LODMoon (Vector3d cameraPos)
    {
      mipmapsPath = "..\\newmodules\\MichalPocatko-LODMoon\\mipmaps";
    }
    /// <summary>
    /// Apply the relevant value-modulation in the given Intersection instance.
    /// Simple variant, w/o an integration support.
    /// </summary>
    /// <param name="inter">Data object to modify.</param>
    /// <returns>Hash value (texture signature) for adaptive subsampling.</returns>
    public virtual long Apply (Intersection inter)
    {
      Vector3d corner1, corner2;
      inter.Solid.GetBoundingBox(out corner1, out corner2);
      inter.Solid.GetAttribute(ISceneNode)
      int x = (int)(inter.TextureCoord.X*Texture.Width);
      int y = (int)(inter.TextureCoord.Y*Texture.Height);
      Color pixelColor = Texture.GetPixel(x, y);
      Util.ColorCopy(new double[] { pixelColor.R/255.0, pixelColor.G/255.0, pixelColor.B /255.0}, inter.SurfaceColor);
      inter.textureApplied = true;
      return Texture.GetHashCode();
      throw new Exception();
    }
  }
}
