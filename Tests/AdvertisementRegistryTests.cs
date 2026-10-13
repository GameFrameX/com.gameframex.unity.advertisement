// GameFrameX 组织下的以及组织衍生的项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
//
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE 文件。
//
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任。

using System;
using System.Reflection;
using GameFrameX.Advertisement.Runtime;
using GameFrameX.Runtime;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace GameFrameX.Advertisement.Tests
{
    public class AdvertisementRegistryTests
    {
        private class StubAdvertisementManager : BaseAdvertisementManager, IShutdownableModule
        {
            public AdvertisementConfig ReceivedConfig;
            public int InitializeCallCount;
            public int ShutdownCallCount;
            public string LastExtraKey;
            public string LastExtraValue;

            public override void Initialize(AdvertisementConfig config)
            {
                InitializeCallCount++;
                ReceivedConfig = config;
            }

#pragma warning disable CS0618
            public override void Initialize(string adUnitId, bool debug = false) { }

            public override void Play(Action<bool> playResult, string customData = null) { }

            public override void Show(Action<string> success, Action<string> fail, Action<bool> onShowResult, string customData = null) { }

            public override void Load(Action<string> success, Action<string> fail, string customData = null) { }
#pragma warning restore CS0618

            public override void Play(AdvertisementPlayOption option) { }

            public override void SetExtraData(string key, string value)
            {
                LastExtraKey = key;
                LastExtraValue = value;
            }

            public void Shutdown()
            {
                ShutdownCallCount++;
            }
        }

        private sealed class OtherAdvertisementManager : StubAdvertisementManager
        {
        }

        [SetUp]
        public void SetUp()
        {
            // EditMode 下注册表告警经 GameFrameX Log 输出，需要挂接日志助手才能被 LogAssert 捕获
            GameFrameworkLog.SetLogHelper(new DefaultLogHelper());
            AdvertisementRegistry.Reset();
        }

        [TearDown]
        public void TearDown()
        {
            AdvertisementRegistry.Reset();
            GameFrameworkLog.SetLogHelper(null);
        }

        [Test]
        public void Register_ThenGetManager_CreatesInstanceLazilyAndCaches()
        {
            var factoryCallCount = 0;
            AdvertisementRegistry.Register(new AdvertisementChannelRegistration("stub", "存根渠道", () =>
            {
                factoryCallCount++;
                return new StubAdvertisementManager();
            }));

            Assert.IsTrue(AdvertisementRegistry.Contains("stub"));
            Assert.AreEqual(0, factoryCallCount, "工厂在首次获取前不应被调用");

            var first = AdvertisementRegistry.GetManager("stub");
            var second = AdvertisementRegistry.GetManager("stub");

            Assert.IsNotNull(first);
            Assert.IsInstanceOf<StubAdvertisementManager>(first);
            Assert.AreSame(first, second, "同一渠道应复用实例");
            Assert.AreEqual(1, factoryCallCount);
        }

        [Test]
        public void Register_DuplicateChannelId_KeepsFirstRegistration()
        {
            var first = new StubAdvertisementManager();
            AdvertisementRegistry.Register(new AdvertisementChannelRegistration("stub", "第一个", () => first));
            AdvertisementRegistry.Register(new AdvertisementChannelRegistration("stub", "第二个", () => new OtherAdvertisementManager()));

            Assert.AreSame(first, AdvertisementRegistry.GetManager("stub"), "重复注册应保留首个");
        }

        [Test]
        public void GetManager_UnknownOrNullChannel_ReturnsNull()
        {
            Assert.IsNull(AdvertisementRegistry.GetManager("not-exists"));
            Assert.IsNull(AdvertisementRegistry.GetManager(null));
            Assert.IsNull(AdvertisementRegistry.GetManager(string.Empty));
            Assert.IsFalse(AdvertisementRegistry.Contains("not-exists"));
        }

        [Test]
        public void Register_NullArguments_AreIgnoredOrThrown()
        {
            AdvertisementRegistry.Register(null);
            Assert.IsFalse(AdvertisementRegistry.Contains("stub"));

            Assert.Throws<ArgumentNullException>(() => new AdvertisementChannelRegistration(null, "无效", () => new StubAdvertisementManager()));
            Assert.Throws<ArgumentNullException>(() => new AdvertisementChannelRegistration("stub", "无效", null));
        }

        [Test]
        public void Unregister_RemovesChannelAndCachedInstance()
        {
            AdvertisementRegistry.Register(new AdvertisementChannelRegistration("stub", "存根渠道", () => new StubAdvertisementManager()));
            var manager = AdvertisementRegistry.GetManager("stub");
            Assert.IsNotNull(manager);

            AdvertisementRegistry.Unregister("stub");
            Assert.IsFalse(AdvertisementRegistry.Contains("stub"));
            Assert.IsNull(AdvertisementRegistry.GetManager("stub"));

            AdvertisementRegistry.Register(new AdvertisementChannelRegistration("stub", "存根渠道", () => new StubAdvertisementManager()));
            Assert.IsInstanceOf<StubAdvertisementManager>(AdvertisementRegistry.GetManager("stub"), "重新注册后可再次获取");
        }

        [Test]
        public void Reset_ClearsAllRegistrations()
        {
            AdvertisementRegistry.Register(new AdvertisementChannelRegistration("stub", "存根渠道", () => new StubAdvertisementManager()));
            AdvertisementRegistry.Reset();

            Assert.IsFalse(AdvertisementRegistry.Contains("stub"));
            Assert.IsNull(AdvertisementRegistry.GetManager("stub"));
        }

        [Test]
        public void GetAllManagers_ReturnsManagersInRegistrationOrder()
        {
            var first = new StubAdvertisementManager();
            var second = new OtherAdvertisementManager();
            AdvertisementRegistry.Register(new AdvertisementChannelRegistration("channel-a", "渠道A", () => first));
            AdvertisementRegistry.Register(new AdvertisementChannelRegistration("channel-b", "渠道B", () => second));

            var managers = AdvertisementRegistry.GetAllManagers();

            Assert.AreEqual(2, managers.Count);
            Assert.AreSame(first, managers[0]);
            Assert.AreSame(second, managers[1]);
        }

        [Test]
        public void ResolveManager_NoRegistrations_ReturnsNull()
        {
            Assert.IsNull(AdvertisementRegistry.ResolveManager());
            Assert.IsNull(AdvertisementRegistry.ResolveManager(null));
            Assert.IsNull(AdvertisementRegistry.ResolveManager(string.Empty));
        }

        [Test]
        public void ResolveManager_UnknownChannel_ReturnsNull()
        {
            AdvertisementRegistry.Register(new AdvertisementChannelRegistration("stub", "存根渠道", () => new StubAdvertisementManager()));

            Assert.IsNull(AdvertisementRegistry.ResolveManager("not-exists"), "显式路由到未注册渠道返回 null");
        }

        [Test]
        public void ResolveManager_Empty_ResolvesFirstRegisteredChannelByPriority()
        {
            var first = new StubAdvertisementManager();
            var second = new OtherAdvertisementManager();
            AdvertisementRegistry.Register(new AdvertisementChannelRegistration("channel-a", "渠道A", () => first));
            AdvertisementRegistry.Register(new AdvertisementChannelRegistration("channel-b", "渠道B", () => second));

            LogAssert.Expect(LogType.Warning, new System.Text.RegularExpressions.Regex("no explicit channel id"));
            var resolved = AdvertisementRegistry.ResolveManager();

            Assert.AreSame(first, resolved, "互斥解析按注册顺序取首个注册渠道");
            Assert.AreSame(resolved, AdvertisementRegistry.ResolveManager(), "重复解析返回同一缓存实例");
        }

        [Test]
        public void ResolveManager_SingleRegistration_DoesNotWarn()
        {
            var first = new StubAdvertisementManager();
            AdvertisementRegistry.Register(new AdvertisementChannelRegistration("channel-a", "渠道A", () => first));

            Assert.AreSame(first, AdvertisementRegistry.ResolveManager());
        }

        [Test]
        public void ResolveManager_ExplicitChannelId_RoutesToSpecifiedChannel()
        {
            var first = new StubAdvertisementManager();
            var second = new OtherAdvertisementManager();
            AdvertisementRegistry.Register(new AdvertisementChannelRegistration("channel-a", "渠道A", () => first));
            AdvertisementRegistry.Register(new AdvertisementChannelRegistration("channel-b", "渠道B", () => second));

            Assert.AreSame(second, AdvertisementRegistry.ResolveManager("channel-b"), "显式渠道标识可覆盖互斥解析结果");
            Assert.AreSame(first, AdvertisementRegistry.ResolveManager("channel-a"));
            Assert.AreNotSame(AdvertisementRegistry.ResolveManager("channel-b"), AdvertisementRegistry.ResolveManager("channel-a"));
        }

        [Test]
        public void FactoryClosure_InjectsStronglyTypedConfigIntoChannelManager()
        {
            // 配置注入回归：工厂闭包内注入强类型 config，修复前该链路被 Base 吞并（仅 csj 走通）
            StubAdvertisementManager created = null;
            var config = new StubAdvertisementConfig { adUnitId = "stub-ad-unit" };
            AdvertisementRegistry.Register(new AdvertisementChannelRegistration("stub", "存根渠道", () =>
            {
                created = new StubAdvertisementManager();
                created.Initialize(config);
                return created;
            }));

            var resolved = AdvertisementRegistry.ResolveManager();

            Assert.AreSame(created, resolved);
            Assert.AreEqual(1, created.InitializeCallCount);
            Assert.AreSame(config, created.ReceivedConfig, "强类型配置应原样到达渠道管理器初始化");
            Assert.AreEqual("stub-ad-unit", ((StubAdvertisementConfig)created.ReceivedConfig).adUnitId);
        }

        private class StubAdvertisementConfig : AdvertisementConfig
        {
            public string adUnitId;
        }

        [Test]
        public void DefaultManager_ForwardsCallsAndShutdownToResolvedChannel()
        {
            var stub = new StubAdvertisementManager();
            AdvertisementRegistry.Register(new AdvertisementChannelRegistration("stub", "存根渠道", () => stub));
            var defaultManager = new AdvertisementManager();

            defaultManager.Initialize(new StubAdvertisementConfig());
            defaultManager.SetExtraData("key", "value");

            Assert.AreEqual(1, stub.InitializeCallCount, "主包默认实现应把初始化转发给注册表解析结果");
            Assert.AreEqual("key", stub.LastExtraKey);
            Assert.AreEqual("value", stub.LastExtraValue);

            defaultManager.Shutdown();
            Assert.AreEqual(1, stub.ShutdownCallCount, "框架关闭应经主包默认实现转发到渠道的 IShutdownableModule");
        }

        [Test]
        public void DefaultManager_NoChannelRegistered_WarnsOnceAndDoesNotThrow()
        {
            var defaultManager = new AdvertisementManager();

            LogAssert.Expect(LogType.Warning, new System.Text.RegularExpressions.Regex("No advertisement channel registered"));
            defaultManager.Initialize(new StubAdvertisementConfig());
            Assert.DoesNotThrow(() => defaultManager.Shutdown());
        }

        [Test]
        public void CSJBootstrap_Register_RegistersCSJChannelWithConfigInjection()
        {
            // 反射调用渠道包 Bootstrap 的注册方法，验证自注册产物（渠道标识、实例类型全名与强类型配置到达渠道字段）
            var bootstrapType = Type.GetType("GameFrameX.Advertisement.CSJ.Runtime.CSJAdvertisementBootstrap, GameFrameX.Advertisement.CSJ.Runtime");
            Assume.That(bootstrapType, Is.Not.Null, "未安装 csj 渠道包或宏未启用时跳过");

            var configType = Type.GetType("GameFrameX.Advertisement.CSJ.Runtime.CSJAdvertisementConfig, GameFrameX.Advertisement.CSJ.Runtime");
            var config = Activator.CreateInstance(configType);
            configType.GetField("appId").SetValue(config, "test-csj-app");
            configType.GetField("adUnitId").SetValue(config, "test-csj-ad");
            bootstrapType.GetProperty("Config").SetValue(null, config);

            var registerMethod = bootstrapType.GetMethod("Register", BindingFlags.NonPublic | BindingFlags.Static);
            registerMethod.Invoke(null, null);

            Assert.IsTrue(AdvertisementRegistry.Contains("csj"));
            var manager = AdvertisementRegistry.GetManager("csj");
            Assert.AreEqual("GameFrameX.Advertisement.CSJ.Runtime.CSJAdvertisementManager", manager.GetType().FullName);
            Assert.AreEqual("test-csj-app", GetPrivateField(manager, "_appId"), "Bootstrap 工厂闭包注入的 appId 应到达渠道管理器");
            Assert.AreEqual("test-csj-ad", GetPrivateField(manager, "_adUnitId"), "Bootstrap 工厂闭包注入的 adUnitId 应到达渠道管理器");
        }

        [Test]
        public void TopOnBootstrap_Register_RegistersTopOnChannelWithConfigInjection()
        {
            var bootstrapType = Type.GetType("GameFrameX.Advertisement.TopOn.Runtime.TopOnAdvertisementBootstrap, GameFrameX.Advertisement.TopOn.Runtime");
            Assume.That(bootstrapType, Is.Not.Null, "未安装 topon 渠道包时跳过");

            var configType = Type.GetType("GameFrameX.Advertisement.TopOn.Runtime.TopOnAdvertisementConfig, GameFrameX.Advertisement.TopOn.Runtime");
            var config = Activator.CreateInstance(configType);
            configType.GetField("appIdAndroid").SetValue(config, "test-topon-app");
            configType.GetField("appKeyAndroid").SetValue(config, "test-topon-key");
            configType.GetField("adUnitId").SetValue(config, "test-topon-ad");
            bootstrapType.GetProperty("Config").SetValue(null, config);

            var registerMethod = bootstrapType.GetMethod("Register", BindingFlags.NonPublic | BindingFlags.Static);
            registerMethod.Invoke(null, null);

            Assert.IsTrue(AdvertisementRegistry.Contains("topon"));
            // EditMode 无场景 BridgeLink，初始化按预期走编辑器告警分支，但配置字段应先到达管理器
            LogAssert.Expect(LogType.Warning, new System.Text.RegularExpressions.Regex("TopOn Advertisement not support in Editor"));
            var manager = AdvertisementRegistry.GetManager("topon");
            Assert.AreEqual("GameFrameX.Advertisement.TopOn.Runtime.TopOnAdvertisementManager", manager.GetType().FullName);
            Assert.AreEqual("test-topon-app", GetPrivateField(manager, "m_appId"), "appId 应从 TopOnAdvertisementConfig（迁出场景 BridgeLink）到达渠道管理器");
            Assert.AreEqual("test-topon-key", GetPrivateField(manager, "m_appKey"));
            Assert.AreEqual("test-topon-ad", GetPrivateField(manager, "m_adUnitId"));
        }

        [Test]
        public void WechatMiniGameBootstrap_Register_RegistersWechatChannel()
        {
            var bootstrapType = Type.GetType("GameFrameX.Advertisement.WechatMiniGame.Runtime.WechatMiniGameAdvertisementBootstrap, GameFrameX.Advertisement.WechatMiniGame.Runtime");
            Assume.That(bootstrapType, Is.Not.Null, "未安装 wechatminigame 渠道包或宏未启用时跳过");

            var configType = Type.GetType("GameFrameX.Advertisement.WechatMiniGame.Runtime.WechatMiniGameAdvertisementConfig, GameFrameX.Advertisement.WechatMiniGame.Runtime");
            var config = Activator.CreateInstance(configType);
            configType.GetField("adUnitId").SetValue(config, "test-wechat-ad");
            bootstrapType.GetProperty("Config").SetValue(null, config);

            var registerMethod = bootstrapType.GetMethod("Register", BindingFlags.NonPublic | BindingFlags.Static);
            registerMethod.Invoke(null, null);

            Assert.IsTrue(AdvertisementRegistry.Contains("wechatminigame"));
            Assert.AreEqual("GameFrameX.Advertisement.WechatMiniGame.Runtime.WechatMiniGameAdvertisementManager", AdvertisementRegistry.GetManager("wechatminigame").GetType().FullName);
        }

        [Test]
        public void DouYinMiniGameBootstrap_Register_RegistersDouYinChannelWithConfigInjection()
        {
            var bootstrapType = Type.GetType("GameFrameX.Advertisement.DouYinMiniGame.Runtime.DouYinMiniGameAdvertisementBootstrap, GameFrameX.Advertisement.DouYinMiniGame.Runtime");
            Assume.That(bootstrapType, Is.Not.Null, "未安装 douyinminigame 渠道包或宏未启用时跳过");

            var configType = Type.GetType("GameFrameX.Advertisement.DouYinMiniGame.Runtime.DouYinMiniGameAdvertisementConfig, GameFrameX.Advertisement.DouYinMiniGame.Runtime");
            var config = Activator.CreateInstance(configType);
            configType.GetField("adUnitId").SetValue(config, "test-douyin-ad");
            bootstrapType.GetProperty("Config").SetValue(null, config);

            var registerMethod = bootstrapType.GetMethod("Register", BindingFlags.NonPublic | BindingFlags.Static);
            registerMethod.Invoke(null, null);

            Assert.IsTrue(AdvertisementRegistry.Contains("douyinminigame"));
            var manager = AdvertisementRegistry.GetManager("douyinminigame");
            Assert.AreEqual("GameFrameX.Advertisement.DouYinMiniGame.Runtime.DouYinMiniGameAdvertisementManager", manager.GetType().FullName);
            Assert.AreEqual("test-douyin-ad", GetPrivateField(manager, "_adUnitId"), "adUnitId 应自 Bootstrap 工厂闭包到达渠道管理器");
        }

        [Test]
        public void KuaiShouMiniGameBootstrap_Register_RegistersKuaiShouChannelWithConfigInjection()
        {
            var bootstrapType = Type.GetType("GameFrameX.Advertisement.KuaiShouMiniGame.Runtime.KuaiShouMiniGameAdvertisementBootstrap, GameFrameX.Advertisement.KuaiShouMiniGame.Runtime");
            Assume.That(bootstrapType, Is.Not.Null, "未安装 kuaishouminigame 渠道包或宏未启用时跳过");

            var configType = Type.GetType("GameFrameX.Advertisement.KuaiShouMiniGame.Runtime.KuaiShouMiniGameAdvertisementConfig, GameFrameX.Advertisement.KuaiShouMiniGame.Runtime");
            var config = Activator.CreateInstance(configType);
            configType.GetField("adUnitId").SetValue(config, "test-kuaishou-ad");
            bootstrapType.GetProperty("Config").SetValue(null, config);

            var registerMethod = bootstrapType.GetMethod("Register", BindingFlags.NonPublic | BindingFlags.Static);
            registerMethod.Invoke(null, null);

            Assert.IsTrue(AdvertisementRegistry.Contains("kuaishouminigame"));
            var manager = AdvertisementRegistry.GetManager("kuaishouminigame");
            Assert.AreEqual("GameFrameX.Advertisement.KuaiShouMiniGame.Runtime.KuaiShouMiniGameAdvertisementManager", manager.GetType().FullName);
            Assert.AreEqual("test-kuaishou-ad", GetPrivateField(manager, "_adUnitId"), "adUnitId 应自 Bootstrap 工厂闭包到达渠道管理器");
        }

        [Test]
        public void AliPayMiniGameBootstrap_Register_RegistersAliPayChannelWithConfigInjection()
        {
            var bootstrapType = Type.GetType("GameFrameX.Advertisement.AliPayMiniGame.Runtime.AliPayMiniGameAdvertisementBootstrap, GameFrameX.Advertisement.AliPayMiniGame.Runtime");
            Assume.That(bootstrapType, Is.Not.Null, "未安装 alipayminigame 渠道包或宏未启用时跳过");

            var configType = Type.GetType("GameFrameX.Advertisement.AliPayMiniGame.Runtime.AliPayMiniGameAdvertisementConfig, GameFrameX.Advertisement.AliPayMiniGame.Runtime");
            var config = Activator.CreateInstance(configType);
            configType.GetField("adUnitId").SetValue(config, "test-alipay-ad");
            bootstrapType.GetProperty("Config").SetValue(null, config);

            var registerMethod = bootstrapType.GetMethod("Register", BindingFlags.NonPublic | BindingFlags.Static);
            registerMethod.Invoke(null, null);

            Assert.IsTrue(AdvertisementRegistry.Contains("alipayminigame"));
            var manager = AdvertisementRegistry.GetManager("alipayminigame");
            Assert.AreEqual("GameFrameX.Advertisement.AliPayMiniGame.Runtime.AliPayMiniGameAdvertisementManager", manager.GetType().FullName);
            Assert.AreEqual("test-alipay-ad", GetPrivateField(manager, "_adUnitId"), "adUnitId 应自 Bootstrap 工厂闭包到达渠道管理器");
        }

        private static object GetPrivateField(object instance, string fieldName)
        {
            var field = instance.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.IsNotNull(field, $"渠道管理器缺少字段 {fieldName}");
            return field.GetValue(instance);
        }
    }
}
