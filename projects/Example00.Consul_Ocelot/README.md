# Consul 注册中心与 Ocelot 网关示例项目

## 使用方式

1. 下载 [consul.exe](https://www.consul.io/downloads.html)，并通过命令运行 `consul agent -dev`
2. 打开项目，编译
3. 分别进入 bin 文件夹，执行以下命令：  
  `Example00.Consul_Ocelot.Server.exe --port="6666"`  
  `Example00.Consul_Ocelot.Server.exe --port="6667"`  
  `Example00.Consul_Ocelot.Server.exe --port="6668"`
4. 启动项目 `Example00.Consul_Ocelot.Gateway`，如果没有弹出 consul 服务注册页面，那么手动在浏览器中打开 <http://127.0.0.1:8500/>
5. 此时应该可以看到三个服务的健康检查被调用，如果访问失败那么检查 consul 的网络，然后应该可以看到注册上去的服务
6. 通过访问 <http://127.0.0.1:6665/Test> 即可看到效果
