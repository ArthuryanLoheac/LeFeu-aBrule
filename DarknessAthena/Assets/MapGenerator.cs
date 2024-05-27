using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject Room_Indicator;
    public GameObject Ground;
    public int nb_rooms = 20;
    public List<Vector2> points;

    private void Spawn_Rectangle(Vector3 position, GameObject Room)
    {
        Vector2 size = new Vector2 (Random.Range(4, 20), Random.Range(4, 20));
        
        RoomStats stats = Room.GetComponent<RoomStats>();
        stats.SetStats(size.x, size.y, position);
        for (int x = 0; x < size.x; x++) {
            for (int y = 0; y < size.y; y++) {
                Vector3 positionUpt = new Vector3 (position.x + (x * 0.16f), position.y + (y * 0.16f), 0f);
                Instantiate(Ground, positionUpt, Quaternion.identity, Room.transform);
            }
        }
    }

    private void Generate_Map()
    {
        Vector3 position;
        for (int i = 0; i < nb_rooms; i++) {
            position = new Vector3 (Random.Range(-(nb_rooms / 4f), (nb_rooms / 4f)), Random.Range(-(nb_rooms / 4f), (nb_rooms / 4f)), 0f);
            position = new Vector3 (Mathf.FloorToInt(position.x / 0.16f) * 0.16f, Mathf.FloorToInt(position.y / 0.16f) * 0.16f, 0f);
            GameObject Room = Instantiate(Room_Indicator, position, Quaternion.identity, this.transform);
            Spawn_Rectangle(position, Room);
        }
    }

    private bool IsAnyRoomOverLapped()
    {
        for (int current = 0; current < transform.childCount; current++) {
            for (int other = 0; other < transform.childCount; other++) {
                if (other != current) {
                    if (transform.GetChild(current).GetComponent<RoomStats>().isOverLapping(transform.GetChild(other).gameObject))
                        return true;
                }
            }
        }
        return false;
    }

    private void Seperate_Rooms()
    {
        do {
            for (int current = 0; current < transform.childCount; current++) {
                for (int other = 0; other < transform.childCount; other++) {
                    if (other == current || !transform.GetChild(current).GetComponent<RoomStats>().isOverLapping(transform.GetChild(other).gameObject)) continue;
                    
                    var direction = (transform.GetChild(other).GetComponent<RoomStats>().Middle - transform.GetChild(current).GetComponent<RoomStats>().Middle).normalized;

                    transform.GetChild(current).GetComponent<RoomStats>().Move(-direction, 0.16f);
                    transform.GetChild(other).GetComponent<RoomStats>().Move(direction, 0.16f);
                }
            }
        } while (IsAnyRoomOverLapped());
    }

    void DrawTriangles(List<EdgeVect> triangles)
    {
        foreach (EdgeVect t in triangles)
        {
            Debug.DrawLine(t.p1, t.p2, Color.red, 100f);
        }
    }

    void Make_Triangulation()
    {
        //Get All Center of rooms
        for (int i = 0; i < transform.childCount; i++) {
            points.Add(new Vector2 (transform.GetChild(i).position.x + ((transform.GetChild(i).gameObject.GetComponent<RoomStats>().sizeX * 0.16f) / 2f),
                                    transform.GetChild(i).position.y + ((transform.GetChild(i).gameObject.GetComponent<RoomStats>().sizeY * 0.16f) / 2f)));
        }
        //Add all points to delaunay List
        List<Point> delaunayPoints = new List<Point>();
        foreach (Vector2 v in points)
            delaunayPoints.Add(new Point(v.x, v.y));

        //Compute Triangles
        List<Triangle> triangles = DelaunayTriangulation.Triangulate(delaunayPoints);
        //Get Edges From triangles
        List<EdgeVect> edges = EdgeVect.getEdgesFromTriangles(triangles);
        // Get Minimal Edges 
        List<EdgeVect> newedges = MinimalSpanningTree.ComputeMinimalSpanningTree(edges, nb_rooms);
        List<EdgeVect> newedges2 = MinimalSpanningTree.AddRandomEdges(edges, newedges);
        DrawTriangles(newedges2);
    }

    void Awake()
    {
        Generate_Map();
        Seperate_Rooms();
        Make_Triangulation();
    }
}
