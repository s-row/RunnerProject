using UnityEngine;

// 오브젝트 종류를 구분하기 위한 타입
public enum ObjectType
{
    Key,
    Block,
    Heart,
    Goal
}

// 아이템, 블록, 목표 지점 같은 오브젝트에 붙여서 타입을 지정
public class ObjectItem : MonoBehaviour
{
    // Inspector에서 이 오브젝트의 종류를 선택
    public ObjectType objectType;
}