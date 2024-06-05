using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RoomGenerator : MonoBehaviour
{
    public GameObject[] Grounds;
    public GameObject WallDown;
    public GameObject WallDownRight;
    public GameObject WallDownRightLink;
    public GameObject WallDownLeft;
    public GameObject WallDownLeftLink;
    public GameObject WallUp;
    public GameObject WallLeft;
    public GameObject WallRight;
    public List<Vector2> lst;

    public void Spawn_Rectangle(Vector3 position, GameObject Room)
    {
        Vector2 size = new Vector2 (Random.Range(4, 20), Random.Range(4, 20));
        
        RoomStats stats = Room.GetComponent<RoomStats>();
        stats.SetStats(size.x, size.y, position);
        for (int x = 0; x < size.x; x++) {
            for (int y = 0; y < size.y; y++) {
                Vector3 positionUpt = new Vector3 (position.x + (x * 0.16f), position.y + (y * 0.16f), 0f);
                Instantiate(Grounds[Random.Range(0, Grounds.Length)], positionUpt, Quaternion.identity, Room.transform);
            }
        }
    }

    float GridValue(float v)
    {
        return Mathf.FloorToInt(v / 0.16f) * 0.16f;
    }

    bool isInList(float x, float y, List<Vector2> PositionGrounds)
    {
        foreach (Vector2 vect in PositionGrounds) {
            if (Vector2.Distance(vect, new Vector2(x, y)) <= 0.1f)
                return true;
        }
        return false;
    }

    int getValueatPos(float x, float y, List<Vector2> PositionGrounds)
    {
        bool Up = isInList(x, y - 0.16f, PositionGrounds);
        bool Down = isInList(x, y + 0.16f, PositionGrounds);
        bool Left = isInList(x + 0.16f, y, PositionGrounds);
        bool Right = isInList(x - 0.16f, y, PositionGrounds);
        bool UpRight = isInList(x + 0.16f, y + 0.16f, PositionGrounds);
        bool UpLeft = isInList(x - 0.16f, y + 0.16f, PositionGrounds);
        bool DownRight = isInList(x + 0.16f, y - 0.16f, PositionGrounds);
        bool DownLeft = isInList(x - 0.16f, y - 0.16f, PositionGrounds);

        if (Up && Down && Left && Right)
            return 1;
        else if (Up)
            return 2;
        else if (Down)
            return 3;
        else if (Left)
            return 4;
        else if (Right)
            return 5;
        else if (UpRight)
            return 6;
        else if (UpLeft)
            return 7;
        else if (DownRight)
            return 8;
        else if (DownLeft)
            return 9;
        return 0;
    }

    void GenerateRightWall(float x, float y, List<Vector2> PositionGrounds)
    {
        bool Up = isInList(x, y - 0.16f, PositionGrounds);
        bool Down = isInList(x, y + 0.16f, PositionGrounds);
        bool Left = isInList(x + 0.16f, y, PositionGrounds);
        bool Right = isInList(x - 0.16f, y, PositionGrounds);
        bool UpRight = isInList(x + 0.16f, y + 0.16f, PositionGrounds);
        bool UpLeft = isInList(x - 0.16f, y + 0.16f, PositionGrounds);
        bool DownRight = isInList(x + 0.16f, y - 0.16f, PositionGrounds);
        bool DownLeft = isInList(x - 0.16f, y - 0.16f, PositionGrounds);

        if (Up && Down && Left && Right)
            Instantiate(WallUp, new Vector3(x, y, 0), Quaternion.identity, transform);
        else if (Up)
            Instantiate(WallUp, new Vector3(x, y, 0), Quaternion.identity, transform);
        else if (Down) {
            if (getValueatPos(x, y - 0.16f, PositionGrounds) == 4 || getValueatPos(x, y - 0.16f, PositionGrounds) == 6)
                Instantiate(WallDownRightLink, new Vector3(x, y, 0), Quaternion.identity, transform);
            else if (getValueatPos(x, y - 0.16f, PositionGrounds) == 5 || getValueatPos(x, y - 0.16f, PositionGrounds) == 7)
                Instantiate(WallDownLeftLink, new Vector3(x, y, 0), Quaternion.identity, transform);
            else 
                Instantiate(WallDown, new Vector3(x, y, 0), Quaternion.identity, transform);
        } else if (Left)
            Instantiate(WallLeft, new Vector3(x, y, 0), Quaternion.identity, transform);
        else if (Right)
            Instantiate(WallRight, new Vector3(x, y, 0), Quaternion.identity, transform);
        else if (UpRight)
            Instantiate(WallDownLeft, new Vector3(x, y, 0), Quaternion.identity, transform);
        else if (UpLeft)
            Instantiate(WallDownRight, new Vector3(x, y, 0), Quaternion.identity, transform);
        else if (DownRight)
            Instantiate(WallLeft, new Vector3(x, y, 0), Quaternion.identity, transform);
        else if (DownLeft)
            Instantiate(WallRight, new Vector3(x, y, 0), Quaternion.identity, transform);

    }

    public void GenerateWalls(List<Vector2> PositionGrounds)
    {
        lst = PositionGrounds;
        Vector2 Min;
        Vector2 Max;
        List<float> ValueX = new List<float>();
        List<float> ValueY = new List<float>();

        foreach (Vector2 vect in PositionGrounds) {
            ValueX.Add(vect.x);
            ValueY.Add(vect.y);
        }
        Min = new Vector2(ValueX.Min() - 0.16f, ValueY.Min() - 0.16f);
        Max = new Vector2(ValueX.Max() + 0.16f, ValueY.Max() + 0.16f);
        Debug.Log(Min);
        Debug.Log(Max);
        for (float x = Min.x; x < Max.x; x += 0.16f) {
            for (float y = Min.y; y < Max.y; y += 0.16f) {
                if (!isInList(x, y, PositionGrounds))
                    GenerateRightWall(x, y, PositionGrounds);
            }   
        }
    }
}
