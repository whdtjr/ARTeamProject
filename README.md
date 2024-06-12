# ARTeamProject
## 목차
1. [필요한 data 정리](#1-필요한-data-정리)
2. [program 시나리오](#2-program-시나리오)
3. [Scripts 설명](#3-scripts-설명)
4. [ARPlacements](#arplacements)
5. [EnablePlacementOnLocalized](#enableplacementonlocalized)
6. [PersistentObjects](#persistentobjects)
7. [SaveAndLoad](#saveandload)
8. [CreateGameObject](#creategameobject)

## 1. 필요한 data 정리
Script파일들을 모두 다운 받고 unity project에서 필요한 기본 setting을 마친 후에 
[ligthship dashborad](https://lightship.dev/account/dashboard)로 이동한다.

여기서 The Gandhi Monument at SF (샌프란시스코에 있는 간디 동상)을 찾아서 mesh를 다운로드한다.

playback 기능을 이용하기 위해서 [lightship play back sample](https://niantic.dev/docs/ardk/experimental/playback_mode.html)
로 이동한 후에 playback을 다운 받고 적용시키면 된다.

게임 효과들을 다운 받기 위해서는 Unity Asset store에서 particle을 검색하고 무료 Asset을 받는다.

## 2. program 시나리오
구현된 program은 scan된 위치에 도착하게 되면 object를 놓을 수 있게 된다. object는 2가지 효과에 대한 object이고 터치 시에 사용할 수 있게 된다.
save 시에 해당 object가 저장되고 다시 program을 켰을 때 load되어서 똑같이 보여야 한다. (그런데 모바일에서 test를 해야하는데 load가 되지 않고 있음.. 디버깅이 필요)
## 3. Scripts 설명

## ARPlacements
object를 버튼을 눌러 선택하고 해당 object를 감지된 plane에 올려놓는다. 현재 올릴 수 있는 object는 2개고 button을 추가하여 더 늘려줄 수 있다.
```cs
    private ARRaycastManager arRaycastManager;
    [SerializeField]private Button button1; // fire
    [SerializeField]private Button button2; // water
    [SerializeField] private List<GameObject> instantiatedObject;  // plane위에 올릴 object들을 저장할 list button을 통해 해당 list에서 가져와서 등록하게 된다.
    private List<GameObject> instantiatedObjects = new(); // 이름을 잘못 지었음.. 나중에 수정 예정
    private Camera mainCam;
    private GameObject target;
```
`camera`는 touch를 처리하기 위해 가져오는 component이다. 
`target`은 list에서 선택된 것을 넣을 변수이다.
`instantiatedObjects`는 생성된 object들을 모두 저장하는 list이다. 관리를 위해 넣어둠..
`arRaycastManager`는 touch 처리를 위해 가져오는 compoenet이다. 

`button`은 onclick.Addlistener로 함수를 실행시킬 수 있다.
`setObject1`은 첫번째 object를 설정하는 함수이다.

이 script의 작동 방식은 마우스가 눌리면 UI인지 아닌지 판단을하고 UI가 눌렸다면 아무것도 하지 않고 아니라면 `Ray`를 쏜다. 
해당 위치에서 날아간 `Ray`와 감지된 `plane`과 만난 지점을 `hits`에 저장하고 그 지점에 object를 `instantiate`하여 생성해준다. 만약 object의 위치가 서로 겹친다면 생성하지 않는다.


## EnablePlacementOnLocalized
ARLocation이 감지되거나 감지가 해제되었다면 `onlocalized` 수행한다. 
```cs
_arLocationManager.locationTrackingStateChanged += OnLocalized;
```
위의 코드는 감지 상태가 변하면 `onlocalized`를 실행시킬 수 있도록 등록하고 있는 코드이다.
모든 unity의 클래스의 부모인 `MonoBehaviour`에는 enable 속성이 존재한다. 그래서 감지가 되었다면 enable를 true로 설정하여 ARPlacements와 ARPlaneManager를 이용할 수 있도록 설정한다.
## PersistentObjects
저장할 object의 data를 정의해놓은 class이다. 
```cs
 public struct persistentObjectData
 {
     public string _arLocation;
     public string _prefabName;
     public string _uuID;

     public Vector3 _position;
     public Quaternion _rotation;
     public Vector3 _scale;

     public string getARLocation()
     {
         return _arLocation;
     }
     public persistentObjectData(string arLocation, string prefabName, string uuID, Vector3 position, Quaternion rotation, Vector3 scale)
     {
         _arLocation = arLocation;
         _prefabName = prefabName;
         _uuID = uuID;

         _position = position;
         _rotation = rotation;
         _scale = scale;
     }
 }
```
위의 data가 저장되게 된다. saveEvent가 수행될 경우 saveData함수가 실행된다.
## SaveAndLoad
save와 load를 수행하는 class이다. button을 통해 save를 하고 save한 data가 있다면 자동으로 바로 load를 하게 된다.
`Application.persistentDataPath` 경로로 가서 저장을 하게 되는데 위의 경로는 desktop 기준으로 user-AppData-LocalLow-DefaultCompany-[UnityProjectName]에 저장된
그렇지만 모바일에서는 이 경로가 같은지 확인을 할 수가 없었다. (모바일에서 실행 시 save, load가 되는 것 같지 않아서 확인을 해야함..)
## CreateGameObject
`SaveAndLoad` class에서 load 시에 object를 instantiate해야 하는데 필요한 것은 Gameobject이고 local에 저장한 data에 맞게 object를 가져와야하므로 `CreateGameObject` script를 통해서
object의 이름과 해당 object를 dictionary로 저장하여 이름을 가지고 gameobject를 뽑아내는 class이다.  

