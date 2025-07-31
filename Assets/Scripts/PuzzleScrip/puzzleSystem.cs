using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class puzzleSystem : MonoBehaviour
{
    [Header("Game Element")]
    [Range(2, 6)]
    [SerializeField] private int difficulty = 4;
    [SerializeField] private Transform gameHolder;
    [SerializeField] private Transform piecePrefab;

    [Header("UI Element")]
    [SerializeField] private List<Texture2D> imageTexture;
    [SerializeField] private Transform levelSelectPanel;
    [SerializeField] private Image levelSelecPrefab;
    [SerializeField] private GameObject correctbutton;

    private List<Transform> pieces;

    private Vector2Int dimensions;
    private float width;
    private float helght;

    private Transform draggingPiece = null;
    private Vector3 offset;

    private int piecesCorrect;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Create UI
        foreach (Texture2D texture in imageTexture)
        {
            Image image = Instantiate(levelSelecPrefab, levelSelectPanel);
            image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            // Assing button action
            image.GetComponent<Button>().onClick.AddListener(delegate { startGame(texture); } );
        }

        correctbutton.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit)
            {
                draggingPiece = hit.transform;
                offset = draggingPiece.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
                offset += Vector3.back;
            }
        }

        if (draggingPiece && Input.GetMouseButtonUp(0))
        {
            SnapAndDisabIfCorrect();
            draggingPiece.position += Vector3.forward;
            draggingPiece = null;
        }

        if (draggingPiece)
        {
            Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //newPosition.z = draggingPiece.position.z;
            newPosition += offset;
            draggingPiece.position = newPosition;
        }
    }

    //public void RestartGame()
    //{
    //    // Destroy all the puzzle pieces.
    //    foreach (Transform piece in pieces)
    //    {
    //        Destroy(piece.gameObject);
    //    }
    //    pieces.Clear();
    //    // Hide the outline
    //    gameHolder.GetComponent<LineRenderer>().enabled = false;
    //    // Show the level select UI.
    //    correctbutton.SetActive(false);
    //    levelSelectPanel.gameObject.SetActive(true);
    //}

    private void SnapAndDisabIfCorrect()
    {
        // We need to know the index of the piece to determine it's correct position.
        int pieceIndex = pieces.IndexOf(draggingPiece);

        // The coordinates of the piece in the puzzle.
        int col = pieceIndex % dimensions.x;
        int row = pieceIndex / dimensions.x;

        // The target position in the non-scaled coordinates.
        Vector2 targetPosition = new((-width * dimensions.x / 2) + (width * col) + (width / 2),
                                     (-helght * dimensions.y / 2) + (helght * row) + (helght / 2));

        // Check if we're in the correct location.
        if (Vector2.Distance(draggingPiece.localPosition, targetPosition) < (width / 2))
        {
            // Snap to our destination.
            draggingPiece.localPosition = targetPosition;

            // Disable the collider so we can't click on the object anymore.
            draggingPiece.GetComponent<BoxCollider2D>().enabled = false;

            // Increase the number of correct pieces, and check for puzzle completion.
            piecesCorrect++;
            if (piecesCorrect == pieces.Count)
            {
                correctbutton.SetActive(true);
            }
        }

    }

    public void startGame( Texture2D jigsawTexture)
    {
        // Hine UI
        levelSelectPanel.gameObject.SetActive(false);
        // store a list of the tranfrom for each jigsaw piece and can track them leter
        pieces = new List<Transform>();
        // Calcilate the size of ezch jigsaw piece, based on a difficulty setting.
        dimensions = GetDimensions(jigsawTexture, difficulty);
        // Calcilate the picec of the correct size with the correct texture.
        CreateJigsawPiece(jigsawTexture);

        Scatter();

        UpdateBorder();

        piecesCorrect = 0;
    }

    private void UpdateBorder()
    {
        LineRenderer lineRenderer = gameHolder.GetComponent<LineRenderer>();

        float halfWidth = (width * dimensions.x) / 2f;
        float halfHelght = (helght * dimensions.y) / 2f;

        float borderZ = 0f;

        lineRenderer.SetPosition(0, new Vector3(-halfWidth, halfHelght, borderZ));
        lineRenderer.SetPosition(1, new Vector3(halfWidth, halfHelght, borderZ));
        lineRenderer.SetPosition(2, new Vector3(halfWidth, -halfHelght, borderZ));
        lineRenderer.SetPosition(3, new Vector3(-halfWidth, -halfHelght, borderZ));

        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;

        lineRenderer.enabled = true;
    }

    private void Scatter()
    {
        float orthoHeight = Camera.main.orthographicSize;
        float screenAspect = (float)Screen.width / Screen.height;
        float orthoWidth = (screenAspect * orthoHeight);

        float pieceWidth = width * gameHolder.localScale.x;
        float piecehright = helght * gameHolder.localScale.y;

        orthoHeight -= pieceWidth;
        orthoWidth -= piecehright;

        foreach (Transform piece in pieces)
        {
            float x = Random.Range(-orthoWidth, orthoWidth);
            float y = Random.Range(-orthoHeight, orthoHeight);
            piece.position = new Vector3(x, y, -1);
        }
    }

    Vector2Int GetDimensions(Texture2D jigsawTexture, int difficulty)
    {
        Vector2Int dimensions = Vector2Int.zero;
        // difficulty is in the number of piece on the smallest texture dimension.
        // this helps ensure the piece are as square as possible.
        if (jigsawTexture.width < jigsawTexture.height)
        {
            dimensions.x = difficulty;
            dimensions.y = (difficulty * jigsawTexture.height) / jigsawTexture.width;
        }
        else
        {
            dimensions.x = (difficulty * jigsawTexture.width) / jigsawTexture.height;
            dimensions.y = difficulty;
        }
        return dimensions;
    }

    // create all the jigsaw piece
    void CreateJigsawPiece(Texture2D jigsawTexture)
    {
        // Calculate piece on the dimension,
        helght = 1f / dimensions.y;
        float aspect = (float)jigsawTexture.width / jigsawTexture.height;
        width = aspect / dimensions.x;

        for (int row = 0; row < dimensions.y; row++)
        {
            for (int col = 0; col < dimensions.x; col++)
            {

                Transform piece = Instantiate(piecePrefab, gameHolder);
                piece.localPosition = new Vector3(
                    (-width * dimensions.x / 2) + (width * col) + (width / 2),
                    (-helght * dimensions.y / 2) + (helght * row) + (helght / 2), -1);
                piece.localScale = new Vector3(width, helght, 1f);


                piece.name = $"Piece {(row * dimensions.x) + col}";
                pieces.Add(piece);

                float wideh1 = 1f / dimensions.x;
                float height1 = 1f / dimensions.y;

                Vector2[] uv = new Vector2[4];
                uv[0] = new Vector2 (wideh1 * col, height1 * row);
                uv[1] = new Vector2 (wideh1 * (col + 1), height1 * row);
                uv[2] = new Vector2 (wideh1 * col, height1 * (row + 1));
                uv[3] = new Vector2 (wideh1 * (col + 1), height1 * (row + 1));

                Mesh mesh = piece.GetComponent<MeshFilter>().mesh;
                mesh.uv = uv;

                piece.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", jigsawTexture);
            }
        }
    }
}
