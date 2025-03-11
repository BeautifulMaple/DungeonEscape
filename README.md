# DungeonEscape
 
## 🏰 프로젝트 소개
### DungeonEscape는 던전을 탈출하는 3D 액션 게임입니다. 플레이어는 다양한 함정과 장애물을 피하고, 벽을 타거나 점프대를 활용하여 목표 지점에 도달해야 합니다.
![전체 영상 (1)](https://github.com/user-attachments/assets/c3aa8174-2c1f-4ec5-ad74-50c318e7436d)

### 만든이     : 김태겸 
### 제작기간   : 2025년 3월 4일 ~ 3월 11일 (7일)
### Unity 버전 : 2022.3.17f1

# 🚀 주요 기능

### 기본 이동 및 점프

- WASD 이동 및 Space 점프 구현


### 체력바 UI

- 플레이어 체력을 UI로 표시

### 동적 환경 조사

- Raycast를 사용하여 상호작용 가능한 오브젝트의 정보를 표시

### 점프대

- 특정 오브젝트를 밟으면 높이 튀어 오름

### 아이템 데이터

- ScriptableObject를 사용해 아이템 데이터 관리


### 아이템 사용

- Coroutine을 사용해 일정 시간 지속되는 효과 적용 (체력, 허기, 스테미나 회복)

### 다양한 아이템 구현

- 점프력 증가
- 
### 레이저 트랩

- Raycast를 활용하여 플레이어 감지 및 트랩 발동

### 상호작용 오브젝트 UI

- 문 'E키를 눌러 열기' 등의 안내 표시

### 3인칭 시점
![제목 없는 동영상 - Clipchamp로 제작](https://github.com/user-attachments/assets/bd80d927-5091-4592-9e7a-a9b0ee69ead8)

- 3인칭 카메라 구현

### 움직이는 플랫폼

https://github.com/user-attachments/assets/310c2f14-b8ab-46ba-b92c-20b9a5b90f3b

- 시간에 따라 이동하는 플랫폼 구현


### 벽 타기 및 매달리기


https://github.com/user-attachments/assets/dfef910d-9d79-4b36-977c-bdcf9b1946c8


- Raycast와 ForceMode를 활용한 벽 매달리기 및 등반


### 장비 장착 시스템
![image](https://github.com/user-attachments/assets/e65c17de-1a0c-4081-ac06-5fb5a895f8f8)

- 장비 시스템 구현 (철제 검, 도끼)


### 플랫폼 발사기


https://github.com/user-attachments/assets/37daefd5-0cea-46c8-9188-34bccb459363


- 일정 시간 후 캐릭터를 발사

### 발전된 AI


https://github.com/user-attachments/assets/88595c1a-cb9f-4b30-b92d-214f3dc61cf8


- NavMesh 기반 AI 경로 탐색 및 동적 장애물 회피


