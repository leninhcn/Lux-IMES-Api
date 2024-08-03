<<<<<<< HEAD
<h2 align="center"> ZR.Admin.NET后台管理系统</h2>
<h4 align="center">基于.Net7 + vue2.x/vue3.x/uniapp前后端分离的.net快速开发框架</h4>

<p align="center">
	<a href="https://gitee.com/izory/ZrAdminNetCore"><img src="https://gitee.com/izory/ZrAdminNetCore/badge/star.svg?theme=dark"></a>
<a href='https://gitee.com/izory/ZrAdminNetCore/members'><img src='https://gitee.com/izory/ZrAdminNetCore/badge/fork.svg?theme=dark' alt='fork'></img></a>
	<a href="https://gitee.com/izory/ZrAdminNetCore/blob/master/LICENSE"><img src="https://img.shields.io/github/license/mashape/apistatus.svg"></a>
	<a href="http://www.izhaorui.cn/doc/changelog.html"><img src="https://img.shields.io/badge/更新日志-20230920-yellow"/></a>
</p>

---

<div align="center">
	<p><strong><a href="README.md">简体中文</a> | <a href="README.en.md">English</a></strong></p>
</div>

---

## 🍟 概述

- 本项目适合有一定 NetCore 和 vue 基础的开发人员
- 基于.NET5/.NET7 实现的通用权限管理平台（RBAC 模式）。整合最新技术高效快速开发，前后端分离模式，开箱即用。
- 代码量少、学习简单、通俗易懂、功能强大、易扩展、轻量级，让 web 开发更快速、简单高效（从此告别 996），解决 70%的重复工作，专注您的业务，轻松开发从现在开始！
- 提供了技术栈(Ant Design Vue)版[Ant Design Vue](https://gitee.com/billzh/mc-dull.git)
- 七牛云通用云产品优惠券：[点我进入](https://s.qiniu.com/FzEfay)。
- 腾讯云秒杀场：[☛☛ 点我进入 ☚☚](https://curl.qcloud.com/4yEoRquq)。
- 腾讯云优惠券：[☛☛ 点我领取 ☚☚](https://curl.qcloud.com/5J4nag8D)。
- 阿里云特惠专区：[☛☛ 点我进入 ☚☚](https://www.aliyun.com/minisite/goods?userCode=uotn5vt1&share_source=copy_link)

```
如果对您有帮助，您可以点右上角 “Star” 收藏一下 ，谢谢！~
```

## 🍿 在线体验

- 官方文档：http://www.izhaorui.cn/doc
- 加入群聊：[立即加入](http://www.izhaorui.cn/doc/contact.html)
- Vue3.x 版本体验：[http://www.izhaorui.cn/vue3](http://www.izhaorui.cn/vue3)
- Vue2.x 版本体验：[http://www.izhaorui.cn/admin](http://www.izhaorui.cn/admin)
- Uniapp 版本体验：[http://www.izhaorui.cn/h5](http://www.izhaorui.cn/h5)
- 账号密码：admin/123456，普通用户 user/123456

| H5                                                                                     | 微信小程序                                                                           |
| -------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------ |
| ![alt](https://gitee.com/izory/ZrAdminNetCore/raw/master/document/images/qrcodeH5.png) | ![alt](https://gitee.com/izory/ZrAdminNetCore/raw/master/document/images/qrcode.jpg) |

```
由于是个人项目，资金有限，体验服务器是低配，请大家爱惜，轻戳，不胜感激！！！
```

## 💒 代码仓库

| 仓库           | Github                                                 | Gitee                                               |
| -------------- | ------------------------------------------------------ | --------------------------------------------------- |
| Net7           | [克隆/下载](https://github.com/izhaorui/Zr.Admin.NET)  | [克隆/下载](https://gitee.com/izory/ZrAdminNetCore) |
| Vue3(推荐使用) | [克隆/下载](https://github.com/izhaorui/ZR.Admin.Vue3) | [克隆/下载](https://gitee.com/izory/ZRAdmin-vue)    |

## 🍁 前端技术

Vue 版前端技术栈 ：基于 vue2.x/vue3.x/uniapp、vuex、vue-router 、vue-cli 、axios、 element-ui、echats、i18n 国际化等，前端采用 vscode 工具开发

## 🍀 后端技术

- 核心框架：.Net5.0/.Net7.0 + Web API + sqlsugar + swagger + signalR + IpRateLimit + Quartz.net + Redis
- 定时计划任务：Quartz.Net 组件，支持执行程序集或者 http 网络请求
- 安全支持：过滤器(数据权限过滤)、Sql 注入、请求伪造
- 日志管理：NLog、登录日志、操作日志、定时任务日志
- 工具类：验证码、丰富公共功能
- 接口限流：支持接口限流，避免恶意请求导致服务层压力过大
- 代码生成：高效率开发，代码生成器可以一键生成所有前后端代码
- 数据字典：支持数据字典，可以方便对一些状态进行管理
- 分库分表：使用 orm sqlsugar 可以很轻松的实现分库分库性能优越
- 多 租 户：支持多租户功能
- 缓存数据：内置内存缓存和 Redis
- signalR：使用 signalr 管理用户在线状态

## 🍖 内置功能

1. 用户管理：用户是系统操作者，该功能主要完成系统用户配置。
2. 部门管理：配置系统组织机构（公司、部门、小组），树结构展现。
3. 岗位管理：配置系统用户所属担任职务。
4. 菜单管理：配置系统菜单，操作权限，按钮权限标识等。
5. 角色管理：角色菜单权限分配。
6. 字典管理：对系统中经常使用的一些较为固定的数据进行维护。
7. 操作日志：系统正常操作日志记录和查询；系统异常信息日志记录和查询。
8. 登录日志：系统登录日志记录查询包含登录异常。
9. 系统接口：使用 swagger 生成相关 api 接口文档。
10. 服务监控：监视当前系统 CPU、内存、磁盘、堆栈等相关信息。
11. 在线构建器：拖动表单元素生成相应的 VUE 代码(仅支持 vue2)。
12. 任务系统：基于 Quartz.NET，可以在线（添加、修改、删除、手动执行)任务调度包含执行结果日志。
13. 文章管理：可以写文章记录。
14. 代码生成：可以一键生成前后端代码(.cs、.vue、.js、.sql、uniapp 等)支持下载，自定义配置前端展示控件、让开发更快捷高效（史上最强）。
15. 参数管理：对系统动态配置常用参数。
16. 发送邮件：可以对多个用户进行发送邮件。
17. 文件管理：可以进行上传文件管理，目前支持上传到本地、阿里云。
18. 通知管理：系统通知公告信息发布维护，使用 signalr 实现对用户实时通知。
19. 账号注册：可以注册账号登录系统。
20. 多语言管理：支持静态、后端动态配置国际化。目前只支持中、英、繁体(仅支持 vue3)
21. 在线用户：可以查看正在登录使用的用户，可以对其踢出、通知操作
22. db 审计日志：数据库审计功能
23. 三方登录：提供三方登录实现逻辑

## 🍻 项目结构

![alt](https://gitee.com/izory/ZrAdminNetCore/raw/master/document/images/kj.png)

```
├─ZR.Service                ->[你的业务服务层类库]：提供WebApi接口调用；
├─ZR.ServiceCore            ->[系统服务层类库]：提供WebApi接口调用；
├─ZR.Repository             ->[仓库层类库]：方便提供有执行存储过程的操作；
├─ZR.Model                	->[实体层类库]：提供项目中的数据库表、数据传输对象；
├─ZR.Admin.WebApi           ->[webapi接口]：为Vue版或其他三方系统提供接口服务。
├─ZR.Tasks               		->[定时任务类库]：提供项目定时任务实现功能；
├─ZR.CodeGenerator          ->[代码生成功能]：包含代码生成的模板、方法、代码生成的下载。
├─ZR.Vue               			->[前端UI]：vue2.0版本UI层(已经不再更新推荐使用vue3)。
├─document               		->[文档]：数据库脚本
```

## 🍎 演示图

<table>
    <tr>
        <td><img src="https://gitee.com/izory/ZrAdminNetCore/raw/master/document/images/1.png"/></td>
        <td><img src="https://gitee.com/izory/ZrAdminNetCore/raw/master/document/images/2.png"/></td>
    </tr>
    <tr>
        <td><img src="https://gitee.com/izory/ZrAdminNetCore/raw/master/document/images/3.png"/></td>
        <td><img src="https://gitee.com/izory/ZrAdminNetCore/raw/master/document/images/4.png"/></td>
    </tr>
    <tr>
        <td><img src="https://gitee.com/izory/ZrAdminNetCore/raw/master/document/images/5.png"/></td>
        <td><img src="https://gitee.com/izory/ZrAdminNetCore/raw/master/document/images/6.png"/></td>
    </tr>
	<tr>
        <td><img src="https://gitee.com/izory/ZrAdminNetCore/raw/master/document/images/7.png"/></td>
        <td><img src="https://gitee.com/izory/ZrAdminNetCore/raw/master/document/images/8.png"/></td>
    </tr>	
	<tr>
        <td><img src="https://gitee.com/izory/ZrAdminNetCore/raw/master/document/images/9.png"/></td>
        <td><img src="https://gitee.com/izory/ZrAdminNetCore/raw/master/document/images/10.png"/></td>
    </tr>
	<tr>
        <td><img src="https://gitee.com/izory/ZrAdminNetCore/raw/master/document/images/11.png"/></td>
        <td><img src="https://gitee.com/izory/ZrAdminNetCore/raw/master/document/images/12.png"/></td>
    </tr>
	<tr>
        <td><img src="https://gitee.com/izory/ZrAdminNetCore/raw/master/document/images/13.png"/></td>
        <td><img src="https://gitee.com/izory/ZrAdminNetCore/raw/master/document/images/14.png"/></td>
    </tr>
	<tr>
        <td><img src="https://gitee.com/izory/ZrAdminNetCore/raw/master/document/images/15.png"/></td>
        <td><img src="https://gitee.com/izory/ZrAdminNetCore/raw/master/document/images/16.png"/></td>
    </tr>
	<tr>
        <td><img src="https://gitee.com/izory/ZrAdminNetCore/raw/master/document/images/17.png"/></td>
				<td><img src="https://gitee.com/izory/ZrAdminNetCore/raw/master/document/images/18.png"/></td>
    </tr>
	<tr>
		<td><img src="https://gitee.com/izory/ZrAdminNetCore/raw/master/document/images/19.png"/></td>
		<td><img src="https://gitee.com/izory/ZrAdminNetCore/raw/master/document/images/20.png"/></td>
	</tr>
	<tr>
		<td><img src="https://gitee.com/izory/ZrAdminNetCore/raw/master/document/images/21.png"/></td>
	</tr>
</table>

## 移动端演示图

<table>
    <tr>
        <td><img src="https://gitee.com/izory/ZrAdminNetCore/raw/master/document/images/a1.png"/></td>
        <td><img src="https://gitee.com/izory/ZrAdminNetCore/raw/master/document/images/a2.png"/></td>
    </tr>
    <tr>
        <td><img src="https://gitee.com/izory/ZrAdminNetCore/raw/master/document/images/a8.png"/></td>
        <td><img src="https://gitee.com/izory/ZrAdminNetCore/raw/master/document/images/a4.png"/></td>
    </tr>
    <tr>
        <td><img src="https://gitee.com/izory/ZrAdminNetCore/raw/master/document/images/a5.png"/></td>
        <td><img src="https://gitee.com/izory/ZrAdminNetCore/raw/master/document/images/a6.png"/></td>
    </tr>
		<tr>
        <td><img src="https://gitee.com/izory/ZrAdminNetCore/raw/master/document/images/a7.png"/></td>
        <td><img src="https://gitee.com/izory/ZrAdminNetCore/raw/master/document/images/a9.png"/></td>
    </tr>
<tr>
        <td><img src="https://gitee.com/izory/ZrAdminNetCore/raw/master/document/images/a10.png"/></td>
    </tr>

</table>

## 🎉 优势

1. 前台系统不用编写登录、授权、认证模块；只负责编写业务模块即可
2. 后台系统无需任何二次开发，直接发布即可使用
3. 前台与后台系统分离，分别为不同的系统（域名可独立）
4. 全局异常统一处理
5. 自定义的代码生成功能
6. 依赖少，上手容易
7. 文档全面
8. 支持中文表头导入数据

## 💐 特别鸣谢

- 👉Ruoyi.vue：[Ruoyi](http://www.ruoyi.vip/)
- 👉SqlSugar：[SqlSugar](https://gitee.com/dotnetchina/SqlSugar)
- 👉vue-element-admin：[vue-element-admin](https://github.com/PanJiaChen/vue-element-admin)
- 👉Meiam.System：[Meiam.System](https://github.com/91270/Meiam.System)
- 👉Furion：[Furion](https://gitee.com/dotnetchina/Furion)

## 🎀 捐赠

如果你觉得这个项目帮助到了你，你可以请作者喝杯咖啡表示鼓励 ☕️
<img src="https://gitee.com/izory/ZrAdminNetCore/raw/master/document/images/pay.jpg"/>
# Lux-IMES-Api
Luxshare IMES Project
=======
# imes-api



## Getting started

To make it easy for you to get started with GitLab, here's a list of recommended next steps.

Already a pro? Just edit this README.md and make it your own. Want to make it easy? [Use the template at the bottom](#editing-this-readme)!

## Add your files

- [ ] [Create](https://docs.gitlab.com/ee/user/project/repository/web_editor.html#create-a-file) or [upload](https://docs.gitlab.com/ee/user/project/repository/web_editor.html#upload-a-file) files
- [ ] [Add files using the command line](https://docs.gitlab.com/ee/gitlab-basics/add-file.html#add-a-file-using-the-command-line) or push an existing Git repository with the following command:

```
cd existing_repo
git remote add origin http://172.23.24.207:6688/leninhcn/imes-api.git
git branch -M main
git push -uf origin main
```

## Integrate with your tools

- [ ] [Set up project integrations](http://172.23.24.207:6688/leninhcn/imes-api/-/settings/integrations)

## Collaborate with your team

- [ ] [Invite team members and collaborators](https://docs.gitlab.com/ee/user/project/members/)
- [ ] [Create a new merge request](https://docs.gitlab.com/ee/user/project/merge_requests/creating_merge_requests.html)
- [ ] [Automatically close issues from merge requests](https://docs.gitlab.com/ee/user/project/issues/managing_issues.html#closing-issues-automatically)
- [ ] [Enable merge request approvals](https://docs.gitlab.com/ee/user/project/merge_requests/approvals/)
- [ ] [Set auto-merge](https://docs.gitlab.com/ee/user/project/merge_requests/merge_when_pipeline_succeeds.html)

## Test and Deploy

Use the built-in continuous integration in GitLab.

- [ ] [Get started with GitLab CI/CD](https://docs.gitlab.com/ee/ci/quick_start/index.html)
- [ ] [Analyze your code for known vulnerabilities with Static Application Security Testing (SAST)](https://docs.gitlab.com/ee/user/application_security/sast/)
- [ ] [Deploy to Kubernetes, Amazon EC2, or Amazon ECS using Auto Deploy](https://docs.gitlab.com/ee/topics/autodevops/requirements.html)
- [ ] [Use pull-based deployments for improved Kubernetes management](https://docs.gitlab.com/ee/user/clusters/agent/)
- [ ] [Set up protected environments](https://docs.gitlab.com/ee/ci/environments/protected_environments.html)

***

# Editing this README

When you're ready to make this README your own, just edit this file and use the handy template below (or feel free to structure it however you want - this is just a starting point!). Thanks to [makeareadme.com](https://www.makeareadme.com/) for this template.

## Suggestions for a good README

Every project is different, so consider which of these sections apply to yours. The sections used in the template are suggestions for most open source projects. Also keep in mind that while a README can be too long and detailed, too long is better than too short. If you think your README is too long, consider utilizing another form of documentation rather than cutting out information.

## Name
Choose a self-explaining name for your project.

## Description
Let people know what your project can do specifically. Provide context and add a link to any reference visitors might be unfamiliar with. A list of Features or a Background subsection can also be added here. If there are alternatives to your project, this is a good place to list differentiating factors.

## Badges
On some READMEs, you may see small images that convey metadata, such as whether or not all the tests are passing for the project. You can use Shields to add some to your README. Many services also have instructions for adding a badge.

## Visuals
Depending on what you are making, it can be a good idea to include screenshots or even a video (you'll frequently see GIFs rather than actual videos). Tools like ttygif can help, but check out Asciinema for a more sophisticated method.

## Installation
Within a particular ecosystem, there may be a common way of installing things, such as using Yarn, NuGet, or Homebrew. However, consider the possibility that whoever is reading your README is a novice and would like more guidance. Listing specific steps helps remove ambiguity and gets people to using your project as quickly as possible. If it only runs in a specific context like a particular programming language version or operating system or has dependencies that have to be installed manually, also add a Requirements subsection.

## Usage
Use examples liberally, and show the expected output if you can. It's helpful to have inline the smallest example of usage that you can demonstrate, while providing links to more sophisticated examples if they are too long to reasonably include in the README.

## Support
Tell people where they can go to for help. It can be any combination of an issue tracker, a chat room, an email address, etc.

## Roadmap
If you have ideas for releases in the future, it is a good idea to list them in the README.

## Contributing
State if you are open to contributions and what your requirements are for accepting them.

For people who want to make changes to your project, it's helpful to have some documentation on how to get started. Perhaps there is a script that they should run or some environment variables that they need to set. Make these steps explicit. These instructions could also be useful to your future self.

You can also document commands to lint the code or run tests. These steps help to ensure high code quality and reduce the likelihood that the changes inadvertently break something. Having instructions for running tests is especially helpful if it requires external setup, such as starting a Selenium server for testing in a browser.

## Authors and acknowledgment
Show your appreciation to those who have contributed to the project.

## License
For open source projects, say how it is licensed.

## Project status
If you have run out of energy or time for your project, put a note at the top of the README saying that development has slowed down or stopped completely. Someone may choose to fork your project or volunteer to step in as a maintainer or owner, allowing your project to keep going. You can also make an explicit request for maintainers.
>>>>>>> fcbf29c (Initial commit)
