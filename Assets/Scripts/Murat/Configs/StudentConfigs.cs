
[System.Serializable]
public class StudentConfigs
{
    public float MinLike {get{return minLike;}}
    public float MaxLike {get{return maxLike;}}
    public float WinTolerance {get{return winTolerance;}}

    [System.Serializable]
    class Dialogue {
        enum Type{
            LIKE_TYPE,
            LIKE_ITEM,
            DISLIKE
        }
        string part1;
        string part2;
        string target;
        bool isGeneric;
        Type t;
    }

    float minLike, maxLike;
    float winTolerance;
}
