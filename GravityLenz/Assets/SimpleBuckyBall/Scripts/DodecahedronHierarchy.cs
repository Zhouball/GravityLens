using UnityEngine;

namespace Platipus
{
	public class DodecahedronHierarchy : MonoBehaviour {

		[Space(10)]
		[Header("General")]

		[SerializeField]
		private float radius = 1f;
		[SerializeField]
		private bool AddABoxCollider = false;

		[Space(10)]
		[Header("Nodes")]
		[SerializeField]
		private GameObject node;
		[Range(0.1f, 10.0f)]
		[SerializeField]
		private float nodeScale = .1f; 
		[SerializeField]
		private bool OverrideNodeMaterial = false;
		[SerializeField]
		private Material nodeMaterial;

		[Space(10)]
		[Header("Lines")]
		[SerializeField]
		private Material lineMaterial;
		[Range(0.1f, 10.0f)]
		[SerializeField]
		private float lineWidth = .1f;


		[Space(10)]
		[Header("Gizmos")]
		[SerializeField]
		private bool showGizmos = false; 

		private static string dodecahedronIcon = "Dodecahedron";

		private BoxCollider boxCollider;
		private GameObject[] nodes;
		private LineRenderer[] linerenderers;
		private int destroyedNodeIndex = -1;
		private Material materialBrokenLink;

		private Vector3[] vertices = new Vector3[] 
			{ 
				new Vector3(    0f, -0.8f,  0.6f ),
				new Vector3( -0.6f, -0.8f,  0.2f ), 
				new Vector3( -0.4f, -0.8f, -0.5f ),
				new Vector3(  0.4f, -0.8f, -0.5f ), 
				new Vector3(  0.6f, -0.8f,  0.2f ),
				new Vector3(    0f, -0.2f,    1f ),
				new Vector3( -0.9f, -0.2f,  0.3f ),
				new Vector3( -0.6f, -0.2f, -0.8f ),
				new Vector3(  0.6f, -0.2f, -0.8f ),
				new Vector3(  0.9f, -0.2f,  0.3f ),
				new Vector3( -0.6f,  0.2f,  0.8f ),
				new Vector3( -0.9f,  0.2f, -0.3f ),
				new Vector3(    0f,  0.2f, -1.0f ),
				new Vector3(  0.9f,  0.2f, -0.3f ),
				new Vector3(  0.6f,  0.2f,  0.8f ),
				new Vector3( -0.4f,  0.8f,  0.5f ),
				new Vector3( -0.6f,  0.8f, -0.2f ),
				new Vector3(    0f,  0.8f, -0.6f ),
				new Vector3(  0.6f,  0.8f, -0.2f ),
				new Vector3(  0.4f,  0.8f,  0.5f )
			};

		private int[][] lines = new int[][]
			{
                /*
				new int[]{ 19, 15 },
				new int[]{ 15, 16 },
				new int[]{ 16, 17 },
				new int[]{ 17, 18 },
				new int[]{ 18, 19 },

				new int[]{  4,  0 },
				new int[]{  0,  1 },
				new int[]{  1,  2 },
				new int[]{  2,  3 },
				new int[]{  3,  4 },

				new int[]{  7, 12 },
				new int[]{ 12,  8 },
				new int[]{  8, 13 },
				new int[]{ 13,  9 },
				new int[]{  9, 14 },
				new int[]{ 14,  5 },
				new int[]{  5, 10 },
				new int[]{ 10,  6 },
				new int[]{  6, 11 },
				new int[]{ 11,  7 },
				
				new int[]{  2,  7 },
				new int[]{  3,  8 },
				new int[]{  4,  9 },
				new int[]{  0,  5 },
				new int[]{  1,  6 },

				new int[]{ 10, 15 },
				new int[]{ 11, 16 },
				new int[]{ 12, 17 },
				new int[]{ 13, 18 },
				new int[]{ 14, 19 }
			
                */
			};

		// Use this for initialization
		void Start () {
		
			initializeDodeca();

			materialBrokenLink = new Material( Shader.Find("Standard") );
			materialBrokenLink.SetColor( "_Color",  Color.red );
		}

		void Update () {
		
			for(int i=0; i < nodes.Length; ++i)
			{
				GameObject goNode = nodes[i];
				if( goNode )
					goNode.transform.rotation = Quaternion.Euler( 0f, 0f, 0f );
			}

		}

		void OnDrawGizmos()
		{
			if( ! showGizmos )
				return;

			Gizmos.DrawIcon( transform.position, dodecahedronIcon, true );

			Gizmos.color = Color.green;
			for( int i = 0; i < lines.Length; ++i )
			{
				Vector3 v1 = transform.position + ( vertices[ lines[i][0] ] * transform.lossyScale.x * radius ) ;
				Vector3 v2 = transform.position + ( vertices[ lines[i][1] ] * transform.lossyScale.x * radius ) ;

				Gizmos.DrawLine( v1, v2 );
			}

			Gizmos.color = Color.blue;
			for( int i=0; i < vertices.Length; ++i )
			{
				Gizmos.DrawWireSphere( transform.position + ( vertices[i] * transform.lossyScale.x * radius ) , 1f );
			}
		}

		public void destroyRandomNode()
		{
			destroyNode( Random.Range(0,20) );
		}

		public void destroyNode( int nodeIndex )
		{
			destroyedNodeIndex = nodeIndex;
			updateAppearance();
		}

		public void repairNode( GameObject newNodeDummy )
		{
			if( destroyedNodeIndex >= 0 && destroyedNodeIndex < nodes.Length && newNodeDummy != null )
			{
				GameObject newNode = Instantiate( newNodeDummy );

				GameObject oldNode = nodes[destroyedNodeIndex];
				newNode.transform.SetParent( oldNode.transform.parent );
				newNode.transform.localScale = oldNode.transform.localScale;
				newNode.transform.localRotation = oldNode.transform.localRotation;
				newNode.transform.localPosition = oldNode.transform.localPosition;

				nodes[destroyedNodeIndex] = newNode;
				Destroy( oldNode );
			}
			destroyedNodeIndex = -1;

			updateAppearance();
		}

		public void destroyTopNode()
		{
			destroyNode( getIndexOfHighestNode() );
		}

		public int getIndexOfHighestNode()
		{
			int indexOfHighestNode = 0;

			for( int i = 1; i < nodes.Length; ++i )
			{
				if( nodes[i].transform.position.y > nodes[indexOfHighestNode].transform.position.y )
				{
					indexOfHighestNode = i;
				}
			}

			return indexOfHighestNode;
		}

		public Vector3 positionOfDestroyedNode()
		{
			if( destroyedNodeIndex >= 0 && destroyedNodeIndex < nodes.Length )
			{
				return ( nodes[ destroyedNodeIndex ] != null ) ? nodes[destroyedNodeIndex].transform.position : transform.position;
			}
			else
			{
				return transform.position;
			}
		
		}

		public void initializeDodeca()
		{
			transform.rotation = Quaternion.Euler( Vector3.zero );

			//Create the hierarchy
			int childs = transform.childCount;
			for (int i = childs - 1; i >= 0; i--)
			{
				if( Application.isEditor )
				{
					GameObject.DestroyImmediate( transform.GetChild(i).gameObject, false );
				}
				else
				{
					GameObject.Destroy(transform.GetChild(i).gameObject);
				}	
			}

			if( lineMaterial == null )
			{
				lineMaterial = new Material( Shader.Find("Standard") );
				lineMaterial.SetColor( "_Color",  Color.green );
			}

			if( nodeMaterial == null )
			{
				nodeMaterial = new Material( Shader.Find("Standard") );
				nodeMaterial.SetColor( "_Color",  Color.blue );
			}
			//Create vertices / nodes
			nodes = new GameObject[vertices.Length];
			for( int i  = 0; i < vertices.Length ; ++i )
			{
				GameObject goNode;
				if( node!=null )
				{
					goNode = Instantiate<GameObject>( node );
				}
				else
				{
					goNode = GameObject.CreatePrimitive( PrimitiveType.Sphere );
				}

				if( OverrideNodeMaterial )
				{
					goNode.GetComponent<Renderer>().sharedMaterial = nodeMaterial;
				}

				goNode.name = "node_"+i;
				goNode.transform.SetParent( transform );
				goNode.transform.localPosition = vertices[i] * radius;
				goNode.transform.localScale = new Vector3(nodeScale,nodeScale,nodeScale);
				goNode.SetActive( i != destroyedNodeIndex );

				nodes[i] = goNode;
			}
			//Create Lines
			linerenderers = new LineRenderer[lines.Length];
			for( int i = 0; i < lines.Length; ++i )
			{
				int v1 = lines[i][0];
				int v2 = lines[i][1];

				bool brokenLink = ( v1 == destroyedNodeIndex ) || ( v2 == destroyedNodeIndex );

				GameObject goLine = new GameObject();
				goLine.name = "line_"+v1+"_"+v2;
				goLine.transform.SetParent( transform );
				goLine.transform.localPosition = Vector3.zero;
				goLine.transform.localScale = Vector3.one;
				
				LineRenderer linerenderer = goLine.AddComponent<LineRenderer>();
				linerenderer.SetVertexCount( 2 );
				linerenderer.useWorldSpace = false;
				linerenderer.material = brokenLink ? materialBrokenLink : lineMaterial;
				linerenderer.SetWidth( lineWidth, lineWidth );
				linerenderers[i] = linerenderer;

				Vector3[] lineVertices = new Vector3[2];
				lineVertices[0] = vertices[ v1 ] * radius;
				lineVertices[1] = vertices[ v2 ] * radius;

				if(brokenLink)
				{
					Vector3 midpoint = new Vector3( (lineVertices[0].x+lineVertices[1].x)/2f, (lineVertices[0].y+lineVertices[1].y)/2f, (lineVertices[0].z+lineVertices[1].z)/2f );
					if( v1 == destroyedNodeIndex )
						lineVertices[0] = midpoint;
					else if( v2 == destroyedNodeIndex )
						lineVertices[1] = midpoint;	
				}

				linerenderer.SetPositions( lineVertices );
			}

			//Create BoxCollider if required
			if( AddABoxCollider )
			{
				boxCollider = gameObject.AddComponent<BoxCollider>();
				boxCollider.size = new Vector3( radius * 1.8f, radius * 1.6f, radius * 1.6f );
			}
		}

		public void updateAppearance()
		{
			for( int i  = 0; i < nodes.Length ; ++i )
			{
				GameObject goNode = nodes[i];
				goNode.SetActive( i != destroyedNodeIndex );
			}
			for( int i = 0; i < lines.Length; ++i )
			{
				LineRenderer linerenderer = linerenderers[i];
				
				int v1 = lines[i][0];
				int v2 = lines[i][1];

				bool brokenLink = ( v1 == destroyedNodeIndex ) || ( v2 == destroyedNodeIndex );

				Vector3[] lineVertices = new Vector3[2];
				lineVertices[0] = vertices[ v1 ] * radius;
				lineVertices[1] = vertices[ v2 ] * radius;

				if(brokenLink)
				{
					Vector3 midpoint = new Vector3( (lineVertices[0].x+lineVertices[1].x)/2f, (lineVertices[0].y+lineVertices[1].y)/2f, (lineVertices[0].z+lineVertices[1].z)/2f );
					if( v1 == destroyedNodeIndex )
						lineVertices[0] = midpoint;
					else if( v2 == destroyedNodeIndex )
						lineVertices[1] = midpoint;	
				}

				linerenderer.material = brokenLink ? materialBrokenLink : lineMaterial;
				linerenderer.SetPositions( lineVertices );
			}
		}
	}
}