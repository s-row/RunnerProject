# Runner Project
Unity로 제작한 2D 모바일 게임입니다.
플레이어는 몬스터를 피하고, 열쇠를 획득하여 블록을 부시며 목표 지점인 깃발에 도착하는 것을 목표로 합니다.
DB는 Supabase를 활용하여 회원가입, 로그인, 점수 저장 등을 구현했습니다.

# 게임 소개
UI 버튼을 통해 좌우, 점프를 조작하는 2D 모바일 플랫폼 게임입니다.
플레이어는 몬스터와 충돌하면 HP가 감소하며 2초의 무적시간이 있습니다. 
맵 중간중간 배치한 열쇠를 먹어 블록을 부시면 HP를 1씩 회복할 수 있습니다.
최종적으로 맵 끝의 깃발에 도착하거나 HP가 0이 되면 끝나고, 총 점수와 랭킹이 표시됩니다.

# 개발 환경
- Engine : Unity 6
- 언어 : C#
- DB : Supabase
- 버전 정보 : GitHub


# 주요 기능
플레이어
- 버튼 UI를 이용한 좌우 이동
- 점프 기능
- HP 시스템
- 피격 애니메이션
- 피격 후 일정 시간 무적 처리

오브젝트
- 열쇠 획득
- 열쇠를 사용한 블록처리
- 하트 회복 아이템
- 깃발 도착 시 게임 종료

몬스터 
- 일반 몬스터와 개구리 몬스터로 구분
- 몬스터 스폰지점과 움직임 처리
- 접촉 시 데미지 처리

UI
- HP UI
- 획득한 열쇠 개수 UI
- 로그인 / 회원가입 UI
- 결과 화면에서 점수 및 랭킹 표시

DB
- Supabase 이메일 회원가입
- 점수 저장
- 저장된 점수를 기반으로 랭킹 계산

# 스크립트 구조
- PlayerControl.cs : 플레이어의 바닥 체크, 충돌 감지, 오브젝트 상호작용 처리
- PlayerStatus.cs : HP, 키 개수, 데미지, 회복, 무적 시간, 애니메이션 상태 관리
- ControlManager.cs : 모바일 버튼 입력, 이동, 점프, 좌우 반전 처리
- ObjectManager.cs : 열쇠, 블록, 하트, 골인 지점 처리
- ObjectItem.cs : 오브젝트 타입을 enum으로 구분
- MonsterMove.cs : 일반 몬스터와 개구리 몬스터 이동 처리
- MonsterSpawnManager.cs : 플레이어 위치에 따른 몬스터 스폰 처리
- UIManager.cs : 하트 UI와 키 개수 UI 갱신
- SupabaseManager.cs : Supabase 연결, 회원가입, 로그인, 점수 저장, 랭킹 조회
- LoginManager.cs : 로그인 화면 입력 검사 및 로그인 요청 처리
- SignUpManager.cs : 회원가입 화면 입력 검사 및 회원가입 요청 처리
- EndSceneManager.cs : 결과 화면 점수 표시, 점수 저장, 랭킹 표시
- CameraControl.cs : 플레이어 추적 및 카메라 이동 범위 제한
- SceneMoveManager.cs : 씬 이동 처리

# 구현하며 배운 점
- **Unity 2D의 물리와 충돌처리의 중요성**
 Rigidbody2D와 Collider2D를 활용해 플레이어, 몬스터, 바닥, 아이템 간의 충돌을 처리하면서 2D 게임에서 물리 설정이 gameplay에 직접적인 영향을 준다는 점을 배웠습니다.
- **Collider범위 설정의 중요성**
  Ground Check, 몬스터 충돌, 아이템 획득 처리 과정에서 Collider의 위치와 크기가 조금만 맞지 않아도 점프 판정이나 충돌 처리가 제대로 동작하지 않을 수 있다는 것을 경험했습니다.
- **스크립트의 기능별 분리**
  플레이어 이동, 충돌 처리, 상태 관리, 오브젝트 처리 기능이 한 스크립트에 모이면 수정과 디버깅이 어려워진다는 것을 느꼈습니다.
  그래서 입력은 `ControlManager`, 충돌은 `PlayerControl`, HP와 키 관리는 `PlayerStatus`, 아이템과 블록 처리는 `ObjectManager`로 나누어 코드 구조를 정리했습니다.











