# 강화학습을 이용한 최적의 목표 추적 시스템 구현
유니티 ML-Agents와 파이썬 tensorFlow를 이용한 강화학습 구현


## 🖥️ 프로젝트 소개
우리나라에 일어나는 크고 작은 사건들을 뉴스로 접했는데, 그것을 보고 이미 높은 검거율 보다는 추가적인 피해를 막기 위한 범죄자의 빠른 검거가 필요하다는 것을 느끼고, Unity환경에서 ML-Agent를 통한 강화학습으 신속한 검거가 가능한 최적의 동선을 구현  
<br>

## 🕰️ 개발 기간
* 23.07.30일 - 23.08.16일

### 🧑‍🤝‍🧑 맴버구성
 - **팀장** : **이강림** 
 - **팀원1** : **김남호** 
 - **팀원2** : **이민성** 
 - **팀원3** : **박수빈** 


### ⚙️ 개발 환경
- `Unity`
- `Python`
- `C#`
- `Anaconda`

## 📌 주요 기능
#### 추적자 - <a href="https://github.com/FrogRim/DevDay/blob/main/Assets/DevDay/ChaserAgent.cs" >상세보기 - CODE 이동</a>
- 목표물과 본인과의 거리 관측
- 목표물과 가까워 질수록 상점 부여, 목표물을 잡으면 큰 상점 부여
- 장애물에 충돌시 벌점 부여

#### 목표물(도망자) - <a href="https://github.com/FrogRim/DevDay/blob/main/Assets/Goalgoing.cs" >상세보기 - CODE 이동</a>
- NavMeshAgent를 통해 장애물을 피해서 특정위치로 이동구현
- 도착지점에 오면 벌점 부여 후, 도망자의 위치를 초기화 시킴, 반대로 추적자에 잡히면 상점을 부여후 위치 초기화

#### Traning Area - <a href="https://github.com/FrogRim/DevDay/blob/main/Assets/DevDay/ChaserManager.cs" >상세보기 - CODE 이동</a>
- 강화학습을 하는 개체가 3개이므로 실제 보상과 보상함수로 나온 예상 보상점수의 차이가 클 것이라 생각해 여기서 1차적으로 보상함수의 수치를 조절해준 후, 각 개체의 EndEpisode() 메서드 호출
