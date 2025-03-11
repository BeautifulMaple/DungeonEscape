# 🏰 DungeonEscape
 
## 프로젝트 소개
### DungeonEscape는 던전을 탈출하는 3D 액션 게임입니다. 플레이어는 다양한 함정과 장애물을 피하고, 벽을 타거나 점프대를 활용하여 목표 지점에 도달해야 합니다.
![전체 영상 (1)](https://github.com/user-attachments/assets/c3aa8174-2c1f-4ec5-ad74-50c318e7436d)

## 📌 프로젝트 정보  
- **제작자**: 김태겸  
- **제작 기간**: 2025년 3월 4일 ~ 3월 11일 (7일)  
- **Unity 버전**: 2022.3.17f1  

---

## 🚀 주요 기능  

### 🎮 기본 이동 및 점프  
- WASD 이동 및 Space 점프 구현  
- `Player Input`을 활용하여 자연스러운 조작 가능  

### ❤️ 체력바 UI  
- 플레이어의 체력을 UI로 표시  
- 체력이 변할 때마다 UI가 자동 갱신됨  

### 🔍 동적 환경 조사 (Raycast)  
- 플레이어가 바라보는 오브젝트의 정보를 UI에 표시  
- 예) 문에 마우스를 올리면 'E키를 눌러 열기' 안내 표시  

### 🦘 점프대  
- 특정 오브젝트를 밟으면 위로 튀어 오름  
- `ForceMode.Impulse`를 사용하여 자연스러운 점프 구현  

### 📦 아이템 시스템  
- `ScriptableObject`를 사용해 다양한 아이템 데이터 관리  

### 📦 아이템 사용
- Coroutine을 사용해 일정 시간 지속되는 효과 적용 (점프력 증가)

### 📦 다양한 아이템 구현
- 점프력 증가: 일정 시간 동안 더 높이 점프 가능
- 플레이어 회복 : 체력, 허기, 스태미나를 회복
  
### 🔫 레이저 트랩  
- `Raycast`를 활용하여 플레이어 감지 후 트랩 발동
- 경고문 표시

### 상호작용 오브젝트 UI
- 문 'E키를 눌러 열기' 등의 안내 표시

### 📌 3인칭 시점  
- 3인칭 카메라를 구현하여 플레이어를 따라다니도록 설정  
- 마우스를 이용해 자유롭게 시점을 조작 가능  
![제목 없는 동영상 - Clipchamp로 제작](https://github.com/user-attachments/assets/bd80d927-5091-4592-9e7a-a9b0ee69ead8)


### ⚙ 움직이는 플랫폼  
- 일정한 경로를 따라 자동으로 움직이는 플랫폼 구현  
- 플레이어가 플랫폼 위에 올라서면 함께 이동  

https://github.com/user-attachments/assets/310c2f14-b8ab-46ba-b92c-20b9a5b90f3b


### 🧗 벽 타기 및 매달리기  
- `Raycast`와 `ForceMode`를 활용하여 벽 매달리기 구현  
- 벽을 타고 위로 이동 가능  

https://github.com/user-attachments/assets/dfef910d-9d79-4b36-977c-bdcf9b1946c8


### ⚔ 장비 장착 시스템  
- 철제 검, 도끼 등 장비를 장착하여 무기 능력치 획득  
- 장비 변경 시 UI 갱신  
![image](https://github.com/user-attachments/assets/e65c17de-1a0c-4081-ac06-5fb5a895f8f8)



### 🚀 플랫폼 발사기  
- 일정 시간이 지나면 캐릭터를 특정 방향으로 발사  
- `ForceMode.Impulse`를 활용하여 자연스러운 물리 반응 적용

https://github.com/user-attachments/assets/37daefd5-0cea-46c8-9188-34bccb459363



### 🧠 발전된 AI  
- `NavMesh` 기반 AI 경로 탐색 및 동적 장애물 회피  
- 특정 장애물을 피하거나 다른 경로를 탐색하도록 구현

https://github.com/user-attachments/assets/88595c1a-cb9f-4b30-b92d-214f3dc61cf8

---

## 🔧 설치 및 실행 방법  

1️⃣ **게임 다운로드**  
   - [GitHub Releases](https://github.com/BeautifulMaple/DungeonEscape/releases) 페이지에서 최신 버전을 다운로드하세요.  
   - `DungeonEscape_vX.X.X.zip` 파일을 받습니다.  

2️⃣ **압축 해제**  
   - 다운로드한 ZIP 파일을 원하는 폴더에 압축 해제합니다.  

3️⃣ **게임 실행**  
   - `DungeonEscape.exe` 파일을 실행하면 게임이 시작됩니다. 
