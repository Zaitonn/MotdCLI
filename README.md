# Motd
基于NET Framework 4.7.2的命令行式Motd获取工具

## 实现参考：
>[Minecraft BedrockEdition Server Motd - 我的世界基岩版服务器Motd协议的API封装](https://github.com/BlackBEDevelopment/MCBE-Server-Motd)  
[MCServerPinger - A light weight project to read the motd of a minecraft sever](https://github.com/Nicolas62x/MCServerPinger)  
[MCBE下基于UDP的RakNet通讯协议原理略谈及利用Python.Socket实现状态查询](https://www.minebbs.com/threads/mcbe-udp-raknet-python-socket.7010/)  

## 许可证
[MIT license](https://github.com/Zaitonn/Motd/blob/master/LICENSE.txt)

## 使用方法
通过使用cmd或作为子进程运行
```
Motd.exe <tcp|udp> <ip> [port]
```
port为空时，根据协议类型自动选择`25565`或`19132`  

## 使用示例
- 在服务器中通过编写相应插件调用来获取其他服务器信息
- 作为面板的外接功能
- 搭配nonebot2食用搭建自己的服务器状态查询机器人
  
## 返回示例

```
>Motd.exe udp 127.0.0.1
MCPE;Dedicated Server;503;1.18.33;0;10;12578007761032183218;Bedrock level;Survival;1;19132;19133;
```
```
>Motd.exe tcp MinecraftOnline.com
? ?{"description":{"text":"§bMinecraftOnline§f - §6Home of Freedonia§r\n§3Survival, Without the Grief!"},"players":{"max":120,"online":1,"sample":[{"id":"a4740a2c-1eec-4b7d-9d22-1c861e7045d7","name":"Biolord101"}]},"version":{"name":"1.12.2","protocol":340},"favicon":"data:image/png;base64,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\nQT5fo+0xAAAAAElFTkSuQmCC"}
```

正常获取到信息时退出返回`0`，否则返回`1`
### TODO
- [x] 基岩版专属服务器
- [x] Java服务器
- [ ] ipv6
