using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
namespace ZombieSoccer.GameLayer.UI
{
#if UNITY_EDITOR
    [ExecuteInEditMode, RequireComponent(typeof(CanvasRenderer))]
#endif
    public class UIMeshRenderer : MonoBehaviour
    {
        public Material material;
        public bool randomColor = true;
        public Color color;
        //public float scale = 1f;

        public float widthRandom = 1f;
        public float heightRandom = 1f;

        [HideInInspector]
        [SerializeField]
        private Mesh mesh;
        private CanvasRenderer canvasRenderer;
        private RectTransform rectTransform;

        [SerializeField]
        public Color linkedColor;

//#if UNITY_EDITOR // only compile in editor
        private Mesh currentMesh;
        private Material currentMaterial;
        private float currentScale;
//#endif
        //public bool forceRebuild = false;
        public void Start()
        {
            canvasRenderer = GetComponent<CanvasRenderer>();
            rectTransform = GetComponent<RectTransform>();

            //if (mesh == null || forceRebuild)
            GenerateMesh(true);
        }

        public void OnEnable()
        {
            Start();
            //GenerateMesh();
        }

        //IEnumerator UpdateData()
        //{
        //    yield return new WaitForEndOfFrame();
        //    GenerateMesh();
        //}

        public void OnDisable()
        {
            canvasRenderer?.Clear();
        }

//#if UNITY_EDITOR // only compile in editor
        public void Update()
        {
            if (mesh != currentMesh || material != currentMaterial)// || !Mathf.Approximately(scale, currentScale))
            {
                SetMesh();
            }
        }
//#endif

        public void SetMesh()
        {
            canvasRenderer.SetMesh(mesh);
            canvasRenderer.SetColor(color);

            if (material)
                canvasRenderer.SetMaterial(material, null);

            if (linkedCanvasRenderer)
            {
                linkedCanvasRenderer.SetMesh(mesh);
                linkedCanvasRenderer.SetColor(linkedColor);
            }
        }

        public void GenerateMesh(bool t)
        {
            StartCoroutine(SetMeshIenum());
        }

        IEnumerator SetMeshIenum()
        {
            yield return new WaitForEndOfFrame();
            GenerateMesh();
        }

        [Button]
        public Mesh GenerateMesh()
        {
            if (rectTransform == null)
                return null;

            Rect drawArea = rectTransform.rect;
            mesh = new Mesh();

            //Debug.Log(string.Format("xMin={0}, xMax={1}, yMin={2}, yMax={3}, w={4}, h={5}", drawArea.xMin, drawArea.xMax, drawArea.yMin, drawArea.yMax, drawArea.width, drawArea.height));

            Vector3[] vertices = new Vector3[4]
            {
                new Vector3(Random.Range(-widthRandom, widthRandom) - drawArea.width / 2f, Random.Range(-heightRandom, heightRandom) - drawArea.height / 2f),
                new Vector3(Random.Range(-widthRandom, widthRandom) + drawArea.width / 2f, Random.Range(-heightRandom, heightRandom) - drawArea.height / 2f),
                new Vector3(Random.Range(-widthRandom, widthRandom) - drawArea.width / 2f, Random.Range(-heightRandom, heightRandom) + drawArea.height / 2f),
                new Vector3(Random.Range(-widthRandom, widthRandom) + drawArea.width / 2f, Random.Range(-heightRandom, heightRandom) + drawArea.height / 2f)
            };

            mesh.vertices = vertices;

            int[] tris = new int[6]
            {
                // lower left triangle
                0, 2, 1,
                // upper right triangle
                2, 3, 1
            };
            mesh.triangles = tris;

            Vector3[] normals = new Vector3[4]
            {
                -Vector3.forward,
                -Vector3.forward,
                -Vector3.forward,
                -Vector3.forward
            };
            mesh.normals = normals;

            Vector2[] uv = new Vector2[4]
            {
                new Vector2(0, 0),
                new Vector2(1, 0),
                new Vector2(0, 1),
                new Vector2(1, 1)
            };
            mesh.uv = uv;

            if (randomColor)
                color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));

            SetMesh();

            return mesh;
        }


        private void OnDrawGizmos()
        {
            return;
            if (mesh == null)
                return;

            foreach (var point in mesh.vertices)
            {
                var translatePoint = rectTransform.TransformPoint(point);
                Gizmos.DrawSphere(translatePoint, 5f);
            }
        }

        private CanvasRenderer linkedCanvasRenderer;
        public void AddLinkedCanvasRenderer(CanvasRenderer canvasRenderer)
        {
            linkedCanvasRenderer = canvasRenderer;
        }
    }
}