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
- **다중 광고 네트워크** - 추상 인터페이스로 다양한 광고 SDK(AdMob, Unity Ads, IronSource 등) 지원
- **보상형 광고** - 인센티브 광고의 보상 콜백 처리 내장
- **에디터 통합** - 커스텀 인스펙터로 간편 설정

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

### 주요 클래스 및 인터페이스

- **`AdvertisementComponent`**: Unity MonoBehaviour 컴포넌트, 광고 시스템과의 메인 진입점.
- **`IAdvertisementManager`**: 광고 매니저의 핵심 인터페이스. 모든 광고 네트워크 구현은 이 인터페이스를 구현해야 합니다.
- **`BaseAdvertisementManager`**: `IAdvertisementManager`를 구현한 선택적 추상 기본 클래스. 공통 콜백 처리 로직을 제공합니다.

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
