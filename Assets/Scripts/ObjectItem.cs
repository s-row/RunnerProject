using UnityEngine;
public enum ObjectType
{
    Key,
    Block,
    Heart,
    Goal
}
public class ObjectItem : MonoBehaviour
{
    public ObjectType objectType;
    
}
