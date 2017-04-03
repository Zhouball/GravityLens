Simple Buckyball generator v1.0

SUMMARY OF THE FUNCTIONALITY;

Creates a Dodecahedron Hierarchy. 
Hierarchy contains GameObject nodes and LineRenderer connecting lines.
Nodes are represented by spheres ( or GameObjects that you supply ). Lines are represented by LineRenderers. 
Lot's of parameters to configure in the Inspector view, like;
* Radius of the Dodecahedron
* Adding a box collider?
* Node GameObject ( to replace the default sphere )
* Scale of the node GameObject
* A node material if you want to override the nodes mian material
* LineRenderer Material
* Line width
* Gizmo's YES / NO 
 
HOW TO USE

There's several ways to add a Dodecahedron to your Unity scene.
Chose what best fit's you.

1. Manually add the DodecahedronHierarchy component to an Empty GameObject. 
2. Go to the menu "GameObject" => "Buckyball Hierarchy" => "Bucky Dodecahedron"
3. Right-click in the Hierarchy view and select "Buckyball Hierarchy" => "Bucky Dodecahedron"

All options immediatly draw your dodecahedron in the Scene view. 
Option 1 will draw it at the location of your GameObject.
Option 2 and 3 will draw it at Vector3.zero in global space