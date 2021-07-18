using Godot;
using System;

public class VanishingBlock : Spatial
{
    /* The vanishing block takes the shape of an upside down triangle:
     *
     *     *********    <- layer-0
     *      *******
     *       *****
     *        ***
     *         *        <- layer-4
     *
     * The animation takes 80 frames at the global FPS, with each change occuring after 8 frames.
     * In other words, every 8 frames, something happens, and 10 changes occur before the cycle repeats.
     * The only exception to this is between frames 60 and 64, where `layer-1` temporarily turns green.
     *
     * The animation beings with only `layer-0` visible, the stack "growing" down, then disappearing from the bottom up.
     * For 8 frames at the end, nothing is visible, and the player can fall through.
     *
     *   FRAME ACTION
     * -    0  `layer-0` visible
     * -    8  `layer-1` visible
     * -   16  `layer-2` visible
     * -   24  `layer-3` visible
     * -   32  `layer-4` visible
     * -   40  `layer-4` hidden
     * -   48  `layer-3` hidden
     * -   54  `layer-2` hidden
     * -   60  `layer-1` turns green
     * -   64  `layer-1` hidden and turns back blue
     * -   72  `layer-0` hidden
     */
    private int step = 0;
    private float time = 0.0f;

    private const int FRAMES = 80;
    private const int FPS = 20;

    private MeshInstance[] layers;
    private SpatialMaterial layer1Material;
    
    private Color blueColor;
    private Color greenColor;

    public override void _Ready()
    {
        layers = new MeshInstance[5] {
            (MeshInstance)GetNode("Layer-0"),
            (MeshInstance)GetNode("Layer-1"),
            (MeshInstance)GetNode("Layer-2"),
            (MeshInstance)GetNode("Layer-3"),
            (MeshInstance)GetNode("Layer-4"),
        };
        
        layers[0].Visible = true;
        layers[1].Visible = false;
        layers[2].Visible = false;
        layers[3].Visible = false;
        layers[4].Visible = false;
        
        layer1Material = (SpatialMaterial)layers[1].Mesh.SurfaceGetMaterial(0);
        blueColor = layer1Material.AlbedoColor;
        greenColor = Color.FromHsv(0.34f, 1.0f, 1.0f);
    }

    public override void _Process(float delta)
    {
        time = (time + delta) % (FRAMES / FPS);
        step = (int)(time * FPS);
        
        // Visibilities
        layers[0].Visible = (step < 72);
        layers[1].Visible = (step >= 8 && step < 64);
        layers[2].Visible = (step >= 16 && step < 56);
        layers[3].Visible = (step >= 24 && step < 48);
        layers[4].Visible = (step >= 32 && step < 40);
        
        // Second layer color
        layer1Material.AlbedoColor = (step >= 60 && step < 64) ? greenColor : blueColor;
    }
}
