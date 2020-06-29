using System.IO;
using MichalPocatko;
using JosefPelikan;
using DavidSosvald_MichalTopfer;
using Rendering;

//////////////////////////////////////////////////
// Rendering params.
context[PropertyName.CTX_WIDTH]         = 640; 
context[PropertyName.CTX_HEIGHT]        = 480;

Debug.Assert(scene != null);
Debug.Assert(scene is ITimeDependent);
Debug.Assert(context != null);

//////////////////////////////////////////////////
// CSG scene.

Animator a; // 'a' is used to register params (names, parsers, interpolators) during scene creation
if (context.ContainsKey("animator")) {
    scene.Animator = (ITimeDependent) ((Animator) context["animator"]).Clone();
a = null; // params were already registered when Animator was created (scene is the same)
}
else {
    string keyframes_file = Path.Combine(Path.GetDirectoryName((string)context[PropertyName.CTX_SCRIPT_PATH]), "AnimatorExample.yaml");
a = new Animator(keyframes_file);
scene.Animator = a;
    context["animator"] = a;
}

AnimatedCSGInnerNode root = new AnimatedCSGInnerNode(SetOperation.Union);
root.SetAttribute(PropertyName.REFLECTANCE_MODEL, new PhongModel());
root.SetAttribute(PropertyName.MATERIAL, new PhongMaterial(new double[] { 1.0, 0.6, 0.1 }, 0.1, 0.6, 0.4, 16));
scene.Intersectable = root;

// Background color.
scene.BackgroundColor = new double[] { 0.0, 0.05, 0.07 };
scene.Background = new StarBackground(scene.BackgroundColor, 600, 0.008, 0.5, 1.6, 1.0);

// Camera.
scene.Camera = new KeyframesAnimatedStaticCamera(a);

// Light sources.
scene.Sources = new System.Collections.Generic.LinkedList<ILightSource>();
scene.Sources.Add(new AmbientLightSource(0.8));
scene.Sources.Add(new PointLightSource(new Vector3d(-5.0, 3.0, -3.0), 1.0));

// --- NODE DEFINITIONS ----------------------------------------------------

// Sphere
Sphere s = new Sphere();
root.InsertChild(s, Matrix4d.Identity);
//s.SetAttribute(PropertyName.TEXTURE, new LODMoon(scene.Camera));
s.SetAttribute(PropertyName.MATERIAL, new PhongMaterial(new double[] { 0.0, 0.8, 0.1 }, 0.1, 0.1, 0.01, 5));

// Cube
Cube c = new Cube();
root.InsertChild(c, Matrix4d.Scale(1.2) * Matrix4d.CreateTranslation(1.5, 0.2, 2.4));
c.SetAttribute(PropertyName.MATERIAL, new PhongMaterial(new double[] { 1.0, 0.8, 0.1 }, 0.1, 0.1, 0.01, 5));

// Infinite plane with checker.
Plane pl = new Plane();
pl.SetAttribute(PropertyName.COLOR, new double[] { 0.5, 0.0, 0.0 });
pl.SetAttribute(PropertyName.TEXTURE, new CheckerTexture(1.0, 1.0, new double[] { 1.0, 1.0, 1.0 }));
root.InsertChild(pl, Matrix4d.RotateX(-MathHelper.PiOver2) * Matrix4d.CreateTranslation(0.0, -1.0, 0.0));

Vector3d bhPos = new Vector3d(0, 1, -3);
Vector3d bhCenter = bhPos;
BlackHole bh = new BlackHole(bhCenter);


root.InsertChild(bh, Matrix4d.CreateTranslation(bhPos));

if (a != null) {
    a.LoadKeyframes();
    context[PropertyName.CTX_START_ANIM] = a.Start;
    context[PropertyName.CTX_END_ANIM] = a.End;
    context[PropertyName.CTX_FPS] = 25.0;
}
