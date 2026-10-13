<div align="center">

<img src="https://download.alianblank.com/gameframex/gameframex_logo_320.png" alt="Game Frame X Logo" width="160" />

# GameFrameX Advertisement 패키지

[![License](https://img.shields.io/github/license/GameFrameX/com.gameframex.unity.advertisement)](https://github.com/GameFrameX/com.gameframex.unity.advertisement/blob/main/LICENSE.md)
[![Version](https://img.shields.io/github/v/release/GameFrameX/com.gameframex.unity.advertisement)](https://github.com/GameFrameX/com.gameframex.unity.advertisement/releases)
[![Unity Version](https://img.shields.io/badge/Unity-2019.4-black?logo=unity)](https://unity.com/)
[![Documentation](https://img.shields.io/badge/Documentation-docs-blue)](https://gameframex.doc.alianblank.com)

인디 게임 개발자를 위한 올인원 솔루션 · 인디 개발자의 꿈을 실현

<br />

[문서](https://gameframex.doc.alianblank.com) · [빠른 시작](#빠른-시작) · QQ 그룹: 467608841 / 233840761

<br />

[English](README.md) | [简体中文](README.zh-CN.md) | [繁體中文](README.zh-TW.md) | [日本語](README.ja.md) | **한국어**

</div>

## 목차

- [프로젝트 개요](#프로젝트-개요)
- [빠른 시작](#빠른-시작)
- [사용 예시](#사용-예시)
- [문서 및 자료](#문서-및-자료)
- [커뮤니티 및 지원](#커뮤니티-및-지원)
- [라이선스](#라이선스)

---

## 프로젝트 개요

GameFrameX Advertisement는 GameFrameX 프레임워크의 광고 컴포넌트로, Unity 프로젝트에 광고 기능을 빠르게 통합할 수 있습니다.

### 주요 기능

- **간편한 통합** - 최소 설정으로 광고 지원 추가
- **컴포넌트 기반 설계** - GameFrameX 모듈식 아키텍처 기반
- **다중 광고 네트워크** - 추상 인터페이스로 다양한 광고 SDK(csj, TopOn, WeChat/Douyin/Kuaishou/Alipay 미니게임 등) 지원
- **보상형 광고** - 인센티브 광고의 보상 콜백 처리 내장
- **채널 레지스트리** - 채널 패키지 Bootstrap 자가 등록, 상호 배타적 해석 + 명시적 채널 ID 오버라이드, 타입 이름 문자열 리플렉션 없음

### 시스템 요구사항

- Unity 2017.1 이상

---

## 빠른 시작

### 설치

다음 방법 중 하나를 선택하세요:

1. Unity 프로젝트의 `Packages/manifest.json`을 편집하여 `scopedRegistries` 섹션을 추가하세요:
   ```json
   {
     "scopedRegistries": [
       {
         "name": "GameFrameX",
         "url": "https://gameframex.upm.alianblank.uk",
         "scopes": [
           "com.gameframex"
         ]
       }
     ],
     "dependencies": {
       "com.gameframex.unity.advertisement": "1.5.0"
     }
   }
   ```

   `scopes`는 이 레지스트리를 통해 어떤 패키지를 해석할지 제어합니다. `com.gameframex`로 시작하는 패키지만 이 레지스트리에서 가져옵니다.

2. `manifest.json`의 `dependencies`에 직접 추가:
   ```json
   {
      "com.gameframex.unity.advertisement": "https://github.com/gameframex/com.gameframex.unity.advertisement.git"
   }
   ```
3. Unity의 **Package Manager**에서 **Git URL**을 사용하여 추가: `https://github.com/gameframex/com.gameframex.unity.advertisement.git`
4. 리포지토리를 Unity 프로젝트의 `Packages` 디렉토리에 클론하세요. 자동으로 로드됩니다.
## 사용 예시

### 기본 사용법

```csharp
using GameFrameX.Advertisement.Runtime;
using UnityEngine;
using System;

public class MyAdCaller : MonoBehaviour
{
    private AdvertisementComponent adComponent;

    void Start()
    {
        // AdvertisementComponent 인스턴스 가져오기
        adComponent = GetComponent<AdvertisementComponent>();
    }

    // 광고 로드
    public void LoadAd()
    {
        if (adComponent != null)
        {
            adComponent.Load(
                (message) => { Debug.Log("광고 로드 성공: " + message); },
                (error) => { Debug.LogError("광고 로드 실패: " + error); }
            );
        }
    }

    // 광고 표시
    public void ShowAd()
    {
        if (adComponent != null)
        {
            adComponent.Show(
                (message) => { Debug.Log("광고 표시 성공: " + message); },
                (error) => { Debug.LogError("광고 표시 실패: " + error); },
                (reward) =>
                {
                    if (reward)
                    {
                        Debug.Log("광고 시청 완료, 보상 지급");
                    }
                    else
                    {
                        Debug.Log("광고를 끝까지 시청하지 않음");
                    }
                }
            );
        }
    }
}
```

### 채널 발견 및 설정 (Bootstrap 자가 등록)

발견 계층은 레지스트리 기반입니다: 각 채널 패키지는 자체 `XxxAdvertisementBootstrap`(`RuntimeInitializeOnLoadMethod(BeforeSceneLoad)`)을 통해 채널 팩토리를 `AdvertisementRegistry`에 등록합니다. 설치되지 않은 채널 패키지의 코드는 제거되므로 등록이 발생하지 않습니다(참조＝등록).

- **상호 배타적 해석**: `AdvertisementRegistry.ResolveManager()`를 인수 없이 호출하면 등록 순서대로 첫 번째 채널을 가져옵니다(미니게임 채널은 플랫폼 매크로로 자연스럽게 상호 배타적)；
- **명시적 라우팅 오버라이드**: `AdvertisementRegistry.ResolveManager("csj")`로 채널을 명시 지정합니다. csj / TopOn 동시 설치 시 수동 선택에 사용；
- **강타입 설정**: 채널 설정(appId / adUnitId 등)은 시작 시 각 채널 Bootstrap의 `Config` 정적 속성에 할당되며, 팩토리 클로저가 첫 해석 시 채널을 인스턴스화하고 강타입 `AdvertisementConfig` 서브클래스를 주입합니다；
- **모듈 연결**: `AdvertisementComponent`의 모듈 슬롯은 메인 패키지의 기본 구현 `AdvertisementManager`가 차지합니다(채널 구현은 더 이상 각자 모듈 슬롯을 차지하지 않음). 모든 호출과 프레임워크 종료(`IShutdownableModule`)는 레지스트리의 해석 결과로 전달됩니다.

```csharp
using GameFrameX.Advertisement.Runtime;
using GameFrameX.Advertisement.CSJ.Runtime; // CSJ 채널 예시

// 시작 시(첫 광고 호출 전) 채널 설정 주입
CSJAdvertisementBootstrap.Config = new CSJAdvertisementConfig
{
    appId = "당신의 CSJ AppId",
    adUnitId = "당신의 광고 유닛 ID",
    isDebug = false,
};

// 이후 평소처럼 AdvertisementComponent 사용. 또는 채널로 명시적으로 라우팅
var manager = AdvertisementRegistry.ResolveManager("csj");
```

### 파괴적 변경 및 마이그레이션 (v2.1)

- **Inspector 타입 드롭다운 제거**: `AdvertisementComponent`가 더 이상 구현 타입 선택 드롭다운을 제공하지 않습니다(기존 `componentType` 타입 이름 문자열 리플렉션 발견 경로 삭제됨). 발견 계층은 `AdvertisementRegistry`로 통일됩니다；
- **매크로 선택 광고 유닛 ID 필드 제거**: 컴포넌트의 `Android/iOS/WebGL/WeChat/Douyin/Kuaishou 광고 유닛 ID` 6개 직렬화 필드가 삭제되었고, 광고 유닛 ID는 각 채널 Bootstrap의 강타입 설정으로 이동했습니다(예: `CSJAdvertisementBootstrap.Config.adUnitId`). 이 값을 해당 채널 패키지의 시작 시 설정으로 옮겨 주세요；
- **TopOn 앱 설정을 씬에서 이전**: `TopOnAdvertisementBridgeLink`의 appId / appKey 직렬화 필드가 삭제되고 `TopOnAdvertisementBootstrap.Config`로 이전되었습니다(BridgeLink는 브리지 콜백 책임만 유지). 기존 프로젝트는 수동으로 설정을 옮겨야 합니다；
- **`AdvertisementConfigAttribute` 삭제됨**: Manager와 설정 타입의 리플렉션 연결(Inspector 리플렉션 그리기용)은 Inspector 리플렉션 체인과 함께 제거되었으며, 등록 항목이 팩토리와 설정을 직접 담습니다.

### 주요 클래스 및 인터페이스

- **`AdvertisementComponent`**: Unity MonoBehaviour 컴포넌트, 광고 시스템과의 메인 진입점(공개 API와 호출 동작은 변경 없음).
- **`IAdvertisementManager`**: 광고 매니저의 핵심 인터페이스. 모든 광고 네트워크 구현은 이 인터페이스를 구현해야 합니다.
- **`BaseAdvertisementManager`**: `IAdvertisementManager`를 구현한 선택적 추상 기본 클래스. 공통 콜백 처리 로직을 제공합니다.
- **`AdvertisementRegistry`**: 광고 채널 레지스트리. `Register` / `Unregister` / `Contains` / `GetManager` / `GetAllManagers` / `ResolveManager(channelId = null)`(상호 배타적 해석 + 명시적 라우팅 오버라이드).
- **`AdvertisementChannelRegistration`**: 채널 등록 설명자(채널 id + 표시 이름 + `Func<IAdvertisementManager>` 팩토리, 클로저 내에서 강타입 설정 주입).
- **`AdvertisementManager`**: 메인 패키지 기본 구현. `IAdvertisementManager` 모듈 슬롯을 차지하고 호출을 레지스트리의 해석 결과로 전달합니다(`IShutdownableModule` 종료 전달 포함).

---

## 문서 및 자료

- **전체 문서**: [https://gameframex.doc.alianblank.com](https://gameframex.doc.alianblank.com)
- **이슈 트래커**: [GitHub Issues](https://github.com/GameFrameX/com.gameframex.unity.advertisement/issues)

---

## 커뮤니티 및 지원

- **QQ 그룹**: [467608841](https://qm.qq.com/cgi-bin/qm/qr?k=sYFd1nv6m2KZIWFLorZ5pBR0AE5ZhbuL&jump_from=webapi&authKey=oCu+uoL3n35fT5SEt7iLgGtROPxh31n/rHUxRlp0w1f+j38W4tKBuWyRH3KEdwHN)
- **기능 요청**: [GitHub Discussions](https://github.com/GameFrameX/com.gameframex.unity.advertisement/discussions)

---


## 의존성

| 패키지 | 설명 |
|--------|------|
| (无) | - |


## 변경 로그

[Releases](https://github.com/GameFrameX/gameframex/com.gameframex.unity.advertisement/releases)에서 변경 로그를 확인하세요.
## 라이선스

자세한 내용은 [LICENSE.md](LICENSE.md) 파일을 참조하세요.
