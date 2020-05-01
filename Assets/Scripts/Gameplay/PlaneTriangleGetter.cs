using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *      How to use:
 *  .DestroySquareAtPosition( Vector3 ) // 
 *  .DestroyTriangleAtPosition( Vector3 )   // Returns true if a triangle was found; 
 * 
 */


public class PlaneTriangleGetter : MonoBehaviour {
    public static PlaneTriangleGetter Instance { get; set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public int GetTriangleByCoordinates(Vector3 coordinates) {
        Vector3 fromPosition = new Vector3(coordinates.x, transform.position.y + 10, coordinates.z);
        Vector3 toPosition   = fromPosition - new Vector3(0, 20, 0);
        Vector3 direction    = toPosition - fromPosition;
        _direction = direction;
        _fromPosition = fromPosition;
        RaycastHit[] hits = Physics.RaycastAll(fromPosition, direction, 1000.0f);
        foreach (var hit in hits) {
            if (hit.transform.gameObject == gameObject) {
                return hit.triangleIndex;
            }
        }
        return -1;
    }

    private void deleteBothTriangles(int t1, int t2) {
        Destroy(GetComponent<MeshCollider>());
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        var oldTriangles = mesh.triangles;
        var newTriangles = new int[mesh.triangles.Length - 3];
        int i = 0, j = 0;
        while (j < mesh.triangles.Length) {
            if (j != t1 * 3 && j != t2 * 3) {
                newTriangles[i++] = oldTriangles[j++];
                newTriangles[i++] = oldTriangles[j++];
                newTriangles[i++] = oldTriangles[j++];
            } else {
                j += 3;
            }
        }
        GetComponent<MeshFilter>().mesh.triangles = newTriangles;
        gameObject.AddComponent<MeshCollider>();
    }
    private int findVertex(Vector3 v) {
        Vector3[] theVertices = GetComponent<MeshFilter>().mesh.vertices;
        for (int i = 0; i<theVertices.Length; i++)
            if (theVertices[i] == v)
                return i;
        return -1;
    }
    private int findOtherTriangle(Vector3 v1, Vector3 v2, int mainTriangleIndex) {
        var mesh = GetComponent<MeshFilter>().mesh;
        int[] triangles = mesh.triangles;
        Vector3[] vertices = mesh.vertices;
        int i = 0, j = 0;
        int found = 0;
        while (j < triangles.Length) {
            if (j/3 != mainTriangleIndex) {
                if (vertices[triangles[j]] == v1 && (vertices[triangles[j+1]] == v2 || vertices[triangles[j+2]] == v2)) {
                    return j/3;
                } else if (vertices[triangles[j]] == v2 && (vertices[triangles[j+1]] == v1 || vertices[triangles[j+2]] == v1)) {
                    return j/3;
                } else if (vertices[triangles[j+1]] == v2 && (vertices[triangles[j]] == v1 || vertices[triangles[j+2]] == v1)) {
                    return j/3;
                } else if (vertices[triangles[j+1]] == v1 && (vertices[triangles[j]] == v2 || vertices[triangles[j+2]] == v2)) {
                    return j/3;
                }
            }
            j += 3;
        }
        return -1;
    }

    public void DeleteSquare(int triangleIndex) {
        int[] theTriangles = GetComponent<MeshFilter>().mesh.triangles;
        Vector3[] theVertices = GetComponent<MeshFilter>().mesh.vertices;
        Vector3 p0 = theVertices[theTriangles[triangleIndex * 3 + 0]];
        Vector3 p1 = theVertices[theTriangles[triangleIndex * 3 + 1]];    // Get the 3 points of that triangle
        Vector3 p2 = theVertices[theTriangles[triangleIndex * 3 + 2]];
        float edge1 = Vector3.Distance(p0, p1);
        float edge2 = Vector3.Distance(p0, p2);
        float edge3 = Vector3.Distance(p1, p1);
        Vector3 shared1;                        // Get the 2 points of the hypothenuse
        Vector3 shared2;
        if (edge1 > edge2 && edge1 > edge3) {
            shared1 = p0;
            shared2 = p1;
        } else if (edge2 > edge1 && edge2 > edge3) {
            shared1 = p0;
            shared2 = p2;
        } else {
            shared1 = p1;
            shared2 = p2;
        }
        int vertex1 = findVertex(shared1);  // Get their position in the vertex array
        int vertex2 = findVertex(shared2);
        int otherTriangleIndex = findOtherTriangle(theVertices[vertex1], theVertices[vertex2], triangleIndex);
        deleteBothTriangles(triangleIndex, otherTriangleIndex);
    }

    public void DeleteTriangle(int triangleIndex) {
        Destroy(GetComponent<MeshCollider>());
        Mesh mesh = transform.GetComponent<MeshFilter>().mesh;
        int[] oldTriangles = mesh.triangles;
        int[] newTriangles = new int[mesh.triangles.Length - 3];
        int i = 0, j = 0;
        while (j < mesh.triangles.Length) {
            if (j != triangleIndex * 3) {
                newTriangles[i++] = oldTriangles[j++];
                newTriangles[i++] = oldTriangles[j++];
                newTriangles[i++] = oldTriangles[j++];
            } else {
                j += 6;
            }
        }
        GetComponent<MeshFilter>().mesh.triangles = newTriangles;
        gameObject.AddComponent<MeshCollider>();
    }

    public bool DeleteTriangleAtPosition(Vector3 pos) {
        int triangle = GetTriangleByCoordinates(pos);
        if (triangle == -1) return false;
        DeleteTriangle(triangle);
        return true;
    }

    public bool DeleteSquareAtPosition(Vector3 pos) {
        int triangleIndex = GetTriangleByCoordinates(pos);
        if (triangleIndex == -1) {
            print("Returning false");
            return false;
        } else {
            DeleteSquare(triangleIndex);
            return true;
        }
    }

    // Debug variables
    // public bool  _debug = false;
    // public float _x = 0.0f;
    // public float _y = 0.0f;
    // public float _z = 0.0f;
    public Vector3 _fromPosition;
    public Vector3 _direction;
    // public GameObject _character;

    // void Update() {
    //     if (_debug) {
    //         if (Input.GetMouseButtonDown(0)) {
    //             if (_character != null) {
    //                 _character.transform.position = new Vector3(_x, _y, _z);
    //             }
    //             print(DeleteSquareAtPosition(new Vector3(_x, _y, _z)));
    //         }
    //         if (_fromPosition != null) {
    //             Debug.DrawRay(_fromPosition, _direction, Color.red);
    //         }
    //     }
    // }
}
