// ==========================================================================================
//   GameFrameX 组织及其衍生项目的版权、商标、专利及其他相关权利
//   GameFrameX organization and its derivative projects' copyrights, trademarks, patents, and related rights
//   均受中华人民共和国及相关国际法律法规保护。
//   are protected by the laws of the People's Republic of China and relevant international regulations.
//   使用本项目须严格遵守相应法律法规及开源许可证之规定。
//   Usage of this project must strictly comply with applicable laws, regulations, and open-source licenses.
//   本项目采用 MIT 许可证与 Apache License 2.0 双许可证分发，
//   This project is dual-licensed under the MIT License and Apache License 2.0,
//   完整许可证文本请参见源代码根目录下的 LICENSE 文件。
//   please refer to the LICENSE file in the root directory of the source code for the full license text.
//   禁止利用本项目实施任何危害国家安全、破坏社会秩序、
//   It is prohibited to use this project to engage in any activities that endanger national security, disrupt social order,
//   侵犯他人合法权益等法律法规所禁止的行为！
//   or infringe upon the legitimate rights and interests of others, as prohibited by laws and regulations!
//   因基于本项目二次开发所产生的一切法律纠纷与责任，
//   Any legal disputes and liabilities arising from secondary development based on this project
//   本项目组织与贡献者概不承担。
//   shall be borne solely by the developer; the project organization and contributors assume no responsibility.
//   GitHub 仓库：https://github.com/GameFrameX
//   GitHub Repository: https://github.com/GameFrameX
//   Gitee  仓库：https://gitee.com/GameFrameX
//   Gitee Repository:  https://gitee.com/GameFrameX
//   CNB  仓库：https://cnb.cool/GameFrameX
//   CNB Repository:  https://cnb.cool/GameFrameX
//   官方文档：https://gameframex.doc.alianblank.com/
//   Official Documentation: https://gameframex.doc.alianblank.com/
//  ==========================================================================================

using System;
using UnityEngine.Scripting;

namespace GameFrameX.Advertisement.Runtime
{
    /// <summary>
    /// 广告播放选项，用于统一承载播放时的回调与扩展数据。
    /// </summary>
    /// <remarks>
    /// Advertisement play options, used to unify callbacks and extended data during ad playback.
    /// </remarks>
    [Preserve]
    [Serializable]
    public class AdvertisementPlayOption
    {
        /// <summary>
        /// 广告播放成功（如奖励验证通过）回调，参数为扩展数据。
        /// </summary>
        /// <remarks>
        /// Callback invoked when the ad plays successfully (e.g., reward verification passed), with extended data as the parameter.
        /// </remarks>
        [Preserve] public Action<string> OnSuccess;

        /// <summary>
        /// 广告播放失败回调，参数为错误信息。
        /// </summary>
        /// <remarks>
        /// Callback invoked when the ad fails to play, with error information as the parameter.
        /// </remarks>
        [Preserve] public Action<string> OnFail;

        /// <summary>
        /// 广告展示结果回调，参数表示是否成功展示。
        /// </summary>
        /// <remarks>
        /// Callback invoked with the ad show result, where the parameter indicates whether the ad was successfully displayed.
        /// </remarks>
        [Preserve] public Action<bool> OnShowResult;

        /// <summary>
        /// 扩展数据，仅在广告加载（Load）阶段透传到服务端奖励验证回调，加载后设置无效。
        /// </summary>
        /// <remarks>
        /// Extended data that is passed through to the server-side reward verification callback only during the Load phase; setting it after loading has no effect.
        /// </remarks>
        [Preserve] public string extraData;
    }
}
