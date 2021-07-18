using Godot;
using System;

public class MonkeyBars : Spatial
{
    /* The monkey bars are pretty simple; There's two forms:
     *
     *     --------    <- free hanging
     *
     *      +    +
     *     -+----+-    <- anchored
     *
     * The only thing that can be changed about this node is whether it is anchored or not.
     * If `Anchored` is `true`, the meshes in `anchors` are made visible; Otherwise, they are hidden.
     */
    private MeshInstance[] anchors;
    private bool anchored = false;

    public override void _Ready()
    {
        anchors = new MeshInstance[4] {
            (MeshInstance)GetNode("Anchor-Front-Right"),
            (MeshInstance)GetNode("Anchor-Front-Left"),
            (MeshInstance)GetNode("Anchor-Back-Right"),
            (MeshInstance)GetNode("Anchor-Back-Left"),
        };

        // Hide anchors if requested
        anchors[0].Visible = anchored;
        anchors[1].Visible = anchored;
        anchors[2].Visible = anchored;
        anchors[3].Visible = anchored;
    }
    
    [Export]
    public bool Anchored
    {
        get => anchored;
        set
        {
            anchored = value;

            // If this property is set before `_Ready` is called, an NPE is thrown
            if (anchors == null)
                return;

            anchors[0].Visible = anchored;
            anchors[1].Visible = anchored;
            anchors[2].Visible = anchored;
            anchors[3].Visible = anchored;
        }
    }
}
