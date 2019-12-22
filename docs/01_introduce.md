# 第一章 分布式基础架构

## 什么是分布式

简单来说，分布式就是，多个计算机共同完成一个任务。比如存储任务，比如计算任务等等。

随着用户体系的不断扩大，单一服务器越来越难以支撑大型项目的运行，主要存在四个方面的原因：

* 磁盘、网络等IO性能瓶颈
* 承载并发量需要扩大
* 项目越来越大，使得代码越来越难以维护
* 项目运行在越多的服务器上，访问权限管理就越麻烦

工程师们为了解决这两个问题使出了浑身解数，对于以上三个问题分别有多种解决方案，其中主流解决方案分别是：

### 应对IO性能瓶颈

对于磁盘IO性能瓶颈的解决方案有：

* 使用多台服务器，通过负载均衡的方式分别读取磁盘数据
* 磁盘阵列（raid 0~N），将磁盘访问压力均匀分布于多个磁盘，读写速度就会有更明显提升
* 云文件系统（Colossus、HDFS等）

对于网络IO性能瓶颈的解决方案有：

* CDN服务，通过用户端IP所在地区均匀分摊请求到不同的机房
* 使用protpbuf减少传输数据体积及使用gzip压缩数据

### 应对承载并发量

这个问题看起来与网络IO具有相似性，但实质不同，此处列举的是在网络IO足够的情况下，无法提升并发量的问题。实际瓶颈可能存在于CPU运算速度限制，也可能存在于数据库读写速度限制，也可能是这两者足够，但因为服务逻辑问题使得无法提升并发量，需要具体场景具体分析。可能的解决方案有：

* 使用网关并开启请求缓存功能
* 使用数据库缓存redis或memcached
* 使用微服务或SOA均匀分摊请求至多台服务器

### 应对代码量过大导致代码杂乱

* 使用DDD领域驱动模型
* 使用微服务架构（与前者不冲突）

### 管理访问权限

* 所有内部服务全部禁止外网访问，由网关统一授权
* 通过数据库或redis之类配置权限，统一管理

## 主流实现方式

目前主流有多种分布式架构，下面来归类整理：

### 面向服务架构 (Service Oriented Architecture, SOA)

推荐指数：⭐

这是最原始的方案，也是最简单的方案，可以在一定层面上缓解请求的压力。这种方式的逻辑是，单体服务复制多份分别在多台计算机中运行，然后通过nginx使用负载均衡的方式分别转发请求。

* ![](https://img.shields.io/github/stars/antirez/redis.svg) [redis](https://github.com/antirez/redis)：内存数据库，多用于存储临时数据，可用于 SOA 架构服务实例间交换数据

### Actor 模型

推荐指数：⭐⭐⭐

这种方案比SOA更优秀，可以使得开发者以简单的方式开发分布式服务，与之衍生的有Virtual Actor模型等。但这种模型相对于微服务架构来说不太完美，微服务天生适合以模块为单位进行拆分，但 Actor 模型容易把代码混一起，有混乱的问题。

以Erlang语言为例，无状态的单个请求与其他请求以资源级别的独立，满足进程的定义，所以通常称之为进程，不过这个进程在操作系统中不可见，几千个进程共同运行在

* ![](https://img.shields.io/github/stars/dotnet/orleans.svg) [orleans](https://github.com/dotnet/orleans)：微软实现的基于 `ASP.Net Core` 的 Virtual Actor 开发框架
* ![](https://img.shields.io/github/stars/dapr/dapr.svg) [dapr](https://github.com/dapr/dapr)：微软实现的基于 Go 语言的 Actor 开发框架，能支持任意语言提供服务

### 服务网格 (Service Mesh)（可实现微服务）

推荐指数：⭐⭐⭐

这种开发方式是，每个微服务进程配套一个反向代理，然后通过反向代理去寻找目标服务的地址。服务间通讯只需要告知目标服务的名称即可。目前这种方式是最优秀的微服务实现方式之一，但还有较长的路要走。

* ![](https://img.shields.io/github/stars/microsoft/service-fabric.svg) [service-fabric](https://github.com/microsoft/service-fabric)：微软实现的基于服务网格的快速开发框架，部分功能与 k8s 重合，可直接搭建服务集群
* ![](https://img.shields.io/github/stars/istio/istio.svg) [istio](https://github.com/istio/istio)：基于 Go 语言的服务网格框架

### CSP 模型（可实现微服务）

推荐指数：⭐⭐⭐⭐

这种模型在 Actor 模型上更进一步，通过消息队列或类似技术，可以使得请求和消费实现松耦合，更方便服务的处理。消息生产者只需要将消息扔进消息队列，消息消费者只需要从消息队列中读取消息自己感兴趣的消息即可。

* ![](https://img.shields.io/github/stars/apache/kafka.svg) [kafka](https://github.com/apache/kafka)：常用消息队列之一，提供完整消息队列功能，需部署独立的消息队列服务器
* ![](https://img.shields.io/github/stars/zeromq/netmq.svg) [ZeroMQ](https://github.com/zeromq/netmq)：世界上最快的消息队列（但常常被当做一种通讯协议），客户端与服务器端均通过调用 sdk 实现，不需要独立服务器
* 与 kafka 类似消息队列关键字：RabbitMQ、ActiveMQ、RocketMQ、ZeroMQ

### 注册中心模式（可实现微服务）

推荐指数：⭐⭐⭐⭐

这是目前实现效果最佳的微服务实现模式，统一由网关控制服务的请求。服务启动后首先会在注册中心注册自己提供的服务类型与。实现方式为，服务首先通过注册中心注册自己的提供服务类型、服务版本，然后在其他需要调用此服务的位置从注册中心查询服务地址，然后再调用服务进行访问。

[Consul+Ocelot 项目示例](../projects/Example00.Consul_Ocelot/README.md)

* ![](https://img.shields.io/github/stars/etcd-io/etcd.svg) [etcd](https://github.com/etcd-io/etcd)：基于 Go 语言实现的 k8s 专用注册中心，在使用 k8s 部署微服务时常使用 etcd 做注册中心的服务发现
* ![](https://img.shields.io/github/stars/hashicorp/consul.svg) [consul](https://github.com/hashicorp/consul)：基于 Go 语言实现的老牌注册中心

## 主流实现方式的区别

### 对外服务

* SOA：通过 nginx 负载均衡对外直接提供服务，安全性一般，性能较好
* Actor：拿 Orleans 举例，直接对外提供集群访问，安全性较差，性能较好；同时也能使用 WebApi 网关方式实现对外统一提供服务，然后由网关模拟 Actor Client 来访问，安全性较好，但性能较差
* 服务网格：通过网关对外提供服务，安全性一般，性能一般
* CSP 模型：通过网关对外提供服务，安全性一般，性能一般
* 注册中心模式：通过网关对外提供服务，安全性一般，性能一般

流行网关：

* ![](https://img.shields.io/github/stars/Kong/kong.svg) [kong](https://github.com/Kong/kong)：微服务常用网关之一
* ![](https://img.shields.io/github/stars/IKende/Bumblebee.svg) [Bumblebee](https://github.com/IKende/Bumblebee)：与 Ocelot 类似的网关，官方宣称性能是 Ocelot 四倍
* ![](https://img.shields.io/github/stars/ThreeMammals/Ocelot.svg) [Ocelot](https://github.com/ThreeMammals/Ocelot)：`ASP.Net Core` 的最流行的网关，但性能不算很好

### 如何将新服务加入集群

* SOA：开发完成后重新编译整套代码，依次下线老服务并上线新服务，升级过程会对生产环境访问有一定影响，可实现灰度更新
* Actor：拿 Orleans 举例，通过将服务接口编译进 Solo，然后依次上线新 Solo并下线老 Solo，升级过程可以做到用户无感知，可实现灰度更新
* 服务网格：上线新服务然后下线老服务即可，升级过程可以做到用户无感知，可实现灰度更新
* CSP 模型：新服务注册信息消费者，然后老服务注销信息消费者，升级过程可以做到用户无感知，可实现灰度更新
* 注册中心模式：新服务注册到注册中心，然后老服务从注册中心移除，升级过程可以做到用户无感知，可实现灰度更新

### 如何获知服务地址及调用服务

* SOA：不需要知道其他实例的地址，直接调用自身服务即可
* Actor：拿 Orleans 举例，通过 Orleans 集群自动分配服务地址
* 服务网格：通过反向代理自动寻找服务地址
* CSP 模型：不需要知道服务地址，直接向消息循环投递即可
* 注册中心模式：通过注册中心查询服务地址，然后通过RPC调用

### 如何获知服务掉线

* SOA：nginx 循环转发请求期间假如始终有一个比例的请求访问失败并返回502，那么说明 nginx 转发到的服务地址很可能已经掉线，对生产环境有影响
* Actor：拿 Orleans 举例，掉线后 Orleans 集群自动移除问题主机，在生产环境下可以自动适应并收缩，可以使得对用户感知几乎没有影响
* 服务网格：掉线后被其他反向代理给监听到，然后同步给所有反向代理，在生产环境下可以自动适应并收缩，可以使得对用户感知几乎没有影响
* CSP 模型：掉线后就不再消费队列中的消息，对生产环境几乎没有影响
* 注册中心模式：掉线后被健康检查发现，然后移除节点，在生产环境下可以自动适应并收缩，可以使得对用户感知几乎没有影响

[返回首页](../README.md) | [上一章 序言](./00_startup.md) | [下一章 通信协议](./02_protocol.md)
